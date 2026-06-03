using Azure;
using Azure.Communication.Email;

namespace F1Fast.API.Services;

/// <summary>
/// Service centralizado para envio de e-mails via Azure Communication Services (ACS).
/// Substitui o SendGrid (trial expirado). A assinatura de EnviarAsync é mantida,
/// então NotificacaoService e os templates HTML continuam intactos.
/// </summary>
public class EmailService(IConfiguration config, ILogger<EmailService> logger)
{
    /// <summary>
    /// Envia um e-mail HTML via Azure Communication Services Email.
    /// </summary>
    /// <param name="para">E-mail do destinatário</param>
    /// <param name="assunto">Assunto do e-mail</param>
    /// <param name="corpoHtml">Conteúdo HTML do e-mail</param>
    public async Task EnviarAsync(string para, string assunto, string corpoHtml)
    {
        var connectionString = config["Azure:Communication:ConnectionString"];
        // Remetente verificado no domínio do ACS (ex: cv@f1fast.com.br)
        var remetente        = config["Azure:Communication:From"] ?? config["Smtp:From"] ?? "cv@f1fast.com.br";

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Azure:Communication:ConnectionString não configurada.");

        var client = new EmailClient(connectionString);

        var mensagem = new EmailMessage(
            senderAddress: remetente,
            content: new EmailContent(assunto) { Html = corpoHtml },
            recipients: new EmailRecipients([new EmailAddress(para)]));

        // WaitUntil.Completed = aguarda a operação concluir para conhecermos o status final
        EmailSendOperation operacao = await client.SendAsync(WaitUntil.Completed, mensagem);

        if (operacao.Value.Status != EmailSendStatus.Succeeded)
        {
            logger.LogError("Falha ACS Email para {Para}: status {Status}", para, operacao.Value.Status);
            throw new Exception($"Falha ao enviar e-mail via Azure: {operacao.Value.Status}");
        }
    }
}
