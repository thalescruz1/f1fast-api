// ============================================================
// DTOs: RankingDTOs
// ============================================================
// DTOs para a classificação geral do campeonato CV2026.
// ============================================================

namespace F1Fast.API.DTOs;

/// <summary>
/// Um item da tabela de ranking — representa um participante e sua
/// pontuação acumulada em todas as etapas disputadas até o momento.
/// A ordem de desempate é: TotalPontos → AcertosMV → AcertosPole → AcertosExatos.
/// </summary>
public record RankingItemDto(
    int    Posicao,            // Posição no ranking (1°, 2°, 3°...)
    int    UsuarioId,          // ID do usuário no banco
    string Login,              // Login do participante
    string Nome,               // Nome completo (Nome + Sobrenome)
    string Localizacao,        // Cidade/estado do participante
    int    TotalPontos,        // Soma de pontos em todas as etapas disputadas
    int    EtapasParticipadas, // Quantas etapas o usuário enviou palpite
    int    AcertosExatos,      // Total de acertos exatos (critério de desempate 3)
    int    AcertosPole,        // Total de poles acertadas (critério de desempate 2)
    int    AcertosMelhorVolta  // Total de melhores voltas acertadas (critério de desempate 1)
);
