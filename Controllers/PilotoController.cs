using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using F1Fast.API.DTOs;

namespace F1Fast.API.Controllers;

[ApiController, Route("api/pilotos")]
public class PilotoController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pilotos = await db.Pilotos
            .Include(p => p.Equipe)
            .Where(p => p.Ativo)
            .OrderBy(p => p.Numero)
            .Select(p => new PilotoDto(p.Id, p.Numero, p.Nome, p.Equipe.Nome, p.Equipe.Cor))
            .ToListAsync();

        return Ok(pilotos);
    }
}
