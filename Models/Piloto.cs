using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.Models;

public class Piloto
{
    public int Id { get; set; }
    public int Numero { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = "";

    public int EquipeId { get; set; }
    public Equipe Equipe { get; set; } = null!;

    public bool Ativo { get; set; } = true;
}
