// ============================================================
// CONTROLLER: ResultadoController (Admin)
// ============================================================
// Painel administrativo para gerenciar resultados e participantes.
//
// [Authorize(Roles = "Admin")] = TODAS as rotas exigem:
//   1. Usuário autenticado (JWT válido)
//   2. Role = "Admin" (usuários normais recebem 403 Forbidden)
//
//   POST  /api/admin/resultado                     → lançar resultado de uma etapa
//   GET   /api/admin/resultado/{etapaId}           → consultar resultado lançado (IDs)
//   PUT   /api/admin/resultado/{etapaId}           → retificar resultado e recalcular pontos
//   GET   /api/admin/resultado/usuarios            → listar todos os participantes
//   PATCH /api/admin/resultado/usuarios/{id}/role  → promover/rebaixar usuário
// ============================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using F1Fast.API.Data;
using F1Fast.API.DTOs;
using F1Fast.API.Models;
using static F1Fast.API.Helpers.DateTimeHelper;
using F1Fast.API.Services;

namespace F1Fast.API.Controllers.Admin;

[ApiController, Route("api/admin/resultado"), Authorize(Roles = "Admin")]
public class ResultadoController(AppDbContext db, PontuacaoService pontuacao, AuditoriaService audit, PushNotificationService push) : ApiControllerBase
{
    // POST /api/admin/resultado → lança o resultado oficial de uma corrida
    // Após salvar o resultado, calcula automaticamente os pontos de todos os participantes.
    [HttpPost]
    public async Task<IActionResult> Inserir(ResultadoRequest req)
    {
        // Verifica se a etapa existe
        var etapa = await db.Etapas.FindAsync(req.EtapaId);
        if (etapa is null) return Erro404("Etapa não encontrada.");

        // Impede relançar resultado de etapa já encerrada
        if (etapa.Encerrada) return Erro400("Esta etapa já foi encerrada.");

        // Verifica se já existe um resultado para esta etapa
        var resultado = await db.Resultados.FirstOrDefaultAsync(r => r.EtapaId == req.EtapaId);

        if (resultado is null)
        {
            // Cria novo resultado
            db.Resultados.Add(new Resultado
            {
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
                Pos11Id       = req.Pos11Id,
                MelhorVoltaId = req.MelhorVoltaId
            });
            await db.SaveChangesAsync();
        }
        else
        {
            // Atualiza resultado existente (caso admin precise corrigir um erro)
            resultado.PoleId        = req.PoleId;
            resultado.Pos1Id        = req.Pos1Id;
            resultado.Pos2Id        = req.Pos2Id;
            resultado.Pos3Id        = req.Pos3Id;
            resultado.Pos4Id        = req.Pos4Id;
            resultado.Pos5Id        = req.Pos5Id;
            resultado.Pos6Id        = req.Pos6Id;
            resultado.Pos7Id        = req.Pos7Id;
            resultado.Pos8Id        = req.Pos8Id;
            resultado.Pos9Id        = req.Pos9Id;
            resultado.Pos10Id       = req.Pos10Id;
            resultado.Pos11Id       = req.Pos11Id;
            resultado.MelhorVoltaId = req.MelhorVoltaId;
            resultado.InseridoEm    = AgoraBRT;
            await db.SaveChangesAsync();
        }

        // Chama o serviço de pontuação para calcular e salvar os pontos de todos
        await pontuacao.CalcularPontosEtapaAsync(req.EtapaId);

        await audit.RegistrarAsync("RESULTADO_LANCADO", int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid) ? uid : null, User.FindFirstValue(ClaimTypes.Name), "Resultado", req.EtapaId, detalhes: "Resultado lançado e pontos calculados", ip: AuditoriaService.ExtrairIp(HttpContext));

        // Notifica todos os usuários (Web Push) que o resultado saiu
        await push.EnviarResultadoCadastradoAsync(req.EtapaId, retificado: false);

