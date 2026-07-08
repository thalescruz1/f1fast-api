// ============================================================
// MODELO: PushSubscription
// ============================================================
// Guarda a inscrição de Web Push de um dispositivo/navegador.
// Cada usuário pode ter várias (um por navegador/celular).
// O envio de push usa Endpoint + P256dh + Auth (padrão Web Push).
// ============================================================

using System.ComponentModel.DataAnnotations;
using static F1Fast.API.Helpers.DateTimeHelper;

namespace F1Fast.API.Models;

/// <summary>
/// Inscrição de Web Push de um dispositivo. Uma linha por navegador/celular.
/// </summary>
public class PushSubscription
{
    public int Id { get; set; }

    /// <summary>Usuário dono da inscrição (FK → Usuarios).</summary>
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    /// <summary>URL única do push service do navegador (identifica o dispositivo).</summary>
    [Required, MaxLength(500)]
    public string Endpoint { get; set; } = "";

    /// <summary>Chave pública do cliente (criptografia da mensagem).</summary>
    [Required, MaxLength(200)]
    public string P256dh { get; set; } = "";

    /// <summary>Segredo de autenticação do cliente.</summary>
    [Required, MaxLength(100)]
    public string Auth { get; set; } = "";

    /// <summary>User-Agent do navegador (só para diagnóstico).</summary>
    [MaxLength(300)]
    public string? UserAgent { get; set; }

    /// <summary>Data/hora da inscrição (horário de Brasília).</summary>
    public DateTime CriadoEm { get; set; } = AgoraBRT;
}
