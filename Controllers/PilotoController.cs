// ============================================================
// CONTROLLER: PilotoController
// ============================================================
// Expõe a lista de pilotos da temporada F1 2026.
// Rota PÚBLICA — usada pelo formulário de palpites para popular
// os dropdowns de seleção de pilotos.
//
//   GET /api/pilotos → retorna todos os 22 pilotos ativos com sua equipe
// ============================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using F1Fast.API.DTOs;

namespace F1Fast.API.Controllers;

[ApiController, Route("api/pilotos")]
public class PilotoController(AppDbContext db) : ApiControllerBase
{
    // GET /api/pilotos → lista todos os pilotos ativos com dados da equipe
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pilotos = await db.Pilotos
            .Include(p => p.Equipe)    // JOIN com a tabela Equipes (para pegar nome e cor)
            .Where(p => p.Ativo)       // filtra apenas pilotos ativos na temporada
            .OrderBy(p => p.Numero)    // ordena pelo número de corrida (1, 3, 5, 6...)
            .Select(p => new PilotoDto(p.Id, p.Numero, p.Nome, p.Equipe.Nome, p.Equipe.Cor))
            .ToListAsync();

        return Ok(pilotos);
    }
}
