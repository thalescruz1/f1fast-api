// ============================================================
// DTOs: PushDTOs
// ============================================================
// Dados que o frontend envia ao (des)inscrever um dispositivo
// para receber Web Push. O Endpoint identifica o dispositivo;
// P256dh e Auth são as chaves de criptografia do padrão Web Push.
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.DTOs;

/// <summary>Inscrição de push enviada pelo navegador (PushSubscription.toJSON achatado).</summary>
public record PushSubscribeRequest(
    [Required] string Endpoint,
    [Required] string P256dh,
    [Required] string Auth
);

/// <summary>Cancelamento de inscrição pelo endpoint do dispositivo.</summary>
public record PushUnsubscribeRequest(
    [Required] string Endpoint
);
