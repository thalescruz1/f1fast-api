// ============================================================
// MODELO: Resultado
// ============================================================
// Guarda o resultado OFICIAL de uma corrida, lançado pelo admin.
// Estrutura idêntica ao Palpite, mas com os dados REAIS da corrida.
//
// Fluxo de uso:
//   1. Admin acessa /admin/resultado após a corrida terminar
//   2. Preenche quem fez pole, as 10 posições e a melhor volta
//   3. O sistema salva o Resultado e chama PontuacaoService
//   4. PontuacaoService compara cada Palpite com o Resultado
//   5. Pontos são calculados e a etapa é marcada como Encerrada
// ============================================================

namespace F1Fast.API.Models;

/// <summary>
/// Resultado oficial de uma etapa, inserido pelo administrador.
/// Cada etapa pode ter no máximo UM resultado (índice único no banco).
/// </summary>
public class Resultado
{
    // Identificador único no banco
    public int Id { get; set; }

    // ID da etapa à qual este resultado pertence (chave estrangeira)
    public int EtapaId { get; set; }

    // Objeto Etapa completo (preenchido pelo Entity Framework)
    public Etapa Etapa { get; set; } = null!;

    // Piloto que fez a pole position real
    public int PoleId { get; set; }

    // Pilotos que chegaram em cada posição real da corrida.
    // Pos1Id = vencedor real, Pos2Id = 2° real, ... Pos10Id = 10° real
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

    // Piloto que fez a melhor volta real da corrida
    public int MelhorVoltaId { get; set; }

    // Data/hora em que o admin inseriu este resultado no sistema
    public DateTime InseridoEm { get; set; } = DateTime.UtcNow;
}
