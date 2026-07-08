// ============================================================
// CONTROLLER: PushController
// ============================================================
// Inscrição/cancelamento de Web Push do dispositivo do usuário.
// [Authorize] = requer login: a inscrição é associada ao usuário
// do token (claim NameIdentifier). O front não envia o UsuarioId.
//
//   POST /api/push/subscribe    → registra o dispositivo atual
//   POST /api/push/unsubscribe  → remove o dispositivo atual
//   GET  /api/push/public-key   → chave pública VAPID (conveniência)
// ============================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using F1Fast.API.DTOs;
using F1Fast.API.Services;

namespace F1Fast.API.Controllers;

[ApiController, Route("api/push"), Authorize]
public class PushController(PushNotificationService push, IConfiguration config) : ApiControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // POST /api/push/subscribe → registra (ou atualiza) a inscrição deste dispositivo
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe(PushSubscribeRequest req)
    {
        var userAgent = Request.Headers.UserAgent.ToString();
        await push.SalvarInscricaoAsync(UserId, req.Endpoint, req.P256dh, req.Auth,
            string.IsNullOrWhiteSpace(userAgent) ? null : userAgent);
        return Ok("Inscrição registrada.");
    }

    // POST /api/push/unsubscribe → remove a inscrição deste dispositivo
    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe(PushUnsubscribeRequest req)
    {
        await push.RemoverInscricaoAsync(req.Endpoint);
        return Ok("Inscrição removida.");
    }

    // GET /api/push/public-key → chave pública VAPID (o front também a tem no environment)
    [HttpGet("public-key"), AllowAnonymous]
    public IActionResult PublicKey() =>
        Ok(new { publicKey = config["WebPush:PublicKey"] ?? "" });
}
