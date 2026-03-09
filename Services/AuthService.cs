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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using F1Fast.API.Data;
using F1Fast.API.DTOs;
using F1Fast.API.Models;

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
        // Busca o usuário pelo login no banco
        var user = await db.Usuarios.FirstOrDefaultAsync(u => u.Login == req.Login);

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
        user.ResetTokenExpiry  = DateTime.UtcNow.AddHours(1); // válido por 1 hora
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
                corpo:   $"Olá {user.Nome},\n\n" +
                         $"Recebemos uma solicitação para redefinir sua senha.\n\n" +
                         $"Clique no link abaixo para criar uma nova senha (válido por 1 hora):\n\n" +
                         $"{link}\n\n" +
                         $"Se você não solicitou isso, ignore este e-mail.\n\n" +
                         $"— Equipe F1Fast"
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
        var user = await db.Usuarios.FirstOrDefaultAsync(u =>
            u.ResetToken == token && u.ResetTokenExpiry > DateTime.UtcNow);

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
            expires:            DateTime.UtcNow.AddDays(30),
            signingCredentials: creds
        );

        // Serializa o token para string (formato: xxxxx.yyyyy.zzzzz)
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Envia um e-mail via SMTP usando as configurações do appsettings.json.
    /// </summary>
    private async Task EnviarEmailAsync(string para, string assunto, string corpo)
    {
        // "using" garante que o SmtpClient seja descartado (disposed) ao terminar
        using var client = new SmtpClient(
            config["Smtp:Host"], int.Parse(config["Smtp:Port"]!));

        client.Credentials = new System.Net.NetworkCredential(config["Smtp:User"], config["Smtp:Pass"]);
        client.EnableSsl   = true; // usa TLS/SSL para envio seguro

        await client.SendMailAsync(
            new MailMessage(config["Smtp:From"]!, para, assunto, corpo));
    }
}
