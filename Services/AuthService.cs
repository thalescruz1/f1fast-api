// ============================================================
// SERVICE: AuthService
// ============================================================
// Um "Service" contém a LÓGICA DE NEGÓCIO da aplicação.
// O Controller recebe a requisição e delega ao Service.
// O Service faz o trabalho pesado: valida, processa e persiste.
//
// Responsabilidades deste service:
//   - Autenticar usuários (verificar senha e gerar JWT)
//   - Registrar novos usuários (com hash de senha)
//   - Gerenciar recuperação de senha por e-mail
// ============================================================

using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using F1Fast.API.Data;
using F1Fast.API.DTOs;
using F1Fast.API.Models;
using static F1Fast.API.Helpers.DateTimeHelper;

namespace F1Fast.API.Services;

// IConfiguration config = acesso ao appsettings.json (chaves JWT, configurações SMTP)
// ILogger<AuthService> logger = sistema de logs do .NET para registrar erros
public class AuthService(AppDbContext db, IConfiguration config, ILogger<AuthService> logger)
{
    /// <summary>
    /// Verifica as credenciais e retorna um token JWT se válidas.
    /// Retorna null se o login ou senha estiverem errados.
    /// </summary>
    public async Task<AuthResponse?> LoginAsync(LoginRequest req)
    {
        // Busca o usuário pelo login OU pelo e-mail (permite login com ambos)
        var identificador = req.Login.Trim();
        var user = identificador.Contains('@')
            ? await db.Usuarios.FirstOrDefaultAsync(u => u.Email == identificador)
            : await db.Usuarios.FirstOrDefaultAsync(u => u.Login == identificador);

        // BCrypt.Verify compara a senha digitada com o hash salvo no banco.
        // Nunca comparamos senhas em texto puro — sempre usamos o hash!
        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Senha, user.SenhaHash))
            return null; // retorna null = credenciais inválidas

        // Gera e retorna o token JWT junto com os dados básicos do usuário
        return new AuthResponse(GerarToken(user), user.Login, user.Nome, user.Role);
    }

    /// <summary>
    /// Cria um novo usuário após validar que login, e-mail e CPF são únicos.
    /// Retorna (true, "") em caso de sucesso ou (false, "mensagem") se inválido.
    /// </summary>
    public async Task<(bool Ok, string Erro)> RegisterAsync(RegisterRequest req)
    {
        // Login não pode conter espaços
        if (req.Login.Contains(' '))
            return (false, "O login não pode conter espaços.");

        // Validação de complexidade da senha
        var erroSenha = ValidarSenha(req.Senha);
        if (erroSenha is not null)
            return (false, erroSenha);

        // AnyAsync = retorna true se existir PELO MENOS UM registro com essa condição
        if (await db.Usuarios.AnyAsync(u => u.Login == req.Login))
            return (false, "Login já em uso.");
        if (await db.Usuarios.AnyAsync(u => u.Email == req.Email))
            return (false, "E-mail já cadastrado.");
        if (await db.Usuarios.AnyAsync(u => u.Cpf == req.Cpf))
            return (false, "CPF já cadastrado.");

        db.Usuarios.Add(new Usuario
        {
            Login       = req.Login,
            Nome        = req.Nome,
            Sobrenome   = req.Sobrenome,
            Cpf         = req.Cpf,
            // HashPassword criptografa a senha antes de salvar no banco
            SenhaHash   = BCrypt.Net.BCrypt.HashPassword(req.Senha),
            Localizacao = req.Localizacao,
            Email       = req.Email
        });

        await db.SaveChangesAsync(); // persiste no banco
        return (true, "");
    }

    /// <summary>
    /// Gera um token de reset e envia por e-mail para o usuário recuperar a senha.
    /// Não revela se o e-mail existe no sistema (segurança contra enumeração).
    /// </summary>
    public async Task SolicitarResetAsync(string email)
    {
        var user = await db.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) return; // sai silenciosamente para não revelar se o e-mail existe

        // Guid.NewGuid() gera um ID único aleatório (ex: "a3f9c2d1e8b4...")
        // ".ToString("N")" = formato sem traços (32 caracteres hex)
        var token = Guid.NewGuid().ToString("N");
        user.ResetToken        = token;
        user.ResetTokenExpiry  = AgoraBRT.AddHours(1); // válido por 1 hora
        await db.SaveChangesAsync();

        // Monta o link de redefinição que será enviado por e-mail
        var appUrl = config["AppUrl"] ?? "https://f1fast.com.br";
        var link   = $"{appUrl}/redefinir-senha?token={token}";

        // try/catch: se o e-mail falhar, não derruba a aplicação inteira
        try
        {
            await EnviarEmailAsync(
                para:    user.Email,
                assunto: "F1Fast — Redefinição de senha",
                corpo:   GerarEmailResetHtml(user.Nome, link)
            );
        }
        catch (Exception ex)
        {
            // Loga o erro mas não propaga — o usuário não precisa saber se o e-mail falhou
            logger.LogError(ex, "Erro ao enviar e-mail de reset para {Email}", email);
        }
    }

    /// <summary>
    /// Redefine a senha usando o token recebido por e-mail.
    /// Valida que o token é correto E que não expirou.
    /// </summary>
    public async Task<(bool Ok, string Erro)> RedefinirSenhaAsync(string token, string novaSenha)
    {
        // Busca o usuário pelo token, garantindo que não expirou
        // Validação de complexidade da senha
        var erroSenha = ValidarSenha(novaSenha);
        if (erroSenha is not null)
            return (false, erroSenha);

        var user = await db.Usuarios.FirstOrDefaultAsync(u =>
            u.ResetToken == token && u.ResetTokenExpiry > AgoraBRT);

        if (user is null)
            return (false, "Token inválido ou expirado.");

        // Atualiza a senha com novo hash e limpa o token de reset
        user.SenhaHash        = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        user.ResetToken       = null; // invalida o token para não poder ser reutilizado
        user.ResetTokenExpiry = null;
        await db.SaveChangesAsync();

        return (true, "");
    }

    /// <summary>
    /// Valida complexidade da senha: mín. 8 chars, maiúscula, número e caractere especial.
    /// Retorna null se válida, ou a mensagem de erro.
    /// </summary>
    private static string? ValidarSenha(string senha)
    {
        if (senha.Length < 8)
            return "A senha deve ter no mínimo 8 caracteres.";
        if (!Regex.IsMatch(senha, @"[A-Z]"))
            return "A senha deve conter pelo menos uma letra maiúscula.";
        if (!Regex.IsMatch(senha, @"[0-9]"))
            return "A senha deve conter pelo menos um número.";
        if (!Regex.IsMatch(senha, @"[!@#$%&*?.\-_]"))
            return "A senha deve conter pelo menos um caractere especial (!@#$%&*?.-_).";
        return null;
    }

    /// <summary>
    /// Gera um JSON Web Token (JWT) assinado digitalmente para o usuário.
    /// O token carrega informações (claims) que identificam o usuário na API.
    /// </summary>
    private string GerarToken(Usuario user)
    {
        // Chave secreta lida do appsettings.json (Jwt:Key)
        var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));

        // Algoritmo de assinatura: HmacSha256 (padrão para JWT)
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // "Claims" = informações embutidas no token que a API pode ler sem consultar o banco
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ID do usuário
            new Claim(ClaimTypes.Name,           user.Login),         // Login
            new Claim(ClaimTypes.Role,           user.Role)           // "User" ou "Admin"
        };

        // Monta o token JWT com validade de 30 dias
        var token = new JwtSecurityToken(
            issuer:             config["Jwt:Issuer"],   // quem emitiu o token
            audience:           config["Jwt:Audience"], // para quem é o token
            claims:             claims,
            expires:            AgoraBRT.AddDays(30),
            signingCredentials: creds
        );

        // Serializa o token para string (formato: xxxxx.yyyyy.zzzzz)
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Envia um e-mail HTML via SMTP usando as configurações do appsettings.json.
    /// </summary>
    private async Task EnviarEmailAsync(string para, string assunto, string corpo)
    {
        // "using" garante que o SmtpClient seja descartado (disposed) ao terminar
        using var client = new SmtpClient(
            config["Smtp:Host"], int.Parse(config["Smtp:Port"]!));

        client.Credentials = new System.Net.NetworkCredential(config["Smtp:User"], config["Smtp:Pass"]);
        client.EnableSsl   = true; // conexão segura (TLS)

        var message = new MailMessage(config["Smtp:From"]!, para, assunto, corpo)
        {
            IsBodyHtml = true // envia como HTML formatado
        };

        await client.SendMailAsync(message);
    }

    /// <summary>
    /// Gera o HTML do e-mail de redefinição de senha com layout F1Fast.
    /// </summary>
    private static string GerarEmailResetHtml(string nome, string link) => $@"
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
    <p style=""margin:0 0 8px;font-size:14px;color:#9E9E9E;text-transform:uppercase;letter-spacing:2px;"">🔒 Redefinição de senha</p>
    <h1 style=""margin:0 0 24px;font-size:22px;color:#1A1A1A;font-weight:700;"">Olá, {nome}!</h1>
    <p style=""margin:0 0 16px;font-size:15px;color:#333333;line-height:1.6;"">
      Recebemos uma solicitação para redefinir sua senha. Clique no botão abaixo para criar uma nova senha:
    </p>

    <!-- Botão CTA -->
    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" style=""margin:32px auto;"">
    <tr><td style=""background-color:#0057E1;border-radius:6px;"">
      <a href=""{link}"" target=""_blank"" style=""display:inline-block;padding:14px 32px;color:#FFFFFF;text-decoration:none;font-family:'Arial Black',Impact,sans-serif;font-size:14px;letter-spacing:2px;text-transform:uppercase;"">
        REDEFINIR MINHA SENHA
      </a>
    </td></tr>
    </table>

    <p style=""margin:0 0 8px;font-size:13px;color:#E10600;text-align:center;font-weight:600;"">⏱ Este link expira em 1 hora</p>

    <!-- Divisor -->
    <hr style=""border:none;border-top:1px solid #EBEBEB;margin:24px 0;"" />

    <p style=""margin:0;font-size:13px;color:#9E9E9E;line-height:1.5;"">
      Se você não solicitou a redefinição de senha, ignore este e-mail. Sua senha permanecerá a mesma.
    </p>
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
