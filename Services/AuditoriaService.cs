using F1Fast.API.Data;
using F1Fast.API.Models;
using static F1Fast.API.Helpers.DateTimeHelper;

namespace F1Fast.API.Services;

/// <summary>
/// Service centralizado para registrar logs de auditoria no banco de dados.
/// Injetado nos controllers/services que precisam logar ações.
/// </summary>
public class AuditoriaService(AppDbContext db)
{
    /// <summary>
    /// Registra uma ação no log de auditoria.
    /// </summary>
    /// <param name="acao">Código da ação (ex: LOGIN_OK, PALPITE_ENVIADO)</param>
    /// <param name="usuarioId">ID do usuário (null se não autenticado)</param>
    /// <param name="usuarioLogin">Login do usuário</param>
    /// <param name="entidade">Tipo da entidade afetada (ex: "Palpite")</param>
    /// <param name="entidadeId">ID do registro afetado</param>
    /// <param name="detalhes">Descrição ou dados extras</param>
    /// <param name="sucesso">true = sucesso, false = falha</param>
    /// <param name="ip">IP do cliente (extraído do HttpContext)</param>
    public async Task RegistrarAsync(
        string acao,
        int? usuarioId = null,
        string? usuarioLogin = null,
        string? entidade = null,
        int? entidadeId = null,
        string? detalhes = null,
        bool sucesso = true,
        string? ip = null)
    {
        db.LogsAuditoria.Add(new LogAuditoria
        {
            DataHora     = AgoraBRT,
            Acao         = acao,
            UsuarioId    = usuarioId,
            UsuarioLogin = usuarioLogin,
            Entidade     = entidade,
            EntidadeId   = entidadeId,
            Detalhes     = detalhes,
            Sucesso      = sucesso,
            Ip           = ip
        });

        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Extrai o IP real do cliente a partir do HttpContext.
    /// Considera o header X-Forwarded-For para apps atrás de proxy/load balancer.
    /// </summary>
    public static string? ExtrairIp(HttpContext? ctx)
    {
        if (ctx is null) return null;
        var forwarded = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwarded))
            return forwarded.Split(',')[0].Trim();
        return ctx.Connection.RemoteIpAddress?.ToString();
    }
}
