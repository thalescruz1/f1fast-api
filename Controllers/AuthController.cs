using Microsoft.AspNetCore.Mvc;
using F1Fast.API.DTOs;
using F1Fast.API.Services;

namespace F1Fast.API.Controllers;

[ApiController, Route("api/auth")]
public class AuthController(AuthService auth) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var result = await auth.LoginAsync(req);
        return result is null ? Unauthorized("Login ou senha inválidos.") : Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var (ok, erro) = await auth.RegisterAsync(req);
        return ok ? Ok("Cadastro realizado com sucesso.") : BadRequest(erro);
    }
}
