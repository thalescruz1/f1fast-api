namespace F1Fast.API.Helpers;

/// <summary>
/// Helper centralizado para obter o horário de Brasília (UTC-3).
/// Todas as datas no banco são armazenadas em BRT, então as comparações
/// de prazo e timestamps devem usar este helper.
/// </summary>
public static class DateTimeHelper
{
    private static readonly TimeZoneInfo BrasiliaZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    /// <summary>
    /// Retorna DateTime.Now no fuso de Brasília (UTC-3).
    /// </summary>
    public static DateTime AgoraBRT =>
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BrasiliaZone);
}
