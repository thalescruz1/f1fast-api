// ============================================================
// MODELO: Piloto
// ============================================================
// Representa um piloto de Fórmula 1 da temporada 2026.
// Cada objeto = uma linha na tabela "Pilotos" do banco.
// Os 22 pilotos são inseridos automaticamente no AppDbContext
// via "Seed" (dados pré-carregados na criação do banco).
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.Models;

/// <summary>
/// Representa um piloto de F1 disponível para ser escolhido nos palpites.
/// </summary>
public class Piloto
{
    // Identificador único do piloto no banco de dados
    public int Id { get; set; }

    // Número de corrida oficial (ex: 1 = Norris, 44 = Hamilton, 5 = Bortoleto)
    public int Numero { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = "";

    // Chave estrangeira: liga este piloto a uma equipe.
    // Guarda apenas o ID da equipe (número), não o objeto inteiro.
    public int EquipeId { get; set; }

    // Propriedade de navegação: acessa os dados completos da equipe.
    // Ex: piloto.Equipe.Nome → "McLaren", piloto.Equipe.Cor → "#FF8000"
    // "null!" = diz ao compilador que sempre terá valor (preenchido pelo EF).
    public Equipe Equipe { get; set; } = null!;

    // Se false, o piloto saiu da temporada mas mantemos no banco para
    // preservar o histórico de palpites já enviados. Padrão: ativo.
    public bool Ativo { get; set; } = true;
}
