using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using F1Fast.API.DTOs;
using F1Fast.API.Models;

namespace F1Fast.API.Services;

public class PontuacaoService(AppDbContext db)
{
    // Regras CV2026
    // Pole acerto:          +2 pts
    // Melhor Volta acerto:  +3 pts
    // Posição exata:        +3 pts
    // Erro de 1 posição:    +2 pts
    // Piloto certo, pos errada (>1): +1 pt
    // Máximo por etapa:     35 pts

    public async Task CalcularPontosEtapaAsync(int etapaId)
    {
        var resultado = await db.Resultados.FirstOrDefaultAsync(r => r.EtapaId == etapaId)
            ?? throw new Exception("Resultado não encontrado.");

        var palpites = await db.Palpites
            .Where(p => p.EtapaId == etapaId)
            .ToListAsync();

        int[] resultadoPos =
        [
            resultado.Pos1Id, resultado.Pos2Id, resultado.Pos3Id, resultado.Pos4Id,
            resultado.Pos5Id, resultado.Pos6Id, resultado.Pos7Id, resultado.Pos8Id,
            resultado.Pos9Id, resultado.Pos10Id
        ];

        foreach (var palpite in palpites)
        {
            int[] palpitePos =
            [
                palpite.Pos1Id, palpite.Pos2Id, palpite.Pos3Id, palpite.Pos4Id,
                palpite.Pos5Id, palpite.Pos6Id, palpite.Pos7Id, palpite.Pos8Id,
                palpite.Pos9Id, palpite.Pos10Id
            ];

            int pontos = 0, acertosExatos = 0, acertosUmaPos = 0, acertosPiloto = 0;

            bool acertouPole = palpite.PoleId == resultado.PoleId;
            if (acertouPole) pontos += 2;

            bool acertouMV = palpite.MelhorVoltaId == resultado.MelhorVoltaId;
            if (acertouMV) pontos += 3;

            for (int i = 0; i < 10; i++)
            {
                int pilPalpite   = palpitePos[i];
                int pilResultado = resultadoPos[i];

                if (pilPalpite == pilResultado)
                {
                    pontos += 3; acertosExatos++;
                }
                else if (resultadoPos.Contains(pilPalpite) &&
                         Math.Abs(i - Array.IndexOf(resultadoPos, pilPalpite)) == 1)
                {
                    pontos += 2; acertosUmaPos++;
                }
                else if (resultadoPos.Contains(pilPalpite))
                {
                    pontos += 1; acertosPiloto++;
                }
            }

            palpite.PontosObtidos = pontos;

            var pont = await db.Pontuacoes
                .FirstOrDefaultAsync(p => p.UsuarioId == palpite.UsuarioId && p.EtapaId == etapaId);

            if (pont is null)
            {
                db.Pontuacoes.Add(new Pontuacao
                {
                    UsuarioId          = palpite.UsuarioId,
                    EtapaId            = etapaId,
                    Pontos             = pontos,
                    AcertosExatos      = acertosExatos,
                    AcertosUmaPos      = acertosUmaPos,
                    AcertosPiloto      = acertosPiloto,
                    AcertouPole        = acertouPole,
                    AcertouMelhorVolta = acertouMV
                });
            }
            else
            {
                pont.Pontos             = pontos;
                pont.AcertosExatos      = acertosExatos;
                pont.AcertosUmaPos      = acertosUmaPos;
                pont.AcertosPiloto      = acertosPiloto;
                pont.AcertouPole        = acertouPole;
                pont.AcertouMelhorVolta = acertouMV;
            }
        }

        var etapa = await db.Etapas.FindAsync(etapaId);
        if (etapa is not null) etapa.Encerrada = true;

        await db.SaveChangesAsync();
    }

    public async Task<List<RankingItemDto>> GetRankingGeralAsync()
    {
        var dados = await db.Pontuacoes
            .Include(p => p.Usuario)
            .GroupBy(p => p.UsuarioId)
            .Select(g => new
            {
                UsuarioId          = g.Key,
                Login              = g.First().Usuario.Login,
                Nome               = g.First().Usuario.Nome + " " + g.First().Usuario.Sobrenome,
                Localizacao        = g.First().Usuario.Localizacao,
                TotalPontos        = g.Sum(x => x.Pontos),
                EtapasParticipadas = g.Count(),
                AcertosExatos      = g.Sum(x => x.AcertosExatos),
                AcertosPole        = g.Count(x => x.AcertouPole),
                AcertosMV          = g.Count(x => x.AcertouMelhorVolta)
            })
            .OrderByDescending(g => g.TotalPontos)
            .ThenByDescending(g => g.AcertosMV)
            .ThenByDescending(g => g.AcertosPole)
            .ThenByDescending(g => g.AcertosExatos)
            .ThenByDescending(g => g.EtapasParticipadas)
            .ToListAsync();

        return dados.Select((p, i) => new RankingItemDto(
            i + 1, p.UsuarioId, p.Login, p.Nome, p.Localizacao,
            p.TotalPontos, p.EtapasParticipadas,
            p.AcertosExatos, p.AcertosPole, p.AcertosMV
        )).ToList();
    }
}
