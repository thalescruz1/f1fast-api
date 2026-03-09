// ============================================================
// BACKGROUND SERVICE: LembreteBackgroundService
// ============================================================
// Um "Background Service" roda em segundo plano continuamente
// enquanto a aplicação está no ar — como um "daemon" ou serviço.
//
// Esta classe herda de "BackgroundService", que é a classe base
// do .NET para tarefas que rodam em loop na aplicação.
//
// Funcionamento:
//   A cada 30 minutos, chama o NotificacaoService para verificar
//   se alguma corrida tem prazo próximo e enviar e-mails de aviso.
// ============================================================

using F1Fast.API.Services;

namespace F1Fast.API.Background;

// IServiceScopeFactory = fábrica de "escopos" de serviços.
// Necessário porque BackgroundService é "singleton" (uma instância só),
// mas NotificacaoService é "scoped" (nova instância por requisição).
// Precisamos criar um scope manualmente para usar o NotificacaoService aqui.
public class LembreteBackgroundService(
    IServiceScopeFactory factory,
    ILogger<LembreteBackgroundService> logger) : BackgroundService
{
    /// <summary>
    /// Método principal do background service — executado automaticamente pelo .NET.
    /// Roda em loop infinito até a aplicação ser encerrada.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // CancellationToken.IsCancellationRequested = true quando a app está sendo encerrada
        // O loop para quando a aplicação for desligada
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Cria um escopo temporário para resolver o NotificacaoService
                // "using" garante que o escopo seja descartado ao sair do bloco
                using var scope = factory.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<NotificacaoService>();

                // Executa a verificação de lembretes
                await svc.EnviarLembretesAsync();
            }
            catch (Exception ex)
            {
                // Loga o erro mas NÃO derruba o loop — o serviço continua rodando
                logger.LogError(ex, "Erro no LembreteBackgroundService");
            }

            // Aguarda 30 minutos antes de verificar novamente
            // "stoppingToken" permite cancelar o delay quando a app for encerrada
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
