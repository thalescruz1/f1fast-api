// ============================================================
// SERVICE: NotificacaoService
// ============================================================
// Responsável por enviar e-mails de lembrete para participantes
// que ainda não fizeram palpite quando o prazo está próximo.
//
// Este service é chamado pelo LembreteBackgroundService a cada
// 30 minutos. Dois tipos de lembrete são enviados:
//   1) Quarta-feira anterior à corrida, às 09:00 BRT (lembrete geral)
//   2) 30 minutos antes do PrazoQualify (lembrete urgente/última chance)
// Flags LembreteEnviado e LembreteUrgenteEnviado evitam duplicatas.
// ============================================================

using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using static F1Fast.API.Helpers.DateTimeHelper;

namespace F1Fast.API.Services;

public class NotificacaoService(AppDbContext db, IConfiguration config, ILogger<NotificacaoService> logger)
{
    /// <summary>
    /// Verifica se hoje é a quarta-feira anterior a alguma corrida (a partir das 09:00 UTC)
    /// e envia e-mails de lembrete para participantes sem palpite.
    /// A flag LembreteEnviado evita envios duplicados.
    /// </summary>
    public async Task EnviarLembretesAsync()
    {
        var agora = AgoraBRT;

        // Só dispara a partir das 09:00 BRT e apenas às quartas-feiras
        if (agora.DayOfWeek != DayOfWeek.Wednesday || agora.Hour < 9)
            return;

        // Busca a próxima etapa não encerrada, que ainda não teve lembrete enviado,
        // e cuja DataCorrida é futura (a corrida ainda não aconteceu)
        var proximaEtapa = await db.Etapas
            .Where(e => !e.Encerrada
                     && !e.Cancelada
                     && !e.LembreteEnviado
                     && e.DataCorrida != null
                     && e.DataCorrida > agora)
            .OrderBy(e => e.DataCorrida)
            .FirstOrDefaultAsync();

        if (proximaEtapa is null) return; // nenhuma etapa pendente

        // Calcula a quarta-feira imediatamente anterior à corrida
        // Ex: corrida domingo 22/03 → quarta-feira seria 18/03
        var dataCorrida       = proximaEtapa.DataCorrida!.Value.Date;
        var diasAteQuarta     = ((int)dataCorrida.DayOfWeek - (int)DayOfWeek.Wednesday + 7) % 7;
        var quartaAnterior    = dataCorrida.AddDays(-diasAteQuarta);

        // Se hoje NÃO é a quarta-feira desta corrida, não envia
        if (agora.Date != quartaAnterior)
            return;

        // IQueryable (não executado ainda): IDs dos usuários QUE JÁ fizeram palpite
        var usuariosComPalpite = db.Palpites
            .Where(p => p.EtapaId == proximaEtapa.Id)
            .Select(p => p.UsuarioId);

        // Busca usuários que NÃO estão na lista acima (os que ainda não palpitaram)
        // ".Contains(u.Id)" vira um SQL "WHERE u.Id NOT IN (...)" eficientemente
        var semPalpite = await db.Usuarios
            .Where(u => !usuariosComPalpite.Contains(u.Id))
            .ToListAsync();

        // Envia e-mail individual para cada usuário sem palpite
        foreach (var usuario in semPalpite)
        {
            try
            {
                await EnviarEmailAsync(
                    para:    usuario.Email,
                    assunto: $"⏱ F1Fast — Prazo do palpite termina em breve! ({proximaEtapa.Nome})",
                    corpo:   GerarEmailLembreteHtml(
                        usuario.Nome,
                        proximaEtapa.Nome,
                        proximaEtapa.PrazoQualify.ToString("dd/MM/yyyy HH:mm"))
                );
            }
            catch (Exception ex)
            {
                // Se falhar para um usuário, loga o erro e continua para o próximo
                logger.LogError(ex, "Erro ao enviar e-mail para {Email}", usuario.Email);
            }
        }

        // Marca a etapa como "lembrete já enviado" para não reenviar
        proximaEtapa.LembreteEnviado = true;
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Lembrete urgente: 30 minutos antes do PrazoQualify, envia um último
    /// aviso para quem AINDA não fez o palpite. Tom mais urgente que o lembrete
    /// de quarta-feira. Flag LembreteUrgenteEnviado evita duplicatas.
    /// </summary>
    public async Task EnviarLembretesUrgentesAsync()
    {
        var agora  = AgoraBRT;
        var limite = agora.AddMinutes(45); // janela de 45 min para cobrir o intervalo de 30 min do background service

        // Busca etapa cujo PrazoQualify está nos próximos 45 minutos e ainda não teve lembrete urgente
        var proximaEtapa = await db.Etapas
            .Where(e => !e.Encerrada
                     && !e.Cancelada
                     && !e.LembreteUrgenteEnviado
                     && e.PrazoQualify > agora
                     && e.PrazoQualify <= limite)
            .OrderBy(e => e.PrazoQualify)
            .FirstOrDefaultAsync();

        if (proximaEtapa is null) return;

        // Busca usuários que AINDA não fizeram palpite
        var usuariosComPalpite = db.Palpites
            .Where(p => p.EtapaId == proximaEtapa.Id)
            .Select(p => p.UsuarioId);

        var semPalpite = await db.Usuarios
            .Where(u => !usuariosComPalpite.Contains(u.Id))
            .ToListAsync();

        foreach (var usuario in semPalpite)
        {
            try
            {
                await EnviarEmailAsync(
                    para:    usuario.Email,
                    assunto: $"🚨 F1Fast — Última chance! Prazo encerra em minutos ({proximaEtapa.Nome})",
                    corpo:   GerarEmailLembreteUrgenteHtml(
                        usuario.Nome,
                        proximaEtapa.Nome,
                        proximaEtapa.PrazoQualify.ToString("dd/MM/yyyy HH:mm"))
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao enviar lembrete urgente para {Email}", usuario.Email);
            }
        }

        // Marca como enviado para não reenviar
        proximaEtapa.LembreteUrgenteEnviado = true;
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Método auxiliar privado para envio de e-mail HTML via SMTP.
    /// </summary>
    private async Task EnviarEmailAsync(string para, string assunto, string corpo)
    {
        using var client = new System.Net.Mail.SmtpClient(
            config["Smtp:Host"], int.Parse(config["Smtp:Port"]!));

        client.Credentials = new System.Net.NetworkCredential(config["Smtp:User"], config["Smtp:Pass"]);
        client.EnableSsl   = true; // conexão segura (TLS)

        var message = new System.Net.Mail.MailMessage(config["Smtp:From"]!, para, assunto, corpo)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }

    /// <summary>
    /// Gera o HTML do e-mail de lembrete de palpite com layout F1Fast.
    /// </summary>
    private static string GerarEmailLembreteHtml(string nome, string nomeEtapa, string prazo) => $@"
<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""></head>
<body style=""margin:0;padding:0;background-color:#1A1A1A;font-family:Inter,Arial,sans-serif;"">
<table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#1A1A1A;padding:32px 16px;"">
<tr><td align=""center"">
<table role=""presentation"" width=""600"" cellpadding=""0"" cellspacing=""0"" style=""max-width:600px;width:100%;border-radius:8px;overflow:hidden;"">

  <!-- Barra vermelha F1 -->
  <tr><td style=""background-color:#E10600;height:4px;font-size:0;line-height:0;"">&nbsp;</td></tr>

  <!-- Header escuro com logo -->
  <tr><td style=""background-color:#1A1A1A;padding:24px 32px;text-align:center;"">
    <img src=""https://novo.f1fast.com.br/logo.png"" alt=""F1Fast"" width=""48"" style=""display:inline-block;vertical-align:middle;margin-right:12px;"" />
    <span style=""font-family:'Arial Black',Impact,sans-serif;font-size:24px;color:#FFFFFF;letter-spacing:3px;vertical-align:middle;"">F1FAST</span>
  </td></tr>

  <!-- Corpo branco -->
  <tr><td style=""background-color:#FFFFFF;padding:40px 32px;"">
    <p style=""margin:0 0 8px;font-size:14px;color:#9E9E9E;text-transform:uppercase;letter-spacing:2px;"">🏁 Lembrete de palpite</p>
    <h1 style=""margin:0 0 24px;font-size:22px;color:#1A1A1A;font-weight:700;"">Olá, {nome}!</h1>
    <p style=""margin:0 0 16px;font-size:15px;color:#333333;line-height:1.6;"">
      O prazo para enviar seu palpite está chegando ao fim. Não fique de fora!
    </p>

    <!-- Card da etapa -->
    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""margin:24px 0;"">
    <tr><td style=""background-color:#F3F3F3;border-left:4px solid #E10600;border-radius:4px;padding:16px 20px;"">
      <p style=""margin:0 0 4px;font-size:12px;color:#9E9E9E;text-transform:uppercase;letter-spacing:1px;"">Etapa</p>
      <p style=""margin:0 0 12px;font-size:18px;color:#1A1A1A;font-weight:700;font-family:'Arial Black',Impact,sans-serif;"">{nomeEtapa}</p>
      <p style=""margin:0;font-size:13px;color:#333333;"">Prazo: <strong style=""color:#E10600;"">{prazo} (Horário de Brasília)</strong></p>
    </td></tr>
    </table>

    <!-- Botão CTA -->
    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" style=""margin:32px auto;"">
    <tr><td style=""background-color:#0057E1;border-radius:6px;"">
      <a href=""https://novo.f1fast.com.br"" target=""_blank"" style=""display:inline-block;padding:14px 32px;color:#FFFFFF;text-decoration:none;font-family:'Arial Black',Impact,sans-serif;font-size:14px;letter-spacing:2px;text-transform:uppercase;"">
        FAZER MEU PALPITE
      </a>
    </td></tr>
    </table>

    <p style=""margin:0;font-size:15px;color:#1A1A1A;text-align:center;"">Boa sorte! 🍀</p>
  </td></tr>

  <!-- Footer -->
  <tr><td style=""background-color:#F3F3F3;padding:20px 32px;text-align:center;"">
    <p style=""margin:0;font-size:12px;color:#9E9E9E;"">
      © {AgoraBRT.Year} F1Fast — Todos os direitos reservados
    </p>
  </td></tr>

</table>
</td></tr>
</table>
</body>
</html>";

    /// <summary>
    /// Gera o HTML do e-mail de lembrete URGENTE (30 min antes do qualify).
    /// Visual mais alarmante: botão vermelho, destaque de urgência.
    /// </summary>
    private static string GerarEmailLembreteUrgenteHtml(string nome, string nomeEtapa, string prazo) => $@"
<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""></head>
<body style=""margin:0;padding:0;background-color:#1A1A1A;font-family:Inter,Arial,sans-serif;"">
<table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#1A1A1A;padding:32px 16px;"">
<tr><td align=""center"">
<table role=""presentation"" width=""600"" cellpadding=""0"" cellspacing=""0"" style=""max-width:600px;width:100%;border-radius:8px;overflow:hidden;"">

  <!-- Barra vermelha F1 -->
  <tr><td style=""background-color:#E10600;height:4px;font-size:0;line-height:0;"">&nbsp;</td></tr>

  <!-- Header escuro com logo -->
  <tr><td style=""background-color:#1A1A1A;padding:24px 32px;text-align:center;"">
    <img src=""https://novo.f1fast.com.br/logo.png"" alt=""F1Fast"" width=""48"" style=""display:inline-block;vertical-align:middle;margin-right:12px;"" />
    <span style=""font-family:'Arial Black',Impact,sans-serif;font-size:24px;color:#FFFFFF;letter-spacing:3px;vertical-align:middle;"">F1FAST</span>
  </td></tr>

  <!-- Corpo branco -->
  <tr><td style=""background-color:#FFFFFF;padding:40px 32px;"">
    <p style=""margin:0 0 8px;font-size:14px;color:#E10600;text-transform:uppercase;letter-spacing:2px;font-weight:700;"">🚨 Última chance!</p>
    <h1 style=""margin:0 0 24px;font-size:22px;color:#1A1A1A;font-weight:700;"">Olá, {nome}!</h1>
    <p style=""margin:0 0 16px;font-size:15px;color:#333333;line-height:1.6;"">
      O prazo para enviar seu palpite encerra em <strong style=""color:#E10600;"">poucos minutos</strong>. Corre que ainda dá tempo!
    </p>

    <!-- Card da etapa com borda vermelha mais grossa -->
    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""margin:24px 0;"">
    <tr><td style=""background-color:#FFF5F5;border-left:4px solid #E10600;border-radius:4px;padding:16px 20px;"">
      <p style=""margin:0 0 4px;font-size:12px;color:#9E9E9E;text-transform:uppercase;letter-spacing:1px;"">Etapa</p>
      <p style=""margin:0 0 12px;font-size:18px;color:#1A1A1A;font-weight:700;font-family:'Arial Black',Impact,sans-serif;"">{nomeEtapa}</p>
      <p style=""margin:0;font-size:13px;color:#333333;"">Prazo: <strong style=""color:#E10600;"">{prazo} (Horário de Brasília)</strong></p>
    </td></tr>
    </table>

    <!-- Botão CTA VERMELHO (urgência) -->
    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" style=""margin:32px auto;"">
    <tr><td style=""background-color:#E10600;border-radius:6px;"">
      <a href=""https://novo.f1fast.com.br"" target=""_blank"" style=""display:inline-block;padding:14px 32px;color:#FFFFFF;text-decoration:none;font-family:'Arial Black',Impact,sans-serif;font-size:14px;letter-spacing:2px;text-transform:uppercase;"">
        FAZER MEU PALPITE AGORA
      </a>
    </td></tr>
    </table>

    <p style=""margin:0;font-size:13px;color:#9E9E9E;text-align:center;"">Não perca pontos — cada palpite conta!</p>
  </td></tr>

  <!-- Footer -->
  <tr><td style=""background-color:#F3F3F3;padding:20px 32px;text-align:center;"">
    <p style=""margin:0;font-size:12px;color:#9E9E9E;"">
      © {AgoraBRT.Year} F1Fast — Todos os direitos reservados
    </p>
  </td></tr>

</table>
</td></tr>
</table>
</body>
</html>";
}
