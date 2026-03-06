using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.DTOs;

public record PilotoDto(
    int    Id,
    int    Numero,
    string Nome,
    string Equipe,
    string CorEquipe
);

public record EtapaDto(
    int       Id,
    int       Numero,
    string    Nome,
    string    Circuito,
    string    Cidade,
    string    Pais,
    bool      Sprint,
    DateTime  PrazoQualify,
    DateTime? DataCorrida,
    bool      Encerrada,
    bool      PrazoExpirado
);

public record PalpiteRequest(
    [Required] int EtapaId,
    [Required] int PoleId,
    [Required] int Pos1Id,
    [Required] int Pos2Id,
    [Required] int Pos3Id,
    [Required] int Pos4Id,
    [Required] int Pos5Id,
    [Required] int Pos6Id,
    [Required] int Pos7Id,
    [Required] int Pos8Id,
    [Required] int Pos9Id,
    [Required] int Pos10Id,
    [Required] int MelhorVoltaId
);

public record PalpitePublicoDto(
    string   Login,
    string   Nome,
    string[] Posicoes,
    int?     PontosObtidos
);
