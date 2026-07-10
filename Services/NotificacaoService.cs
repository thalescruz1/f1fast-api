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
using F1Fast.API.Models;
using static F1Fast.API.Helpers.DateTimeHelper;

namespace F1Fast.API.Services;

public class NotificacaoService(AppDbContext db, ILogger<NotificacaoService> logger, AuditoriaService audit, EmailService emailService, PushNotificationService push, IConfiguration config)
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

        // Busca TODOS os usuários — lembrete de quarta vai pra todo mundo
        var todosUsuarios = await db.Usuarios.ToListAsync();

        // Envia e-mail individual para cada usuário
        foreach (var usuario in todosUsuarios)
        {
            try
            {
                await emailService.EnviarAsync(
                    usuario.Email, $"⏱ F1Fast — Prazo do palpite termina em breve! ({proximaEtapa.Nome})",
                    GerarEmailLembreteHtml(
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

        // Push para todos os usuários (mesmo público do e-mail)
        await push.EnviarParaUsuariosAsync(
            todosUsuarios.Select(u => u.Id),
            new PushNotificationService.PushPayload(
                "⏱ Prazo do palpite chegando",
                $"Não esqueça de palpitar no {proximaEtapa.Nome}!",
                "/palpite",
                $"lembrete-{proximaEtapa.Id}"));

        // Marca a etapa como "lembrete já enviado" para não reenviar
        proximaEtapa.LembreteEnviado = true;
        await db.SaveChangesAsync();

        await audit.RegistrarAsync("LEMBRETE_ENVIADO", entidade: "Etapa", entidadeId: proximaEtapa.Id, detalhes: $"{todosUsuarios.Count} e-mails enviados para {proximaEtapa.Nome}");
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
                await emailService.EnviarAsync(
                    usuario.Email, $"🚨 F1Fast — Última chance! Prazo encerra em minutos ({proximaEtapa.Nome})",
                    GerarEmailLembreteUrgenteHtml(
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

        // Push urgente apenas para quem ainda não palpitou (mesmo público do e-mail)
        await push.EnviarParaUsuariosAsync(
            semPalpite.Select(u => u.Id),
            new PushNotificationService.PushPayload(
                "🚨 Última chance de palpitar!",
                $"O prazo do {proximaEtapa.Nome} encerra em minutos. Corre!",
                "/palpite",
                $"urgente-{proximaEtapa.Id}"));

        // Marca como enviado para não reenviar
        proximaEtapa.LembreteUrgenteEnviado = true;
        await db.SaveChangesAsync();

        await audit.RegistrarAsync("LEMBRETE_URGENTE", entidade: "Etapa", entidadeId: proximaEtapa.Id, detalhes: $"{semPalpite.Count} e-mails urgentes para {proximaEtapa.Nome}");
    }

    /// <summary>
    /// Reenvio MANUAL (acionado pelo admin) do lembrete geral de uma etapa específica.
    /// Diferente de EnviarLembretesAsync, ignora as travas de data (quarta-feira/09:00)
    /// e a flag LembreteEnviado — envia para TODOS os usuários na hora.
    /// Retorna a quantidade de e-mails enviados com sucesso.
    /// </summary>
    public async Task<int> ReenviarLembreteEtapaAsync(Etapa etapa)
    {
        var usuarios = await db.Usuarios.ToListAsync();
        var enviados = 0;

        foreach (var usuario in usuarios)
        {
            try
            {
                await emailService.EnviarAsync(
                    usuario.Email, $"⏱ F1Fast — Prazo do palpite termina em breve! ({etapa.Nome})",
                    GerarEmailLembreteHtml(
                        usuario.Nome,
                        etapa.Nome,
                        etapa.PrazoQualify.ToString("dd/MM/yyyy HH:mm")));
                enviados++;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao reenviar e-mail para {Email}", usuario.Email);
            }
        }

        // Marca como enviado (deixa o fluxo automático ciente de que já houve envio)
        etapa.LembreteEnviado = true;
        await db.SaveChangesAsync();

        return enviados;
    }

    /// <summary>
    // E-mail agora é enviado via EmailService (Azure Communication Services)

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
    <img src=""https://www.f1fast.com.br/logo.png"" alt=""F1Fast"" width=""48"" style=""display:inline-block;vertical-align:middle;margin-right:12px;"" />
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
      <a href=""https://www.f1fast.com.br"" target=""_blank"" style=""display:inline-block;padding:14px 32px;color:#FFFFFF;text-decoration:none;font-family:'Arial Black',Impact,sans-serif;font-size:14px;letter-spacing:2px;text-transform:uppercase;"">
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
    <img src=""https://www.f1fast.com.br/logo.png"" alt=""F1Fast"" width=""48"" style=""display:inline-block;vertical-align:middle;margin-right:12px;"" />
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
      <a href=""https://www.f1fast.com.br"" target=""_blank"" style=""display:inline-block;padding:14px 32px;color:#FFFFFF;text-decoration:none;font-family:'Arial Black',Impact,sans-serif;font-size:14px;letter-spacing:2px;text-transform:uppercase;"">
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

    // ============================================================
    // SEMANA DE F1! — segunda-feira de semana com corrida
    // ============================================================

    /// <summary>
    /// Envia e-mail + push "Semana de F1!" nas segundas-feiras de semana com corrida.
    /// Agrupa a Sprint e o GP do mesmo fim de semana (duas Etapas) para montar a
    /// agenda completa com os dois prazos. A flag SemanaEnviada evita reenvio.
    /// </summary>
    public async Task EnviarSemanaDeF1Async()
    {
        var agora = AgoraBRT;

        // Só nas segundas-feiras, a partir do meio-dia (BRT)
        if (agora.DayOfWeek != DayOfWeek.Monday || agora.Hour < 12)
            return;

        // Janela desta semana: segunda 00:00 → domingo 23:59:59
        var inicioSemana = agora.Date;
        var fimSemana    = inicioSemana.AddDays(7).AddSeconds(-1);

        // GP (corrida principal) desta semana ainda não avisado
        var gp = await db.Etapas
            .Where(e => !e.Sprint
                     && !e.Cancelada
                     && !e.Encerrada
                     && !e.SemanaEnviada
                     && e.DataCorrida != null
                     && e.DataCorrida >= inicioSemana
                     && e.DataCorrida <= fimSemana)
            .OrderBy(e => e.DataCorrida)
            .FirstOrDefaultAsync();

        if (gp is null) return; // nenhuma corrida nesta semana

        // Sprint pareada (se for fim de semana Sprint): mesma pista, número imediatamente anterior
        var sprint = await db.Etapas
            .FirstOrDefaultAsync(e => e.Sprint
                && !e.Cancelada
                && e.Circuito == gp.Circuito
                && e.Cidade == gp.Cidade
                && e.Numero == gp.Numero - 1);

        var todosUsuarios = await db.Usuarios.ToListAsync();
        const string assunto = "Semana de F1! 🏁";

        foreach (var usuario in todosUsuarios)
        {
            try
            {
                await emailService.EnviarAsync(usuario.Email, assunto, GerarEmailSemanaHtml(gp, sprint, usuario.Nome));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao enviar Semana de F1 para {Email}", usuario.Email);
            }
        }

        // Push para todos
        await push.EnviarParaUsuariosAsync(
            todosUsuarios.Select(u => u.Id),
            new PushNotificationService.PushPayload(
                "🏁 Semana de F1!",
                $"{gp.Nome} neste fim de semana — veja os horários e faça seu palpite.",
                $"/etapa/{gp.Id}",
                $"semana-{gp.Id}"));

        // Marca as etapas do fim de semana como avisadas
        gp.SemanaEnviada = true;
        if (sprint is not null) sprint.SemanaEnviada = true;
        await db.SaveChangesAsync();

        await audit.RegistrarAsync("SEMANA_ENVIADA", entidade: "Etapa", entidadeId: gp.Id,
            detalhes: $"{todosUsuarios.Count} avisos de Semana de F1 para {gp.Nome}");
    }

    // Abreviações fixas pt-BR (não depende da culture do servidor)
    private static readonly string[] DiasSemana = ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"];
    private static string FmtSessao(DateTime dt) => $"{DiasSemana[(int)dt.DayOfWeek]} {dt:dd/MM} · {dt:HH:mm}";

    // Converte código de país ISO ("cn") em emoji de bandeira (🇨🇳)
    private static string PaisEmoji(string cod)
    {
        if (string.IsNullOrWhiteSpace(cod) || cod.Length != 2) return "🏁";
        cod = cod.ToUpperInvariant();
        return char.ConvertFromUtf32(0x1F1E6 + (cod[0] - 'A')) + char.ConvertFromUtf32(0x1F1E6 + (cod[1] - 'A'));
    }

    private static string LinhaSessao(string nome, DateTime? dt, bool destaque)
    {
        if (dt is null) return "";
        var cor  = destaque ? "#17171B" : "#33333A";
        var peso = destaque ? "800" : "600";
        var dot  = destaque ? "#E10600" : "#CFCFD4";
        return $@"<tr>
      <td style=""padding:11px 0;border-bottom:1px solid #EFEFEF;font-size:14px;color:{cor};font-weight:{peso};"">
        <span style=""display:inline-block;width:7px;height:7px;border-radius:50%;background-color:{dot};margin-right:9px;"">&nbsp;</span>{nome}
      </td>
      <td style=""padding:11px 0;border-bottom:1px solid #EFEFEF;font-size:14px;color:#45454D;text-align:right;white-space:nowrap;"">{FmtSessao(dt.Value)}</td>
    </tr>";
    }

    private static string CardPrazo(string titulo, DateTime prazo) => $@"
    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0""><tr>
      <td style=""background-color:#FFF5F5;border:1px solid #F2D0D0;border-radius:8px;padding:12px 14px;"">
        <p style=""margin:0;font-size:11px;font-weight:700;letter-spacing:1px;text-transform:uppercase;color:#B0122A;"">{titulo}</p>
        <p style=""margin:4px 0 0;font-size:15px;font-weight:800;color:#17171B;"">{FmtSessao(prazo)}</p>
      </td>
    </tr></table>";

    private static string StatCell(string k, string v) => $@"
    <div style=""background-color:#F5F5F7;border-radius:8px;padding:10px 12px;"">
      <p style=""margin:0;font-size:10px;font-weight:700;letter-spacing:1px;text-transform:uppercase;color:#9A9AA2;"">{k}</p>
      <p style=""margin:2px 0 0;font-size:14px;font-weight:700;color:#2A2A30;"">{(string.IsNullOrWhiteSpace(v) ? "—" : v)}</p>
    </div>";

    /// <summary>
    /// Gera o HTML do e-mail "Semana de F1!" agrupando GP (+ Sprint, se houver).
    /// </summary>
    private string GerarEmailSemanaHtml(Etapa gp, Etapa? sprint, string nome)
    {
        var appUrl     = (config["AppUrl"] ?? "https://f1fast.com.br").TrimEnd('/');
        var linkEvento = $"{appUrl}/etapa/{gp.Id}";
        var temSprint  = sprint is not null;
        var bandeira   = PaisEmoji(gp.Pais);

        // Programação (só sessões com horário definido)
        var linhas = new System.Text.StringBuilder();
        if (temSprint)
        {
            linhas.Append(LinhaSessao("Treino Livre 1", sprint!.TreinoLivre1 ?? gp.TreinoLivre1, false));
            linhas.Append(LinhaSessao("Sprint Qualifying", sprint.Classificacao, false));
            linhas.Append(LinhaSessao("Sprint", sprint.DataCorrida, true));
            linhas.Append(LinhaSessao("Classificação", gp.Classificacao, false));
            linhas.Append(LinhaSessao("Corrida", gp.DataCorrida, true));
        }
        else
        {
            linhas.Append(LinhaSessao("Treino Livre 1", gp.TreinoLivre1, false));
            linhas.Append(LinhaSessao("Treino Livre 2", gp.TreinoLivre2, false));
            linhas.Append(LinhaSessao("Treino Livre 3", gp.TreinoLivre3, false));
            linhas.Append(LinhaSessao("Classificação", gp.Classificacao, false));
            linhas.Append(LinhaSessao("Corrida", gp.DataCorrida, true));
        }

        // Prazos: 2 cards (Sprint + Corrida) ou 1 (só Corrida)
        var prazos = temSprint
            ? $@"<table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0""><tr>
                  <td width=""50%"" valign=""top"" style=""padding-right:6px;"">{CardPrazo("Para a Sprint", sprint!.PrazoQualify)}</td>
                  <td width=""50%"" valign=""top"" style=""padding-left:6px;"">{CardPrazo("Para a Corrida", gp.PrazoQualify)}</td>
                </tr></table>"
            : CardPrazo("Prazo de palpite", gp.PrazoQualify);

        var badge = temSprint
            ? @"<span style=""background-color:#FFD84D;color:#8A5A00;font-size:11px;font-weight:700;letter-spacing:1px;text-transform:uppercase;padding:4px 10px;border-radius:4px;"">Fim de semana Sprint</span>"
            : "";

        var lead = temSprint
            ? "Tem GP chegando — e ainda é fim de semana de <strong>Sprint</strong>. Veja os horários e não perca os prazos de palpite."
            : "Tem corrida neste fim de semana. Confira os horários e não perca o prazo de palpite.";

        return $@"
<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""></head>
<body style=""margin:0;padding:0;background-color:#1A1A1A;font-family:Inter,Arial,sans-serif;"">
<table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#1A1A1A;padding:32px 16px;"">
<tr><td align=""center"">
<table role=""presentation"" width=""600"" cellpadding=""0"" cellspacing=""0"" style=""max-width:600px;width:100%;border-radius:8px;overflow:hidden;background-color:#FFFFFF;"">

  <tr><td style=""background-color:#E10600;height:4px;font-size:0;line-height:0;"">&nbsp;</td></tr>

  <tr><td style=""background-color:#16161A;padding:22px 32px;"">
    <span style=""font-family:'Arial Black',Impact,sans-serif;font-size:20px;color:#FFFFFF;letter-spacing:3px;"">F1<span style=""color:#E10600;"">FAST</span></span>
    <span style=""float:right;font-size:11px;font-weight:700;letter-spacing:2px;text-transform:uppercase;color:#E10600;padding-top:6px;"">Semana de F1</span>
  </td></tr>

  <tr><td style=""background-color:#FFFFFF;padding:34px 32px 12px;"">
    <p style=""margin:0 0 8px;font-size:11px;font-weight:700;letter-spacing:3px;text-transform:uppercase;color:#B0122A;"">É semana de corrida</p>
    <h1 style=""margin:0 0 10px;font-size:24px;color:#17171B;font-weight:800;line-height:1.15;"">Bora de F1 neste fim de semana! 🏁</h1>
    <p style=""margin:0 0 22px;font-size:15px;color:#45454D;line-height:1.6;"">Olá, {nome}! {lead}</p>

    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""margin-bottom:22px;"">
    <tr><td style=""background-color:#FAFAFA;border:1px solid #ECECEC;border-left:4px solid #E10600;border-radius:8px;padding:16px 18px;"">
      <span style=""font-size:22px;vertical-align:middle;"">{bandeira}</span>
      <span style=""font-size:18px;font-weight:800;color:#17171B;vertical-align:middle;margin-left:8px;"">{gp.Nome}</span>
      <span style=""float:right;"">{badge}</span>
      <p style=""margin:6px 0 0;font-size:13px;color:#6C6C74;"">{gp.Circuito} · {gp.Cidade} · Etapa {gp.Numero}</p>
    </td></tr>
    </table>

    <p style=""margin:0 0 10px;font-size:11px;font-weight:700;letter-spacing:2px;text-transform:uppercase;color:#9A9AA2;"">Programação <span style=""text-transform:none;letter-spacing:0;color:#C2C2C8;font-weight:400;"">· horário de Brasília</span></p>
    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"">{linhas}</table>

    <p style=""margin:24px 0 10px;font-size:11px;font-weight:700;letter-spacing:2px;text-transform:uppercase;color:#9A9AA2;"">Prazos de palpite</p>
    {prazos}

    <p style=""margin:24px 0 10px;font-size:11px;font-weight:700;letter-spacing:2px;text-transform:uppercase;color:#9A9AA2;"">O circuito</p>
    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"">
      <tr>
        <td width=""50%"" valign=""top"" style=""padding:0 4px 8px 0;"">{StatCell("Comprimento", gp.CircuitoComprimento)}</td>
        <td width=""50%"" valign=""top"" style=""padding:0 0 8px 4px;"">{StatCell("Voltas", gp.Voltas.ToString())}</td>
      </tr>
      <tr>
        <td width=""50%"" valign=""top"" style=""padding:0 4px 0 0;"">{StatCell("Distância", gp.Distancia)}</td>
        <td width=""50%"" valign=""top"" style=""padding:0 0 0 4px;"">{StatCell("Tipo", gp.CircuitoTipo)}</td>
      </tr>
    </table>

    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" style=""margin:26px auto 8px;"">
    <tr><td style=""background-color:#E10600;border-radius:6px;"">
      <a href=""{linkEvento}"" target=""_blank"" style=""display:inline-block;padding:15px 34px;color:#FFFFFF;text-decoration:none;font-family:'Arial Black',Impact,sans-serif;font-size:14px;letter-spacing:1px;text-transform:uppercase;"">
        Ver tudo e palpitar →
      </a>
    </td></tr>
    </table>
  </td></tr>

  <tr><td style=""background-color:#F3F3F3;padding:18px 32px;text-align:center;"">
    <p style=""margin:0;font-size:12px;color:#8B8B92;line-height:1.5;"">
      Você recebe este aviso toda segunda de semana com corrida.<br>© {AgoraBRT.Year} F1Fast — Campeonato Virtual CV2026
    </p>
  </td></tr>

</table>
</td></tr>
</table>
</body>
</html>";
    }
}
