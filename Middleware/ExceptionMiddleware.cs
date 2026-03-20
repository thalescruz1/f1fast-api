// ============================================================
// MIDDLEWARE: ExceptionMiddleware
// ============================================================
// Intercepta QUALQUER exceção não tratada que ocorra durante
// o processamento de uma requisição HTTP.
//
// Sem este middleware, exceções não tratadas resultam em:
//   - Stack trace exposto ao cliente (risco de segurança)
//   - Formato de erro inconsistente (dificulta o frontend)
//
// Com ele, TODA exceção vira uma resposta padronizada:
//   { "status": 500, "mensagem": "Ocorreu um erro interno..." }
//
// Exceções de banco (DbUpdateException) recebem tratamento
// especial para traduzir erros de constraint (ex: duplicata).
// ============================================================

using System.Text.Json;
using F1Fast.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace F1Fast.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    // Opções de serialização: camelCase para manter padrão JSON do frontend
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Passa a requisição adiante no pipeline
            await next(context);
        }
        catch (Exception ex)
        {
            // Loga a exceção completa (stack trace) para debug no servidor
            logger.LogError(ex, "Erro não tratado em {Method} {Path}",
                context.Request.Method, context.Request.Path);

            // Determina o status HTTP e a mensagem amigável
            var (status, mensagem) = ClassificarExcecao(ex);

            // Monta e envia a resposta padronizada
            context.Response.StatusCode  = status;
            context.Response.ContentType = "application/json";

            var erro = new ApiError(status, mensagem);
            await context.Response.WriteAsync(JsonSerializer.Serialize(erro, JsonOpts));
        }
    }

    /// <summary>
    /// Classifica a exceção em um status HTTP + mensagem amigável.
    /// Exceções conhecidas recebem mensagens traduzidas;
    /// exceções genéricas recebem mensagem genérica (sem expor detalhes internos).
    /// </summary>
    private static (int Status, string Mensagem) ClassificarExcecao(Exception ex) => ex switch
    {
        // Erros de banco de dados (constraint violation, conexão, etc.)
        DbUpdateException dbEx when dbEx.InnerException?.Message.Contains("Duplicate") == true
            => (409, "Registro duplicado. Verifique os dados e tente novamente."),

        DbUpdateException
            => (500, "Erro ao salvar os dados. Tente novamente em instantes."),

        // Timeout de operação
        OperationCanceledException
            => (408, "A operação demorou demais e foi cancelada. Tente novamente."),

        // Argumento inválido lançado por validação de negócio
        ArgumentException argEx
            => (400, argEx.Message),

        // Qualquer outra exceção → erro genérico (não expõe detalhes internos)
        _ => (500, "Ocorreu um erro interno. Tente novamente em instantes.")
    };
}
