using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.DTOs;

public record ResultadoRequest(
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
