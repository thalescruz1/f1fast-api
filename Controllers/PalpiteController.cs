// ============================================================
// CONTROLLER: PalpiteController
// ============================================================
// Gerencia o envio e consulta de palpites dos usuários.
//
// [Authorize] = TODAS as rotas exigem login (JWT válido), EXCETO
//              as que têm [AllowAnonymous] explicitamente.
//
//   POST /api/palpites                    → enviar/atualizar palpite (requer login)
//   GET  /api/palpites/{etapaId}/meu      → ver meu palpite (requer login)
//   GET  /api/palpites/{etapaId}/publico  → ver palpites de todos (público, após prazo)
// ============================================================

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
    // Propriedade que lê o ID do usuário logado a partir do JWT.
    // ClaimTypes.NameIdentifier = campo do token que guarda o ID do usuário.
    // "=>" sem bloco { } = propriedade calculada (recalculada a cada acesso).
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // POST /api/palpites → envia um palpite (ou atualiza se já enviou antes)
    [HttpPost]
    public async Task<IActionResult> Enviar(PalpiteRequest req)
    {
        // Verifica se a etapa existe no banco
        var etapa = await db.Etapas.FindAsync(req.EtapaId);
        if (etapa is null) return NotFound("Etapa não encontrada.");

        // Verifica se o prazo não expirou
        if (DateTime.UtcNow > etapa.PrazoQualify)
            return BadRequest("Prazo encerrado para esta etapa.");

        // Agrupa os IDs das 10 posições para verificar duplicatas
        int[] posicoes = [
            req.Pos1Id, req.Pos2Id, req.Pos3Id, req.Pos4Id, req.Pos5Id,
            req.Pos6Id, req.Pos7Id, req.Pos8Id, req.Pos9Id, req.Pos10Id
        ];

        // .Distinct() remove duplicatas; se Count() != 10, há piloto repetido
        if (posicoes.Distinct().Count() != 10)
            return BadRequest("O mesmo piloto não pode aparecer em mais de uma posição.");

        // Verifica se o usuário já tem um palpite para esta etapa
        var existente = await db.Palpites
            .FirstOrDefaultAsync(p => p.UsuarioId == UserId && p.EtapaId == req.EtapaId);

        if (existente is not null)
        {
            // Atualiza o palpite existente (usuário pode alterar até o prazo)
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
            existente.PontosObtidos = null; // reseta pontos ao alterar palpite
        }
        else
        {
            // Cria um novo palpite
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

        // Persiste as mudanças no banco de dados
        await db.SaveChangesAsync();
        return Ok("Palpite enviado com sucesso.");
    }

    // GET /api/palpites/{etapaId}/meu → retorna meu palpite para uma etapa
    // "{etapaId:int}" = parâmetro na URL com restrição de tipo (deve ser int)
    [HttpGet("{etapaId:int}/meu")]
    public async Task<IActionResult> GetMeu(int etapaId)
    {
        var palpite = await db.Palpites
            .FirstOrDefaultAsync(p => p.UsuarioId == UserId && p.EtapaId == etapaId);

        return palpite is null ? NotFound() : Ok(palpite);
    }

    // GET /api/palpites/{etapaId}/resultado → resultado oficial público (após prazo)
    // Retorna as posições do resultado oficial ou null se ainda não cadastrado
    [HttpGet("{etapaId:int}/resultado"), AllowAnonymous]
    public async Task<IActionResult> GetResultado(int etapaId)
    {
        var etapa = await db.Etapas.FindAsync(etapaId);
        if (etapa is null) return NotFound();

        if (DateTime.UtcNow < etapa.PrazoQualify)
            return BadRequest("Resultado não disponível antes do prazo.");

        var resultado = await db.Resultados.FirstOrDefaultAsync(r => r.EtapaId == etapaId);
        if (resultado is null) return NotFound("Resultado ainda não cadastrado.");

        var pilotos = await db.Pilotos
            .ToDictionaryAsync(p => p.Id, p => $"{p.Numero} — {p.Nome}");

        var posicoes = new[]
        {
            pilotos.GetValueOrDefault(resultado.PoleId,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos1Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos2Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos3Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos4Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos5Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos6Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos7Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos8Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos9Id,        "?"),
            pilotos.GetValueOrDefault(resultado.Pos10Id,       "?"),
            pilotos.GetValueOrDefault(resultado.MelhorVoltaId, "?")
        };

        return Ok(new { posicoes });
    }

    // GET /api/palpites/{etapaId}/publico → palpites de TODOS após o prazo
    // [AllowAnonymous] = sobrescreve o [Authorize] da classe — permite acesso sem login
    [HttpGet("{etapaId:int}/publico"), AllowAnonymous]
    public async Task<IActionResult> GetPublico(int etapaId)
    {
        var etapa = await db.Etapas.FindAsync(etapaId);
        if (etapa is null) return NotFound();

        // Bloqueia acesso antes do prazo (os palpites ficam ocultos até o qualifying)
        if (DateTime.UtcNow < etapa.PrazoQualify)
            return BadRequest("Os palpites ficam visíveis após o prazo.");

        // Cria um dicionário id→nome para resolver os IDs em nomes de pilotos
        // Ex: { 17: "44 — Lewis Hamilton", 9: "16 — Charles Leclerc" }
        var pilotos = await db.Pilotos
            .ToDictionaryAsync(p => p.Id, p => $"{p.Numero} — {p.Nome}");

        // Busca todos os palpites desta etapa com os dados do usuário (Include = JOIN)
        // Ordena pelos mais pontuados primeiro (null vai para o fim)
        var palpites = await db.Palpites
            .Include(p => p.Usuario)
            .Where(p => p.EtapaId == etapaId)
            .OrderByDescending(p => p.PontosObtidos)
            .Select(p => new PalpitePublicoDto(
                p.Usuario.Login,
                p.Usuario.Nome,
                new[]
                {
                    // GetValueOrDefault(id, "?") → retorna o nome ou "?" se não encontrar
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
                p.PontosObtidos,
                p.EnviadoEm
            ))
            .ToListAsync();

        return Ok(palpites);
    }
}
