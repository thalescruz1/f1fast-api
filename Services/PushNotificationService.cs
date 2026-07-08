// ============================================================
// SERVICE: PushNotificationService
// ============================================================
// Envia Web Push (notificação nativa do navegador/PWA) usando
// VAPID. As inscrições ficam na tabela PushSubscriptions.
//
// - SalvarInscricaoAsync/RemoverInscricaoAsync: chamados pelo PushController
// - EnviarParaUsuariosAsync: envio genérico (remove inscrições mortas 410/404)
// - EnviarResultadoCadastradoAsync: usado pelo ResultadoController
//
// Os lembretes chamam EnviarParaUsuariosAsync a partir do NotificacaoService.
// Chaves VAPID vêm de appsettings (seção "WebPush").
// ============================================================

using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using F1Fast.API.Data;
using F1Fast.API.Models;
using WebPushClient = WebPush.WebPushClient;
using WebPushVapidDetails = WebPush.VapidDetails;
using WebPushException = WebPush.WebPushException;
using LibPushSubscription = WebPush.PushSubscription;

namespace F1Fast.API.Services;

public class PushNotificationService
{
    private readonly AppDbContext db;
    private readonly ILogger<PushNotificationService> logger;
    private readonly WebPushClient client = new();
    private readonly WebPushVapidDetails? vapid;

    public PushNotificationService(AppDbContext db, IConfiguration config, ILogger<PushNotificationService> logger)
    {
        this.db = db;
        this.logger = logger;

        var subject = config["WebPush:Subject"];
        var pub     = config["WebPush:PublicKey"];
        var priv    = config["WebPush:PrivateKey"];

        // Só habilita push se as chaves estiverem configuradas
        if (!string.IsNullOrWhiteSpace(subject) && !string.IsNullOrWhiteSpace(pub) && !string.IsNullOrWhiteSpace(priv))
            vapid = new WebPushVapidDetails(subject, pub, priv);
    }

    /// <summary>Conteúdo de uma notificação push (lido pelo service worker em sw.js).</summary>
    public record PushPayload(string Title, string Body, string? Url = null, string? Tag = null);

    /// <summary>Cria ou atualiza a inscrição de um dispositivo (upsert por Endpoint).</summary>
    public async Task SalvarInscricaoAsync(int usuarioId, string endpoint, string p256dh, string auth, string? userAgent)
    {
        var existente = await db.PushSubscriptions.FirstOrDefaultAsync(s => s.Endpoint == endpoint);

        if (existente is null)
        {
            db.PushSubscriptions.Add(new PushSubscription
            {
                UsuarioId = usuarioId,
                Endpoint  = endpoint,
                P256dh    = p256dh,
                Auth      = auth,
                UserAgent = userAgent
            });
        }
        else
        {
            // Mesmo endpoint pode trocar de dono (outro login no mesmo navegador) ou renovar chaves
            existente.UsuarioId = usuarioId;
            existente.P256dh    = p256dh;
            existente.Auth      = auth;
            existente.UserAgent = userAgent;
        }

        await db.SaveChangesAsync();
    }

    /// <summary>Remove a inscrição de um dispositivo pelo endpoint.</summary>
    public async Task RemoverInscricaoAsync(string endpoint)
    {
        var sub = await db.PushSubscriptions.FirstOrDefaultAsync(s => s.Endpoint == endpoint);
        if (sub is not null)
        {
            db.PushSubscriptions.Remove(sub);
            await db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Envia um push para todos os dispositivos dos usuários informados.
    /// Inscrições expiradas (410 Gone / 404) são removidas automaticamente.
    /// Tolerante a falha: um erro em um dispositivo não interrompe os demais.
    /// </summary>
    public async Task EnviarParaUsuariosAsync(IEnumerable<int> usuarioIds, PushPayload payload)
    {
        if (vapid is null)
        {
            logger.LogWarning("VAPID não configurado (seção WebPush); push ignorado.");
            return;
        }

        var ids = usuarioIds.Distinct().ToList();
        if (ids.Count == 0) return;

        var subs = await db.PushSubscriptions.Where(s => ids.Contains(s.UsuarioId)).ToListAsync();
        if (subs.Count == 0) return;

        var json = JsonSerializer.Serialize(new
        {
            title = payload.Title,
            body  = payload.Body,
            url   = payload.Url ?? "/",
            tag   = payload.Tag
        });

        var mortas = new List<PushSubscription>();

        foreach (var s in subs)
        {
            try
            {
                await client.SendNotificationAsync(
                    new LibPushSubscription(s.Endpoint, s.P256dh, s.Auth), json, vapid, default);
            }
            catch (WebPushException ex) when (ex.StatusCode is HttpStatusCode.Gone or HttpStatusCode.NotFound)
            {
                mortas.Add(s); // inscrição expirada/cancelada no push service
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao enviar push para {Endpoint}", s.Endpoint);
            }
        }

        if (mortas.Count > 0)
        {
            db.PushSubscriptions.RemoveRange(mortas);
            await db.SaveChangesAsync();
        }
    }

    /// <summary>Notifica todos os usuários que o resultado de uma etapa foi lançado/retificado.</summary>
    public async Task EnviarResultadoCadastradoAsync(int etapaId, bool retificado)
    {
        var etapa = await db.Etapas.FindAsync(etapaId);
        if (etapa is null) return;

        var titulo = retificado ? "🏁 Resultado retificado" : "🏁 Resultado no ar!";
        var corpo  = retificado
            ? $"O resultado do {etapa.Nome} foi atualizado. Confira sua nova pontuação!"
            : $"O resultado do {etapa.Nome} já está disponível. Veja como você foi!";

        var ids = await db.Usuarios.Select(u => u.Id).ToListAsync();
        await EnviarParaUsuariosAsync(ids, new PushPayload(titulo, corpo, "/ranking", $"resultado-{etapaId}"));
    }
}
