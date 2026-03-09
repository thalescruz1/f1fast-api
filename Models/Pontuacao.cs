// ============================================================
// MODELO: Pontuacao
// ============================================================
// Guarda o resultado detalhado de um usuário em uma etapa.
// Criado automaticamente pelo PontuacaoService após o admin
// lançar o resultado de uma corrida.
//
// Permite calcular o ranking somando pontos de todas as etapas,
// além de guardar estatísticas detalhadas para desempate.
// Regra de pontuação:
//   Pole acertada      → +2 pts
//   Melhor Volta certa → +3 pts
//   Posição exata      → +3 pts por posição
//   Erro de 1 posição  → +2 pts
//   Piloto certo errado → +1 pt
// ============================================================

namespace F1Fast.API.Models;

/// <summary>
/// Pontuação detalhada de um usuário em uma etapa específica.
/// Uma linha por usuário por etapa (índice único: UsuarioId + EtapaId).
/// </summary>
public class Pontuacao
{
    // Identificador único no banco
    public int Id { get; set; }

    // Qual usuário recebeu estes pontos (chave estrangeira)
    public int UsuarioId { get; set; }

    // Objeto Usuario completo (preenchido pelo Entity Framework)
    public Usuario Usuario { get; set; } = null!;

    // Em qual etapa estes pontos foram ganhos (chave estrangeira)
    public int EtapaId { get; set; }

    // Objeto Etapa completo (preenchido pelo Entity Framework)
    public Etapa Etapa { get; set; } = null!;

    // Total de pontos ganhos nesta etapa (máximo 35 por corrida)
    public int Pontos { get; set; }

    // Quantidade de posições onde acertou o piloto EXATO (+3 pts cada)
    public int AcertosExatos { get; set; }

    // Quantidade de posições com erro de apenas 1 lugar (+2 pts cada)
    public int AcertosUmaPos { get; set; }

    // Quantidade de pilotos certos, mas na posição errada (+1 pt cada)
    public int AcertosPiloto { get; set; }

    // true = acertou quem fez a pole position (+2 pts)
    public bool AcertouPole { get; set; }

    // true = acertou quem fez a melhor volta (+3 pts)
    public bool AcertouMelhorVolta { get; set; }
}
