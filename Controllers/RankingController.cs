// ============================================================
// CONTROLLER: RankingController
// ============================================================
// Expõe a classificação geral do campeonato CV2026.
// Rota PÚBLICA — qualquer pessoa (inclusive sem login) pode ver o ranking.
//
//   GET /api/ranking → retorna a tabela de classificação ordenada por pontos
// ============================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using F1Fast.API.Services;

namespace F1Fast.API.Controllers;

// PontuacaoService pontuacao = injeção de dependência do serviço de pontuação
[ApiController, Route("api/ranking")]
public class RankingController(PontuacaoService pontuacao, AppDbContext db) : ControllerBase
{
    // GET /api/ranking → retorna a classificação geral de todos os participantes
    // "=>" é uma "expression body" — forma compacta de escrever um método de uma linha
    [HttpGet]
    public async Task<IActionResult> GetGeral() =>
        Ok(await pontuacao.GetRankingGeralAsync());

    // GET /api/ranking/ultimo-gp → retorna o nome do último GP com resultado cadastrado
    // Usado no frontend para exibir "Classificação atualizada após o GP de X"
    [HttpGet("ultimo-gp")]
    public async Task<IActionResult> GetUltimoGp()
    {
        var nomeGp = await db.Etapas
            .Where(e => e.Encerrada)
            .OrderByDescending(e => e.Numero)
            .Select(e => e.Nome)
            .FirstOrDefaultAsync();

        return Ok(new { nomeGp });
    }
}
