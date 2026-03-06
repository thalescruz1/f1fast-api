using F1Fast.API.Services;

namespace F1Fast.API.Background;

public class LembreteBackgroundService(
    IServiceScopeFactory factory,
    ILogger<LembreteBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = factory.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<NotificacaoService>();
                await svc.EnviarLembretesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro no LembreteBackgroundService");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
