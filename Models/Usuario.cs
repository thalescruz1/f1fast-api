using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string Login { get; set; } = "";

    [Required, MaxLength(100)]
    public string Nome { get; set; } = "";

    [Required, MaxLength(100)]
    public string Sobrenome { get; set; } = "";

    [Required, MaxLength(14)]
    public string Cpf { get; set; } = "";

    [Required]
    public string SenhaHash { get; set; } = "";

    [Required, MaxLength(100)]
    public string Localizacao { get; set; } = "";

    [Required, MaxLength(150)]
    public string Email { get; set; } = "";

    // "User" | "Admin"
    public string Role { get; set; } = "User";

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<Palpite> Palpites { get; set; } = [];
}
