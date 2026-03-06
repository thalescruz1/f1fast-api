using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;

namespace F1Fast.API.Services;

public class NotificacaoService(AppDbContext db, IConfiguration config, ILogger<NotificacaoService> logger)
{
    public async Task EnviarLembretesAsync()
    {
        var agora  = DateTime.UtcNow;
        var limite = agora.AddHours(3);

        var proximaEtapa = await db.Etapas
            .Where(e => !e.Encerrada && e.PrazoQualify > agora && e.PrazoQualify <= limite)
            .OrderBy(e => e.PrazoQualify)
            .FirstOrDefaultAsync();

        if (proximaEtapa is null) return;

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
                logger.LogError(ex, "Erro ao enviar e-mail para {Email}", usuario.Email);
            }
        }
    }

    private async Task EnviarEmailAsync(string para, string assunto, string corpo)
    {
        using var client = new System.Net.Mail.SmtpClient(
            config["Smtp:Host"], int.Parse(config["Smtp:Port"]!));

        client.Credentials = new System.Net.NetworkCredential(config["Smtp:User"], config["Smtp:Pass"]);
        client.EnableSsl   = true;

        await client.SendMailAsync(
            new System.Net.Mail.MailMessage(config["Smtp:From"]!, para, assunto, corpo));
    }
}
