// ============================================================
// DTOs: ResultadoDTOs
// ============================================================
// DTOs do painel administrativo para lançar o resultado oficial
// de uma corrida. Rota protegida: só Role="Admin" tem acesso.
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.DTOs;

/// <summary>
/// Dados enviados pelo administrador ao lançar o resultado de uma corrida.
/// Após receber estes dados, a API automaticamente:
///   1. Salva o resultado no banco
///   2. Compara com todos os palpites daquela etapa
///   3. Calcula e salva os pontos de cada participante
///   4. Marca a etapa como "Encerrada"
/// </summary>
public record ResultadoRequest(
    [Required] int EtapaId,       // ID da etapa que está sendo encerrada
    [Required] int PoleId,        // Quem fez a pole position real
    [Required] int Pos1Id,        // Vencedor real da corrida
    [Required] int Pos2Id,
    [Required] int Pos3Id,
    [Required] int Pos4Id,
    [Required] int Pos5Id,
    [Required] int Pos6Id,
    [Required] int Pos7Id,
    [Required] int Pos8Id,
    [Required] int Pos9Id,
    [Required] int Pos10Id,       // 10° colocado real
    [Required] int Pos11Id,       // 11° colocado real (necessário para ±1 na pontuação do 10°)
    [Required] int MelhorVoltaId  // Quem fez a melhor volta real
);