        return Ok("Resultado inserido e pontos calculados.");
    }

    // GET /api/admin/resultado/{etapaId} → resultado já lançado de uma etapa, com IDs
    // Usado pelo painel admin para pré-preencher o form no modo retificação.
    [HttpGet("{etapaId:int}")]
    public async Task<IActionResult> GetPorEtapa(int etapaId)
    {
        var resultado = await db.Resultados.FirstOrDefaultAsync(r => r.EtapaId == etapaId);
        if (resultado is null) return Erro404("Resultado ainda não cadastrado para esta etapa.");

        return Ok(new
        {
            resultado.EtapaId,
            resultado.PoleId,
            resultado.Pos1Id,  resultado.Pos2Id,  resultado.Pos3Id,  resultado.Pos4Id,
            resultado.Pos5Id,  resultado.Pos6Id,  resultado.Pos7Id,  resultado.Pos8Id,
            resultado.Pos9Id,  resultado.Pos10Id, resultado.Pos11Id,
            resultado.MelhorVoltaId,
            resultado.InseridoEm
        });
    }

    // PUT /api/admin/resultado/{etapaId} → retifica um resultado já lançado
    // (ex: reclassificação da FIA após a corrida) e recalcula os pontos de todos.
    // Diferente do POST, NÃO bloqueia etapa encerrada — esse é exatamente o caso de uso.
    // O recálculo é idempotente: sobrescreve Pontuacao e Palpite.PontosObtidos.
    [HttpPut("{etapaId:int}")]
    public async Task<IActionResult> Retificar(int etapaId, ResultadoRequest req)
    {
        if (req.EtapaId != etapaId)
            return Erro400("EtapaId do corpo da requisição não confere com a rota.");

        var etapa = await db.Etapas.FindAsync(etapaId);
        if (etapa is null) return Erro404("Etapa não encontrada.");

        // Retificação só vale para resultado já lançado; lançamento inicial é via POST
        var resultado = await db.Resultados.FirstOrDefaultAsync(r => r.EtapaId == etapaId);
        if (resultado is null) return Erro404("Resultado ainda não cadastrado. Use o lançamento normal.");

        // Snapshot do resultado anterior para a auditoria (histórico da reclassificação)
        var anterior = System.Text.Json.JsonSerializer.Serialize(new
        {
            resultado.PoleId,
            resultado.Pos1Id,  resultado.Pos2Id,  resultado.Pos3Id,  resultado.Pos4Id,
            resultado.Pos5Id,  resultado.Pos6Id,  resultado.Pos7Id,  resultado.Pos8Id,
            resultado.Pos9Id,  resultado.Pos10Id, resultado.Pos11Id,
            resultado.MelhorVoltaId
        });

        resultado.PoleId        = req.PoleId;
        resultado.Pos1Id        = req.Pos1Id;
        resultado.Pos2Id        = req.Pos2Id;
        resultado.Pos3Id        = req.Pos3Id;
        resultado.Pos4Id        = req.Pos4Id;
        resultado.Pos5Id        = req.Pos5Id;
        resultado.Pos6Id        = req.Pos6Id;
        resultado.Pos7Id        = req.Pos7Id;
        resultado.Pos8Id        = req.Pos8Id;
        resultado.Pos9Id        = req.Pos9Id;
        resultado.Pos10Id       = req.Pos10Id;
        resultado.Pos11Id       = req.Pos11Id;
        resultado.MelhorVoltaId = req.MelhorVoltaId;
        resultado.InseridoEm    = AgoraBRT;
        await db.SaveChangesAsync();

        // Recalcula os pontos de todos os palpites da etapa (sobrescreve valores antigos)
        await pontuacao.CalcularPontosEtapaAsync(etapaId);

        await audit.RegistrarAsync("RESULTADO_RETIFICADO", int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid) ? uid : null, User.FindFirstValue(ClaimTypes.Name), "Resultado", etapaId, detalhes: $"Resultado retificado e pontos recalculados. Anterior: {anterior}", ip: AuditoriaService.ExtrairIp(HttpContext));

        // Notifica todos os usuários (Web Push) que o resultado foi retificado
        await push.EnviarResultadoCadastradoAsync(etapaId, retificado: true);

        return Ok("Resultado retificado e pontos recalculados.");
    }

    // GET /api/admin/resultado/usuarios → lista todos os participantes cadastrados
    // Usado pela aba "Participantes" do painel admin
    [HttpGet("usuarios")]
    public async Task<IActionResult> GetUsuarios()
    {
        // "new { ... }" = objeto anônimo: seleciona apenas os campos necessários
        // (não expõe SenhaHash nem tokens sensíveis)
        var lista = await db.Usuarios
            .Select(u => new { u.Id, u.Login, u.Nome, u.Sobrenome, u.Email, u.Localizacao, u.Role, u.CriadoEm })
            .OrderBy(u => u.Nome)
            .ToListAsync();

        return Ok(lista);
    }

    // PATCH /api/admin/resultado/usuarios/{id}/role → altera o papel de um usuário
    // [HttpPatch] = atualização parcial (muda só o campo Role, não o objeto inteiro)
    // [FromBody] = lê o valor diretamente do corpo da requisição HTTP
    [HttpPatch("usuarios/{id:int}/role")]
    public async Task<IActionResult> AlterarRole(int id, [FromBody] string novaRole)
    {
        // Valida que só aceita "Admin" ou "User" como roles válidas
        if (novaRole is not ("Admin" or "User"))
            return Erro400("Role inválida. Use 'Admin' ou 'User'.");

        var user = await db.Usuarios.FindAsync(id);
        if (user is null) return Erro404("Usuário não encontrado.");

        user.Role = novaRole;
        await db.SaveChangesAsync();

        await audit.RegistrarAsync("ROLE_ALTERADA", int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var adminId) ? adminId : null, User.FindFirstValue(ClaimTypes.Name), "Usuario", id, detalhes: $"Nova role: {novaRole}", ip: AuditoriaService.ExtrairIp(HttpContext));

        return Ok();
    }
}
