using Microsoft.AspNetCore.Mvc;
using F1Fast.API.Services;

namespace F1Fast.API.Controllers;

[ApiController, Route("api/ranking")]
public class RankingController(PontuacaoService pontuacao) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGeral() =>
        Ok(await pontuacao.GetRankingGeralAsync());
}
