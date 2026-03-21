// ============================================================
// CONTROLLER: EtapaController
// ============================================================
// Expõe informações sobre as corridas (etapas) do calendário F1 2026.
// Estas rotas são PÚBLICAS — qualquer pessoa pode acessar sem login.
//
//   GET /api/etapas         → retorna todas as 30 etapas
//   GET /api/etapas/proxima → retorna a próxima etapa com palpite aberto
// ============================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using F1Fast.API.DTOs;

namespace F1Fast.API.Controllers;

// AppDbContext db = contexto do banco de dados (injetado pelo .NET)
// Usamos ele para fazer queries no banco sem escrever SQL diretamente.
[ApiController, Route("api/etapas")]
public class EtapaController(AppDbContext db) : ApiControllerBase
{
    // GET /api/etapas → retorna todas as etapas ordenadas pelo número
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var agora  = DateTime.UtcNow; // hora atual para calcular PrazoExpirado

        // db.Etapas = acessa a tabela Etapas do banco
        // .OrderBy()  = ordena pelo campo Numero (1, 2, 3...)
        // .Select()   = projeta os dados para um DTO (descarta campos que não precisamos)
        // .ToListAsync() = executa o SQL e retorna uma lista (async = não bloqueia o servidor)
        var etapas = await db.Etapas
            .OrderBy(e => e.Numero)
            .Select(e => new EtapaDto(
                e.Id, e.Numero, e.Nome, e.Circuito, e.Cidade, e.Pais,
                e.Sprint, e.PrazoQualify, e.DataCorrida,
                e.Encerrada,
                agora > e.PrazoQualify,
                e.CircuitoTipo, e.CircuitoComprimento, e.Voltas, e.Distancia,
                e.Recordista, e.TempoRecord, e.AnoRecord,
                e.TreinoLivre1, e.TreinoLivre2, e.TreinoLivre3, e.Classificacao, e.CircuitoSvg,
                e.Cancelada))
            .ToListAsync();

        return Ok(etapas); // retorna 200 OK com a lista JSON
    }

    // GET /api/etapas/proxima → retorna a próxima etapa disponível para palpites
    [HttpGet("proxima")]
    public async Task<IActionResult> GetProxima()
    {
        var agora   = DateTime.UtcNow;

        // Filtramos etapas que:
        //   - Não foram encerradas (!e.Encerrada)
        //   - Não foram canceladas (!e.Cancelada)
        //   - Ainda estão no prazo (PrazoQualify > agora)
        // Pegamos a primeira pelo prazo mais próximo (OrderBy PrazoQualify)
        var proxima = await db.Etapas
            .Where(e => !e.Encerrada && !e.Cancelada && e.PrazoQualify > agora)
            .OrderBy(e => e.PrazoQualify)
            .Select(e => new EtapaDto(
                e.Id, e.Numero, e.Nome, e.Circuito, e.Cidade, e.Pais,
                e.Sprint, e.PrazoQualify, e.DataCorrida,
                e.Encerrada, false,
                e.CircuitoTipo, e.CircuitoComprimento, e.Voltas, e.Distancia,
                e.Recordista, e.TempoRecord, e.AnoRecord,
                e.TreinoLivre1, e.TreinoLivre2, e.TreinoLivre3, e.Classificacao, e.CircuitoSvg,
                e.Cancelada))
            .FirstOrDefaultAsync(); // null se não houver nenhuma etapa aberta

        // Se não houver próxima etapa → 404 Not Found
        // Se houver → 200 OK com os dados da etapa
        return proxima is null ? Erro404("Nenhuma etapa aberta no momento.") : Ok(proxima);
    }
}
