using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.Models;

public class Equipe
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Nome { get; set; } = "";

    [MaxLength(7)]
    public string Cor { get; set; } = "#888888";

    public ICollection<Piloto> Pilotos { get; set; } = [];
}
