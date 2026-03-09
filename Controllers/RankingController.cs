// ============================================================
// CONTROLLER: RankingController
// ============================================================
// Expõe a classificação geral do campeonato CV2026.
// Rota PÚBLICA — qualquer pessoa (inclusive sem login) pode ver o ranking.
//
//   GET /api/ranking → retorna a tabela de classificação ordenada por pontos
// ============================================================

using Microsoft.AspNetCore.Mvc;
using F1Fast.API.Services;

namespace F1Fast.API.Controllers;

// PontuacaoService pontuacao = injeção de dependência do serviço de pontuação
[ApiController, Route("api/ranking")]
public class RankingController(PontuacaoService pontuacao) : ControllerBase
{
    // GET /api/ranking → retorna a classificação geral de todos os participantes
    // "=>" é uma "expression body" — forma compacta de escrever um método de uma linha
    [HttpGet]
    public async Task<IActionResult> GetGeral() =>
        Ok(await pontuacao.GetRankingGeralAsync());
}
