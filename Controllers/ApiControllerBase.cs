// ============================================================
// BASE: ApiControllerBase
// ============================================================
// Classe base que fornece métodos auxiliares para retornar
// respostas de erro padronizadas no formato ApiError.
//
// Em vez de: return BadRequest("mensagem")      → corpo = string
// Usar:     return Erro400("mensagem")          → corpo = { status: 400, mensagem: "..." }
//
// Todos os controllers herdam desta classe para manter
// o formato de erro consistente em toda a API.
// ============================================================

using Microsoft.AspNetCore.Mvc;
using F1Fast.API.DTOs;

namespace F1Fast.API.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>400 Bad Request com corpo padronizado</summary>
    protected ObjectResult Erro400(string mensagem) =>
        StatusCode(400, new ApiError(400, mensagem));

    /// <summary>401 Unauthorized com corpo padronizado</summary>
    protected ObjectResult Erro401(string mensagem) =>
        StatusCode(401, new ApiError(401, mensagem));

    /// <summary>404 Not Found com corpo padronizado</summary>
    protected ObjectResult Erro404(string mensagem) =>
        StatusCode(404, new ApiError(404, mensagem));

    /// <summary>409 Conflict com corpo padronizado</summary>
    protected ObjectResult Erro409(string mensagem) =>
        StatusCode(409, new ApiError(409, mensagem));
}
