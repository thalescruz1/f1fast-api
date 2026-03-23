using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;

namespace F1Fast.API.Controllers.Admin;

/// <summary>
/// Endpoint admin para consultar os logs de auditoria do sistema.
/// </summary>
[ApiController, Route("api/admin/logs"), Authorize(Roles = "Admin")]
public class LogController(AppDbContext db) : ControllerBase
{
    /// <summary>
    /// GET /api/admin/logs?acao=LOGIN_OK&amp;login=thales&amp;limite=200
    /// Retorna os logs mais recentes com filtros opcionais.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetLogs(
        [FromQuery] string? acao = null,
        [FromQuery] string? login = null,
        [FromQuery] int limite = 200)
    {
        var query = db.LogsAuditoria.AsQueryable();

        if (!string.IsNullOrEmpty(acao))
            query = query.Where(l => l.Acao == acao);

        if (!string.IsNullOrEmpty(login))
            query = query.Where(l => l.UsuarioLogin != null && l.UsuarioLogin.Contains(login));

        var logs = await query
            .OrderByDescending(l => l.DataHora)
            .Take(limite)
            .Select(l => new
            {
                l.Id,
                l.DataHora,
                l.Acao,
                l.UsuarioId,
                l.UsuarioLogin,
                l.Entidade,
                l.EntidadeId,
                l.Detalhes,
                l.Sucesso,
                l.Ip
            })
            .ToListAsync();

        return Ok(logs);
    }
}
