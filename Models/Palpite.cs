// ============================================================
// MODELO: Palpite
// ============================================================
// Representa a previsão (palpite) de um usuário para uma etapa.
// Cada usuário pode ter NO MÁXIMO um palpite por etapa.
// O palpite pode ser atualizado até o prazo (PrazoQualify).
//
// O usuário escolhe:
//   - 1 piloto para Pole Position
//   - 10 pilotos em ordem (1° ao 10° lugar)
//   - 1 piloto para Melhor Volta
// ============================================================

using static F1Fast.API.Helpers.DateTimeHelper;

namespace F1Fast.API.Models;

/// <summary>
/// Previsão completa de um usuário para o resultado de uma corrida.
/// </summary>
public class Palpite
{
    // Identificador único no banco
    public int Id { get; set; }

    // ID do usuário que fez este palpite (chave estrangeira → tabela Usuarios)
    public int UsuarioId { get; set; }

    // Objeto Usuario completo (preenchido pelo Entity Framework com JOIN)
    public Usuario Usuario { get; set; } = null!;

    // ID da etapa para qual este palpite foi feito (chave estrangeira → tabela Etapas)
    public int EtapaId { get; set; }

    // Objeto Etapa completo (preenchido pelo Entity Framework com JOIN)
    public Etapa Etapa { get; set; } = null!;

    // Piloto que o usuário acredita que vai largar em 1° (Pole Position)
    public int PoleId { get; set; }

    // Pilotos que o usuário acredita que vão terminar em cada posição.
    // Pos1Id = vencedor, Pos2Id = 2° lugar, ... Pos10Id = 10° lugar.
    // REGRA IMPORTANTE: cada piloto só pode aparecer em UMA posição.
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

    // Piloto que o usuário acredita que vai fazer a melhor volta da corrida
    public int MelhorVoltaId { get; set; }

    // Data/hora do envio (ou última atualização) do palpite
    public DateTime EnviadoEm { get; set; } = AgoraBRT;

    // Pontos ganhos nesta etapa. "?" = null enquanto não encerrada.
    // Preenchido pelo PontuacaoService após o admin lançar o resultado real.
    public int? PontosObtidos { get; set; }
}
