namespace F1Fast.API.Models;

public class Resultado
{
    public int Id { get; set; }

    public int EtapaId { get; set; }
    public Etapa Etapa { get; set; } = null!;

    public int PoleId { get; set; }
    public int Pos1Id { get; set; }
    public int Pos2Id { get; set; }
    public int Pos3Id { get; set; }
    public int Pos4Id { get; set; }
    public int Pos5Id { get; set; }
    public int Pos6Id { get; set; }
    public int Pos7Id { get; set; }
    public int Pos8Id { get; set; }
    public int Pos9Id { get; set; }
    public int Pos10Id { get; set; }
    public int MelhorVoltaId { get; set; }

    public DateTime InseridoEm { get; set; } = DateTime.UtcNow;
}
