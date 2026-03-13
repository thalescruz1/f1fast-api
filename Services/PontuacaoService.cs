// ============================================================
// SERVICE: PontuacaoService
// ============================================================
// O "coração" do sistema: calcula os pontos de cada participante
// comparando o palpite com o resultado real da corrida.
//
// Regras de pontuação CV2026:
//   Pole acertada              → +2 pts
//   Melhor Volta acertada      → +3 pts
//   Posição exata              → +3 pts por posição
//   Erro de 1 posição          → +2 pts por posição
//   Piloto certo, posição errada (>1) → +1 pt por posição
//   Máximo por etapa: 35 pts (2 + 3 + 10×3)
// ============================================================

using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using F1Fast.API.DTOs;
using F1Fast.API.Models;

namespace F1Fast.API.Services;

public class PontuacaoService(AppDbContext db)
{
    /// <summary>
    /// Calcula os pontos de todos os participantes para uma etapa específica.
    /// Chamado automaticamente após o admin lançar o resultado de uma corrida.
    /// </summary>
    public async Task CalcularPontosEtapaAsync(int etapaId)
    {
        // Busca o resultado oficial da etapa. "?? throw" = lança exceção se null
        var resultado = await db.Resultados.FirstOrDefaultAsync(r => r.EtapaId == etapaId)
            ?? throw new Exception("Resultado não encontrado.");

        // Busca todos os palpites desta etapa
        var palpites = await db.Palpites
            .Where(p => p.EtapaId == etapaId)
            .ToListAsync();

        // Array com as 10 posições reais da corrida
        // resultadoPos[0] = vencedor real, [1] = 2° real, etc.
        int[] resultadoPos =
        [
            resultado.Pos1Id, resultado.Pos2Id, resultado.Pos3Id, resultado.Pos4Id,
            resultado.Pos5Id, resultado.Pos6Id, resultado.Pos7Id, resultado.Pos8Id,
            resultado.Pos9Id, resultado.Pos10Id
        ];

        // Processa cada palpite individualmente
        foreach (var palpite in palpites)
        {
            // Array com as 10 posições que o usuário chutou
            int[] palpitePos =
            [
                palpite.Pos1Id, palpite.Pos2Id, palpite.Pos3Id, palpite.Pos4Id,
                palpite.Pos5Id, palpite.Pos6Id, palpite.Pos7Id, palpite.Pos8Id,
                palpite.Pos9Id, palpite.Pos10Id
            ];

            int pontos = 0, acertosExatos = 0, acertosUmaPos = 0, acertosPiloto = 0;

            // Verifica Pole Position
            bool acertouPole = palpite.PoleId == resultado.PoleId;
            if (acertouPole) pontos += 2;

            // Verifica Melhor Volta
            bool acertouMV = palpite.MelhorVoltaId == resultado.MelhorVoltaId;
            if (acertouMV) pontos += 3;

            // Verifica cada posição (índice 0 = 1° lugar, índice 9 = 10° lugar)
            for (int i = 0; i < 10; i++)
            {
                int pilPalpite   = palpitePos[i];   // piloto que o usuário colocou na posição i
                int pilResultado = resultadoPos[i]; // piloto que realmente ficou na posição i

                if (pilPalpite == pilResultado)
                {
                    // Acerto exato: piloto certo NA posição certa
                    pontos += 3; acertosExatos++;
                }
                else if (i == 9 && pilPalpite == resultado.Pos11Id)
                {
                    // Caso especial: chutou 10° e o piloto chegou 11°.
                    // Pos11Id não está no array resultadoPos (top-10), mas o erro é de 1 posição → +2
                    pontos += 2; acertosUmaPos++;
                }
                else if (resultadoPos.Contains(pilPalpite) &&
                         Math.Abs(i - Array.IndexOf(resultadoPos, pilPalpite)) == 1)
                {
                    // Acerto com erro de 1 posição: piloto está no top-10 E errou apenas 1 lugar
                    // Math.Abs = valor absoluto (distância sem sinal)
                    // Array.IndexOf = posição real do piloto escolhido no resultado
                    pontos += 2; acertosUmaPos++;
                }
                else if (resultadoPos.Contains(pilPalpite))
                {
                    // Piloto certo, mas posição errada (erro > 1): está no top-10 mas longe
                    pontos += 1; acertosPiloto++;
                }
                // Se o piloto escolhido não está no top-10 real nem no 11° → 0 pontos
            }

            // Atualiza o palpite com os pontos calculados
            palpite.PontosObtidos = pontos;

            // Cria ou atualiza o registro de Pontuacao deste usuário nesta etapa
            var pont = await db.Pontuacoes
                .FirstOrDefaultAsync(p => p.UsuarioId == palpite.UsuarioId && p.EtapaId == etapaId);

            if (pont is null)
            {
                // Primeira vez calculando: cria registro novo
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
                // Recalculando (admin corrigiu resultado): atualiza registro existente
                pont.Pontos             = pontos;
                pont.AcertosExatos      = acertosExatos;
                pont.AcertosUmaPos      = acertosUmaPos;
                pont.AcertosPiloto      = acertosPiloto;
                pont.AcertouPole        = acertouPole;
                pont.AcertouMelhorVolta = acertouMV;
            }
        }

        // Marca a etapa como encerrada após calcular todos os pontos
        var etapa = await db.Etapas.FindAsync(etapaId);
        if (etapa is not null) etapa.Encerrada = true;

        // Persiste todas as mudanças em uma única transação
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Retorna a classificação geral do campeonato, somando pontos de todas as etapas.
    /// Usa GroupBy para agregar os dados de pontuação por usuário.
    /// </summary>
    public async Task<List<RankingItemDto>> GetRankingGeralAsync()
    {
        var dados = await db.Pontuacoes
            .Include(p => p.Usuario) // JOIN com tabela Usuarios
            .GroupBy(p => p.UsuarioId) // agrupa por usuário (como um GROUP BY no SQL)
            .Select(g => new
            {
                UsuarioId          = g.Key, // g.Key = o UsuarioId pelo qual agrupamos
                Login              = g.First().Usuario.Login,
                Nome               = g.First().Usuario.Nome + " " + g.First().Usuario.Sobrenome,
                Localizacao        = g.First().Usuario.Localizacao,
                TotalPontos        = g.Sum(x => x.Pontos),          // soma todos os pontos
                EtapasParticipadas = g.Count(),                      // quantas etapas fez palpite
                AcertosExatos      = g.Sum(x => x.AcertosExatos),   // soma acertos exatos
                AcertosPole        = g.Count(x => x.AcertouPole),   // conta etapas com pole certa
                AcertosMV          = g.Count(x => x.AcertouMelhorVolta) // conta MV certas
            })
            // Ordenação com critérios de desempate em cascata:
            .OrderByDescending(g => g.TotalPontos)        // 1° critério: mais pontos
            .ThenByDescending(g => g.AcertosMV)           // desempate 1: mais MV certas
            .ThenByDescending(g => g.AcertosPole)         // desempate 2: mais poles certas
            .ThenByDescending(g => g.AcertosExatos)       // desempate 3: mais posições exatas
            .ThenByDescending(g => g.EtapasParticipadas)  // desempate 4: participou de mais etapas
            .ToListAsync();

        // ".Select((p, i)" com índice: gera a posição do ranking (1°, 2°, 3°...)
        // "i + 1" porque o índice começa em 0 mas queremos mostrar a partir de 1
        return dados.Select((p, i) => new RankingItemDto(
            i + 1, p.UsuarioId, p.Login, p.Nome, p.Localizacao,
            p.TotalPontos, p.EtapasParticipadas,
            p.AcertosExatos, p.AcertosPole, p.AcertosMV
        )).ToList();
    }
}
