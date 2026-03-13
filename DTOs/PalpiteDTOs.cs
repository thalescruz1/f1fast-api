// ============================================================
// DTOs: PalpiteDTOs
// ============================================================
// DTOs relacionados a pilotos, etapas e palpites.
// Cada "record" aqui transporta um conjunto específico de dados
// entre a API e o frontend Angular.
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.DTOs;

/// <summary>
/// Dados de um piloto enviados para o frontend.
/// Inclui a cor da equipe para exibir a barrinha colorida na UI.
/// </summary>
public record PilotoDto(
    int    Id,        // ID do banco (usado como valor nos <select> do formulário)
    int    Numero,    // Número de corrida (ex: 44, 1, 5)
    string Nome,      // Nome completo (ex: "Lewis Hamilton")
    string Equipe,    // Nome da equipe (ex: "Ferrari")
    string CorEquipe  // Cor hex da equipe (ex: "#E8002D") para colorir a UI
);

/// <summary>
/// Dados de uma etapa do calendário enviados para o frontend.
/// PrazoExpirado é calculado na hora da requisição (não fica no banco).
/// </summary>
public record EtapaDto(
    int       Id,
    int       Numero,
    string    Nome,
    string    Circuito,
    string    Cidade,
    string    Pais,
    bool      Sprint,
    DateTime  PrazoQualify,  // Prazo limite para enviar palpites
    DateTime? DataCorrida,
    bool      Encerrada,
    bool      PrazoExpirado  // true se DateTime.UtcNow > PrazoQualify (calculado na query)
);

/// <summary>
/// Dados enviados pelo frontend ao submeter um palpite.
/// Contém os IDs dos pilotos escolhidos pelo usuário.
/// </summary>
public record PalpiteRequest(
    [Required] int EtapaId,       // Para qual corrida é este palpite
    [Required] int PoleId,        // Piloto escolhido para Pole Position
    [Required] int Pos1Id,        // Piloto escolhido para 1° lugar
    [Required] int Pos2Id,
    [Required] int Pos3Id,
    [Required] int Pos4Id,
    [Required] int Pos5Id,
    [Required] int Pos6Id,
    [Required] int Pos7Id,
    [Required] int Pos8Id,
    [Required] int Pos9Id,
    [Required] int Pos10Id,       // Piloto escolhido para 10° lugar
    [Required] int MelhorVoltaId  // Piloto escolhido para Melhor Volta
);

/// <summary>
/// Palpite de um usuário exibido publicamente após o prazo fechar.
/// Não expõe dados sensíveis — apenas o login, nome e as escolhas.
/// </summary>
public record PalpitePublicoDto(
    string   Login,         // Login do participante
    string   Nome,          // Nome do participante
    string[] Posicoes,      // Nomes dos pilotos nas 12 posições:
                            // [0]=Pole, [1]-[10]=Pos1 a Pos10, [11]=MelhorVolta
    int?     PontosObtidos, // null se a etapa ainda não foi encerrada
    DateTime EnviadoEm      // Data/hora em que o palpite foi enviado (ou última atualização)
);
