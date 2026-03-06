namespace F1Fast.API.DTOs;

public record RankingItemDto(
    int    Posicao,
    int    UsuarioId,
    string Login,
    string Nome,
    string Localizacao,
    int    TotalPontos,
    int    EtapasParticipadas,
    int    AcertosExatos,
    int    AcertosPole,
    int    AcertosMelhorVolta
);
