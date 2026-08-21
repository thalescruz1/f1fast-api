// ============================================================
// DTOs: PilotoAdminDTOs
// ============================================================
// Gestão de pilotos pelo painel admin: listar todos (inclusive
// inativos), adicionar substituto, trocar de equipe e ativar/
// desativar. Rotas protegidas (Role = "Admin").
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.DTOs;

/// <summary>Piloto na visão do admin (inclui equipe e status).</summary>
public record PilotoAdminDto(
    int    Id,
    int    Numero,
    string Nome,
    int    EquipeId,
    string EquipeNome,
    string EquipeCor,
    bool   Ativo
);

/// <summary>Equipe para o dropdown de seleção.</summary>
public record EquipeDto(int Id, string Nome, string Cor);

/// <summary>Dados para adicionar um piloto novo (ex: substituto).</summary>
public record PilotoCreateRequest(
    [Required] int Numero,
    [Required, MaxLength(100)] string Nome,
    [Required] int EquipeId,
    bool Ativo = true
);

/// <summary>Dados para atualizar um piloto (equipe, status, nome, número).</summary>
public record PilotoUpdateRequest(
    [Required] int Numero,
    [Required, MaxLength(100)] string Nome,
    [Required] int EquipeId,
    bool Ativo
);
