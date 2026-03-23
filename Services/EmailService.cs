using SendGrid;
using SendGrid.Helpers.Mail;

namespace F1Fast.API.Services;

/// <summary>
/// Service centralizado para envio de e-mails via SendGrid.
/// Substitui o SmtpClient que dava problemas de certificado.
/// </summary>
public class EmailService(IConfiguration config, ILogger<EmailService> logger)
{
    /// <summary>
    /// Envia um e-mail HTML via SendGrid API.
    /// </summary>
    /// <param name="para">E-mail do destinatário</param>
    /// <param name="assunto">Assunto do e-mail</param>
    /// <param name="corpoHtml">Conteúdo HTML do e-mail</param>
    public async Task EnviarAsync(string para, string assunto, string corpoHtml)
    {
        var apiKey = config["SendGrid:ApiKey"];
        var from   = config["SendGrid:From"] ?? config["Smtp:From"] ?? "contato@f1fast.com.br";
        var nome   = config["SendGrid:FromName"] ?? "F1Fast";

        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException("SendGrid:ApiKey não configurada no appsettings.");

        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage
        {
            From             = new EmailAddress(from, nome),
            Subject          = assunto,
            HtmlContent      = corpoHtml,
            PlainTextContent = ""  // fallback vazio — e-mail é HTML
        };
        msg.AddTo(new EmailAddress(para));

        // Desabilita tracking de cliques e aberturas (opcional, mantém links limpos)
        msg.SetClickTracking(false, false);

        var response = await client.SendEmailAsync(msg);

        if ((int)response.StatusCode >= 400)
        {
            var body = await response.Body.ReadAsStringAsync();
            logger.LogError("Erro SendGrid ({StatusCode}): {Body}", response.StatusCode, body);
            throw new Exception($"Falha ao enviar e-mail via SendGrid: {response.StatusCode}");
        }
    }
}
