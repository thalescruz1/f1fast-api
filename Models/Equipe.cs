// ============================================================
// MODELO: Equipe
// ============================================================
// Representa uma escuderia (equipe) de Fórmula 1.
// Cada objeto = uma linha na tabela "Equipes" do banco.
// As 11 equipes de 2026 são inseridas automaticamente
// no AppDbContext via "Seed" na configuração inicial.
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.Models;

/// <summary>
/// Representa uma escuderia/equipe de F1 da temporada 2026.
/// </summary>
public class Equipe
{
    // Identificador único da equipe no banco
    public int Id { get; set; }

    // Nome da equipe (ex: "McLaren", "Ferrari", "Red Bull")
    [Required, MaxLength(50)]
    public string Nome { get; set; } = "";

    // Cor oficial da equipe em formato hexadecimal CSS.
    // Ex: "#FF8000" = laranja McLaren | "#E8002D" = vermelho Ferrari.
    // Usada no frontend para colorir as barrinhas ao lado dos pilotos.
    [MaxLength(7)] // '#' + 6 dígitos hex = 7 caracteres
    public string Cor { get; set; } = "#888888"; // cinza como cor padrão

    // Lista de pilotos desta equipe (propriedade de navegação).
    // Ex: equipe.Pilotos → [Lando Norris, Oscar Piastri] para McLaren
    public ICollection<Piloto> Pilotos { get; set; } = [];
}
