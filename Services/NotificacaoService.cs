// ============================================================
// SERVICE: NotificacaoService
// ============================================================
// Responsável por enviar e-mails de lembrete para participantes
// que ainda não fizeram palpite quando o prazo está próximo.
//
// Este service é chamado pelo LembreteBackgroundService a cada
// 30 minutos. Se houver uma etapa com prazo nas próximas 3 horas,
// todos os usuários SEM palpite recebem um e-mail de aviso.
// ============================================================

using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;

namespace F1Fast.API.Services;

public class NotificacaoService(AppDbContext db, IConfiguration config, ILogger<NotificacaoService> logger)
{
    /// <summary>
    /// Verifica se alguma etapa tem prazo nas próximas 3 horas e
    /// envia e-mails de lembrete para participantes sem palpite.
    /// </summary>
    public async Task EnviarLembretesAsync()
    {
        var agora  = DateTime.UtcNow;
        var limite = agora.AddHours(3); // janela de alerta: próximas 3 horas

        // Busca etapa com prazo entre "agora" e "agora + 3 horas"
        var proximaEtapa = await db.Etapas
            .Where(e => !e.Encerrada && e.PrazoQualify > agora && e.PrazoQualify <= limite)
            .OrderBy(e => e.PrazoQualify)
            .FirstOrDefaultAsync();

        if (proximaEtapa is null) return; // nenhuma etapa urgente — não faz nada

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
                    corpo:   $"Olá {usuario.Nome},\n\n" +
                             $"O prazo para enviar seu palpite para o {proximaEtapa.Nome} " +
                             $"termina em {proximaEtapa.PrazoQualify:dd/MM/yyyy HH:mm} UTC.\n\n" +
                             $"Acesse f1fast.com.br e faça seu palpite agora!\n\n" +
                             $"Boa sorte!\n— Equipe F1Fast"
                );
            }
            catch (Exception ex)
            {
                // Se falhar para um usuário, loga o erro e continua para o próximo
                logger.LogError(ex, "Erro ao enviar e-mail para {Email}", usuario.Email);
            }
        }
    }

    /// <summary>
    /// Método auxiliar privado para envio de e-mail via SMTP.
    /// "private" = só pode ser chamado dentro desta classe.
    /// </summary>
    private async Task EnviarEmailAsync(string para, string assunto, string corpo)
    {
        // "using" = garante que o SmtpClient seja descartado após o uso (evita vazamento de recursos)
        using var client = new System.Net.Mail.SmtpClient(
            config["Smtp:Host"], int.Parse(config["Smtp:Port"]!));

        client.Credentials = new System.Net.NetworkCredential(config["Smtp:User"], config["Smtp:Pass"]);
        client.EnableSsl   = true; // conexão segura (TLS)

        await client.SendMailAsync(
            new System.Net.Mail.MailMessage(config["Smtp:From"]!, para, assunto, corpo));
    }
}
