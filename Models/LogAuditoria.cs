using System.ComponentModel.DataAnnotations;
using static F1Fast.API.Helpers.DateTimeHelper;

namespace F1Fast.API.Models;

/// <summary>
/// Registro de auditoria para rastrear ações importantes no sistema.
/// Armazena: quem fez, o quê, quando, com que resultado.
/// </summary>
public class LogAuditoria
{
    public int Id { get; set; }

    /// <summary>Data/hora da ação (horário de Brasília).</summary>
    public DateTime DataHora { get; set; } = AgoraBRT;

    /// <summary>Código da ação (ex: LOGIN_OK, PALPITE_ENVIADO, RESULTADO_LANCADO).</summary>
    [Required, MaxLength(50)]
    public string Acao { get; set; } = "";

    /// <summary>ID do usuário que executou a ação (null para ações não autenticadas).</summary>
    public int? UsuarioId { get; set; }

    /// <summary>Login do usuário (gravado para consulta rápida sem JOIN).</summary>
    [MaxLength(50)]
    public string? UsuarioLogin { get; set; }

    /// <summary>Tipo da entidade afetada (ex: "Palpite", "Etapa", "Usuario").</summary>
    [MaxLength(50)]
    public string? Entidade { get; set; }

    /// <summary>ID do registro afetado.</summary>
    public int? EntidadeId { get; set; }

    /// <summary>Descrição ou dados extras da ação.</summary>
    [MaxLength(500)]
    public string? Detalhes { get; set; }

    /// <summary>true = ação bem-sucedida, false = falha.</summary>
    public bool Sucesso { get; set; } = true;

    /// <summary>Endereço IP do cliente.</summary>
    [MaxLength(45)]
    public string? Ip { get; set; }
}
