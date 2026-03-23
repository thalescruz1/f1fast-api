// ============================================================
// CONTROLLER: EtapaAdminController (Admin)
// ============================================================
// Permite ao administrador visualizar e alterar o prazo de
// apostas (PrazoQualify) de qualquer etapa do calendário.
//
// Cenário de uso: o prazo de uma etapa já passou, mas o admin
// quer reabrir temporariamente para permitir novos palpites
// (ex: atraso no qualifying, ou correção de data errada).
//
//   GET   /api/admin/etapas          → lista todas as etapas com status
//   PATCH /api/admin/etapas/{id}/prazo → atualiza o PrazoQualify de uma etapa
//
// Ambos exigem [Authorize(Roles = "Admin")].
// ============================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using static F1Fast.API.Helpers.DateTimeHelper;

namespace F1Fast.API.Controllers.Admin;

// DTO de request para atualizar o prazo.
// record = tipo imutável e conciso do C# 9+, ideal para DTOs simples.
public record AtualizarPrazoRequest(DateTime NovoPrazo);

[ApiController, Route("api/admin/etapas"), Authorize(Roles = "Admin")]
public class EtapaAdminController(AppDbContext db) : ApiControllerBase
{
    // GET /api/admin/etapas → retorna todas as 30 etapas com seus prazos atuais
    // Usado pela tela "Gerenciar Prazos" do painel admin para listar as etapas.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var agora = AgoraBRT;

        var etapas = await db.Etapas
            .OrderBy(e => e.Numero)
            .Select(e => new
            {
                e.Id,
                e.Numero,
                e.Pais,
                e.Nome,
                e.Circuito,
                e.PrazoQualify,
                e.DataCorrida,
                e.Encerrada,
                e.Cancelada,
                // PrazoExpirado = calculado na hora, não salvo no banco
                PrazoExpirado = agora > e.PrazoQualify
            })
            .ToListAsync();

        return Ok(etapas);
    }

    // PATCH /api/admin/etapas/{id}/prazo → atualiza o prazo de uma etapa específica
    // [FromBody] = lê o AtualizarPrazoRequest do corpo da requisição HTTP (JSON)
    // Não requer que a etapa seja não-encerrada: o admin pode reabrir uma etapa
    // manualmente se necessário (ex: corrida cancelada/adiada).
    [HttpPatch("{id:int}/prazo")]
    public async Task<IActionResult> AtualizarPrazo(int id, [FromBody] AtualizarPrazoRequest req)
    {
        var etapa = await db.Etapas.FindAsync(id);
        if (etapa is null) return Erro404("Etapa não encontrada.");

        // Salva o novo prazo no banco. A partir deste momento, o PalpiteController
        // usará este novo valor para liberar ou bloquear apostas.
        etapa.PrazoQualify = req.NovoPrazo;
        await db.SaveChangesAsync();

        return Ok($"Prazo da etapa '{etapa.Nome}' atualizado para {req.NovoPrazo:dd/MM/yyyy HH:mm} UTC.");
    }

    // PATCH /api/admin/etapas/{id}/cancelar → alterna o status de cancelamento
    [HttpPatch("{id:int}/cancelar")]
    public async Task<IActionResult> ToggleCancelada(int id)
    {
        var etapa = await db.Etapas.FindAsync(id);
        if (etapa is null) return Erro404("Etapa não encontrada.");

        etapa.Cancelada = !etapa.Cancelada;
        await db.SaveChangesAsync();

        var status = etapa.Cancelada ? "cancelada" : "reativada";
        return Ok(new { mensagem = $"Etapa '{etapa.Nome}' {status}.", cancelada = etapa.Cancelada });
    }
}
