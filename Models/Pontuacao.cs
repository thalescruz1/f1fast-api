namespace F1Fast.API.Models;

public class Pontuacao
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int EtapaId { get; set; }
    public Etapa Etapa { get; set; } = null!;

    public int Pontos { get; set; }
    public int AcertosExatos { get; set; }
    public int AcertosUmaPos { get; set; }
    public int AcertosPiloto { get; set; }
    public bool AcertouPole { get; set; }
    public bool AcertouMelhorVolta { get; set; }
}
