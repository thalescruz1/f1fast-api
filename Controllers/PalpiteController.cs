using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using F1Fast.API.Data;
using F1Fast.API.DTOs;
using F1Fast.API.Models;

namespace F1Fast.API.Controllers;

[ApiController, Route("api/palpites"), Authorize]
public class PalpiteController(AppDbContext db) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Enviar(PalpiteRequest req)
    {
        var etapa = await db.Etapas.FindAsync(req.EtapaId);
        if (etapa is null) return NotFound("Etapa não encontrada.");
        if (DateTime.UtcNow > etapa.PrazoQualify)
            return BadRequest("Prazo encerrado para esta etapa.");

        int[] posicoes = [
            req.Pos1Id, req.Pos2Id, req.Pos3Id, req.Pos4Id, req.Pos5Id,
            req.Pos6Id, req.Pos7Id, req.Pos8Id, req.Pos9Id, req.Pos10Id
        ];

        if (posicoes.Distinct().Count() != 10)
            return BadRequest("O mesmo piloto não pode aparecer em mais de uma posição.");

        var existente = await db.Palpites
            .FirstOrDefaultAsync(p => p.UsuarioId == UserId && p.EtapaId == req.EtapaId);

        if (existente is not null)
        {
            existente.PoleId        = req.PoleId;
            existente.Pos1Id        = req.Pos1Id;
            existente.Pos2Id        = req.Pos2Id;
            existente.Pos3Id        = req.Pos3Id;
            existente.Pos4Id        = req.Pos4Id;
            existente.Pos5Id        = req.Pos5Id;
            existente.Pos6Id        = req.Pos6Id;
            existente.Pos7Id        = req.Pos7Id;
            existente.Pos8Id        = req.Pos8Id;
            existente.Pos9Id        = req.Pos9Id;
            existente.Pos10Id       = req.Pos10Id;
            existente.MelhorVoltaId = req.MelhorVoltaId;
            existente.EnviadoEm     = DateTime.UtcNow;
            existente.PontosObtidos = null;
        }
        else
        {
            db.Palpites.Add(new Palpite
            {
                UsuarioId     = UserId,
                EtapaId       = req.EtapaId,
                PoleId        = req.PoleId,
                Pos1Id        = req.Pos1Id,
                Pos2Id        = req.Pos2Id,
                Pos3Id        = req.Pos3Id,
                Pos4Id        = req.Pos4Id,
                Pos5Id        = req.Pos5Id,
                Pos6Id        = req.Pos6Id,
                Pos7Id        = req.Pos7Id,
                Pos8Id        = req.Pos8Id,
                Pos9Id        = req.Pos9Id,
                Pos10Id       = req.Pos10Id,
                MelhorVoltaId = req.MelhorVoltaId
            });
        }

        await db.SaveChangesAsync();
        return Ok("Palpite enviado com sucesso.");
    }

    [HttpGet("{etapaId:int}/meu")]
    public async Task<IActionResult> GetMeu(int etapaId)
    {
        var palpite = await db.Palpites
            .FirstOrDefaultAsync(p => p.UsuarioId == UserId && p.EtapaId == etapaId);

        return palpite is null ? NotFound() : Ok(palpite);
    }

    [HttpGet("{etapaId:int}/publico"), AllowAnonymous]
    public async Task<IActionResult> GetPublico(int etapaId)
    {
        var etapa = await db.Etapas.FindAsync(etapaId);
        if (etapa is null) return NotFound();
        if (DateTime.UtcNow < etapa.PrazoQualify)
            return BadRequest("Os palpites ficam visíveis após o prazo.");

        var pilotos = await db.Pilotos
            .ToDictionaryAsync(p => p.Id, p => $"{p.Numero} — {p.Nome}");

        var palpites = await db.Palpites
            .Include(p => p.Usuario)
            .Where(p => p.EtapaId == etapaId)
            .OrderByDescending(p => p.PontosObtidos)
            .Select(p => new PalpitePublicoDto(
                p.Usuario.Login,
                p.Usuario.Nome,
                new[]
                {
                    pilotos.GetValueOrDefault(p.PoleId,        "?"),
                    pilotos.GetValueOrDefault(p.Pos1Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos2Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos3Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos4Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos5Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos6Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos7Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos8Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos9Id,        "?"),
                    pilotos.GetValueOrDefault(p.Pos10Id,       "?"),
                    pilotos.GetValueOrDefault(p.MelhorVoltaId, "?")
                },
                p.PontosObtidos
            ))
            .ToListAsync();

        return Ok(palpites);
    }
}
