// ============================================================
// CONTROLLER: PilotoAdminController (Admin)
// ============================================================
// CRUD de pilotos para o painel admin. Usado para lidar com
// substituições no meio da temporada (ex: piloto lesionado):
// adicionar um substituto, trocar a equipe de um piloto para
// um fim de semana, e ativar/desativar.
//
//   GET    /api/admin/pilotos          → lista TODOS (inclusive inativos)
//   GET    /api/admin/pilotos/equipes  → equipes (para o dropdown)
//   POST   /api/admin/pilotos          → adiciona um piloto
//   PATCH  /api/admin/pilotos/{id}     → atualiza (equipe, status, nome, número)
//
// Obs: a lista pública (GET /api/pilotos, usada nos formulários de
// palpite e resultado) já filtra apenas Ativo == true, então ativar/
// desativar aqui reflete direto na seleção de pilotos.
// ============================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using F1Fast.API.Data;
using F1Fast.API.DTOs;
using F1Fast.API.Models;
using F1Fast.API.Services;

namespace F1Fast.API.Controllers.Admin;

[ApiController, Route("api/admin/pilotos"), Authorize(Roles = "Admin")]
public class PilotoAdminController(AppDbContext db, AuditoriaService audit) : ApiControllerBase
{
    private int?    Uid   => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
    private string? Login => User.FindFirstValue(ClaimTypes.Name);

    // GET /api/admin/pilotos → todos os pilotos (ativos primeiro), com equipe e status
    [HttpGet]
    public async Task<IActionResult> GetTodos()
    {
        var pilotos = await db.Pilotos
            .Include(p => p.Equipe)
            .OrderByDescending(p => p.Ativo)
            .ThenBy(p => p.Numero)
            .Select(p => new PilotoAdminDto(p.Id, p.Numero, p.Nome, p.EquipeId, p.Equipe.Nome, p.Equipe.Cor, p.Ativo))
            .ToListAsync();

        return Ok(pilotos);
    }

    // GET /api/admin/pilotos/equipes → lista de equipes para o dropdown
    [HttpGet("equipes")]
    public async Task<IActionResult> GetEquipes() =>
        Ok(await db.Equipes.OrderBy(e => e.Nome)
            .Select(e => new EquipeDto(e.Id, e.Nome, e.Cor))
            .ToListAsync());

    // POST /api/admin/pilotos → adiciona um piloto (ex: substituto)
    [HttpPost]
    public async Task<IActionResult> Adicionar(PilotoCreateRequest req)
    {
        var equipe = await db.Equipes.FindAsync(req.EquipeId);
        if (equipe is null) return Erro404("Equipe não encontrada.");

        if (await db.Pilotos.AnyAsync(p => p.Numero == req.Numero))
            return Erro400($"Já existe um piloto com o número {req.Numero}.");

        var piloto = new Piloto
        {
            Numero   = req.Numero,
            Nome     = req.Nome.Trim(),
            EquipeId = req.EquipeId,
            Ativo    = req.Ativo
        };
        db.Pilotos.Add(piloto);
        await db.SaveChangesAsync();

        await audit.RegistrarAsync("PILOTO_ADICIONADO", Uid, Login, "Piloto", piloto.Id,
            detalhes: $"{piloto.Nome} (#{piloto.Numero}) — {equipe.Nome}", ip: AuditoriaService.ExtrairIp(HttpContext));

        return Ok(new PilotoAdminDto(piloto.Id, piloto.Numero, piloto.Nome, piloto.EquipeId, equipe.Nome, equipe.Cor, piloto.Ativo));
    }

    // PATCH /api/admin/pilotos/{id} → atualiza equipe, status, nome e número
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, PilotoUpdateRequest req)
    {
        var piloto = await db.Pilotos.FindAsync(id);
        if (piloto is null) return Erro404("Piloto não encontrado.");

        var equipe = await db.Equipes.FindAsync(req.EquipeId);
        if (equipe is null) return Erro404("Equipe não encontrada.");

        if (req.Numero != piloto.Numero && await db.Pilotos.AnyAsync(p => p.Numero == req.Numero && p.Id != id))
            return Erro400($"Já existe um piloto com o número {req.Numero}.");

        piloto.Numero   = req.Numero;
        piloto.Nome     = req.Nome.Trim();
        piloto.EquipeId = req.EquipeId;
        piloto.Ativo    = req.Ativo;
        await db.SaveChangesAsync();

        await audit.RegistrarAsync("PILOTO_ATUALIZADO", Uid, Login, "Piloto", piloto.Id,
            detalhes: $"{piloto.Nome} (#{piloto.Numero}) — {equipe.Nome} — {(piloto.Ativo ? "ativo" : "inativo")}", ip: AuditoriaService.ExtrairIp(HttpContext));

        return Ok(new PilotoAdminDto(piloto.Id, piloto.Numero, piloto.Nome, piloto.EquipeId, equipe.Nome, equipe.Cor, piloto.Ativo));
    }
}
