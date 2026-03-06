using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using F1Fast.API.DTOs;

namespace F1Fast.API.Controllers;

[ApiController, Route("api/etapas")]
public class EtapaController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var agora  = DateTime.UtcNow;
        var etapas = await db.Etapas
            .OrderBy(e => e.Numero)
            .Select(e => new EtapaDto(
                e.Id, e.Numero, e.Nome, e.Circuito, e.Cidade, e.Pais,
                e.Sprint, e.PrazoQualify, e.DataCorrida,
                e.Encerrada, agora > e.PrazoQualify))
            .ToListAsync();

        return Ok(etapas);
    }

    [HttpGet("proxima")]
    public async Task<IActionResult> GetProxima()
    {
        var agora   = DateTime.UtcNow;
        var proxima = await db.Etapas
            .Where(e => !e.Encerrada && e.PrazoQualify > agora)
            .OrderBy(e => e.PrazoQualify)
            .Select(e => new EtapaDto(
                e.Id, e.Numero, e.Nome, e.Circuito, e.Cidade, e.Pais,
                e.Sprint, e.PrazoQualify, e.DataCorrida,
                e.Encerrada, false))
            .FirstOrDefaultAsync();

        return proxima is null ? NotFound() : Ok(proxima);
    }
}
