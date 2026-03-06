using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.Models;

public class Etapa
{
    public int Id { get; set; }
    public int Numero { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = "";

    [Required, MaxLength(100)]
    public string Circuito { get; set; } = "";

    [Required, MaxLength(100)]
    public string Cidade { get; set; } = "";

    [MaxLength(10)]
    public string Pais { get; set; } = "";

    public bool Sprint { get; set; } = false;

    public DateTime PrazoQualify { get; set; }
    public DateTime? DataCorrida { get; set; }

    public bool Encerrada { get; set; } = false;

    public ICollection<Palpite> Palpites { get; set; } = [];
    public Resultado? Resultado { get; set; }
}
