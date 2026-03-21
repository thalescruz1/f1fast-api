using System.Text.Json;
using System.Text.Json.Serialization;

namespace F1Fast.API.Converters;

/// <summary>
/// Converte DateTime para sempre serializar com sufixo "Z" (UTC).
/// O MySQL retorna DateTime com Kind=Unspecified, o que faz o System.Text.Json
/// serializar sem "Z". O frontend então interpreta como horário local, causando
/// diferença de fuso. Este converter força Kind=Utc na serialização.
/// </summary>
public class UtcDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dt = reader.GetDateTime();
        return dt.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dt, DateTimeKind.Utc)
            : dt.ToUniversalTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Força Kind=Utc para que o JSON sempre inclua "Z"
        var utc = value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value.ToUniversalTime();

        writer.WriteStringValue(utc.ToString("yyyy-MM-ddTHH:mm:ssZ"));
    }
}
