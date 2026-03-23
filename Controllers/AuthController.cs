// ============================================================
// CONTROLLER: AuthController
// ============================================================
// Em .NET, um "Controller" recebe as requisições HTTP do frontend
// e decide o que fazer. Pense nele como a "recepção" da API:
// recebe os pedidos, chama o serviço correto e devolve a resposta.
//
// [ApiController]       = indica que é uma controller de API (ativa validações automáticas)
// [Route("api/auth")]   = define o prefixo da URL: todas as rotas aqui começam com /api/auth
// ControllerBase        = classe base que fornece métodos como Ok(), BadRequest(), Unauthorized()
//
// Esta controller cuida do acesso ao sistema:
//   POST /api/auth/login          → fazer login
//   POST /api/auth/register       → criar conta
//   POST /api/auth/esqueci-senha  → pedir recuperação de senha
//   POST /api/auth/redefinir-senha → definir nova senha
// ============================================================

using Microsoft.AspNetCore.Mvc;
using F1Fast.API.DTOs;
using F1Fast.API.Services;

namespace F1Fast.API.Controllers;

// "(AuthService auth)" = injeção de dependência (DI).
// O .NET cria e injeta o AuthService automaticamente.
// Não precisamos escrever "new AuthService()" — o framework gerencia o ciclo de vida.
[ApiController, Route("api/auth")]
public class AuthController(AuthService auth, AuditoriaService audit) : ApiControllerBase
{
    // [HttpPost("login")] = responde a requisições POST na URL /api/auth/login
    // LoginRequest req = o .NET deserializa o JSON do body automaticamente
    // Task<IActionResult> = método assíncrono (async/await) que retorna uma resposta HTTP
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        // Delega a lógica de login para o AuthService
        var result = await auth.LoginAsync(req);

        if (result is null)
        {
            await audit.RegistrarAsync("LOGIN_FALHA", detalhes: $"Login tentado: {req.Login}", sucesso: false, ip: AuditoriaService.ExtrairIp(HttpContext));
            return Erro401("Login ou senha inválidos.");
        }

        await audit.RegistrarAsync("LOGIN_OK", usuarioLogin: result.Login, detalhes: "Login bem-sucedido", ip: AuditoriaService.ExtrairIp(HttpContext));
        return Ok(result);
    }

    // POST /api/auth/register → criar uma nova conta
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        // "var (ok, erro)" = destructuring de tupla. RegisterAsync retorna dois valores.
        var (ok, erro) = await auth.RegisterAsync(req);

        // Se ok=true → 200 OK com mensagem de sucesso
        // Se ok=false → 400 BadRequest com a mensagem de erro (ex: "Login já em uso")
        if (!ok) return Erro400(erro);

        await audit.RegistrarAsync("CADASTRO", entidade: "Usuario", detalhes: $"Novo usuário: {req.Login} ({req.Email})", ip: AuditoriaService.ExtrairIp(HttpContext));
        return Ok("Cadastro realizado com sucesso.");
    }

    // POST /api/auth/esqueci-senha → envia e-mail de recuperação de senha
    [HttpPost("esqueci-senha")]
    public async Task<IActionResult> EsqueciSenha(EsqueciSenhaRequest req)
    {
        await auth.SolicitarResetAsync(req.Email);

        await audit.RegistrarAsync("RESET_SENHA_SOLICITADO", detalhes: $"E-mail: {req.Email}", ip: AuditoriaService.ExtrairIp(HttpContext));

        // SEGURANÇA: sempre retorna 200 (mesmo se o e-mail não existir)
        // Isso evita que alguém descubra quais e-mails estão cadastrados no sistema.
        return Ok("Se o e-mail existir, você receberá o link em instantes.");
    }

    // POST /api/auth/redefinir-senha → define a nova senha usando o token do e-mail
    [HttpPost("redefinir-senha")]
    public async Task<IActionResult> RedefinirSenha(RedefinirSenhaRequest req)
    {
        var (ok, erro) = await auth.RedefinirSenhaAsync(req.Token, req.NovaSenha);

        if (!ok) return Erro400(erro);

        await audit.RegistrarAsync("RESET_SENHA_OK", detalhes: $"Token: {req.Token[..8]}...", ip: AuditoriaService.ExtrairIp(HttpContext));
        return Ok("Senha redefinida com sucesso.");
    }
}
