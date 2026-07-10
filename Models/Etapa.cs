// ============================================================
// MODELO: Etapa
// ============================================================
// Representa uma corrida (ou sprint) do calendário F1 2026.
// Cada objeto = uma linha na tabela "Etapas" do banco.
// A temporada tem 30 etapas, inseridas via "Seed" no
// AppDbContext — de março/2026 (Austrália) a dezembro (Abu Dhabi).
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace F1Fast.API.Models;

/// <summary>
/// Representa uma etapa (corrida ou sprint) do calendário F1 2026.
/// </summary>
public class Etapa
{
    // Identificador único no banco
    public int Id { get; set; }

    // Número sequencial da etapa no calendário (1 a 30)
    public int Numero { get; set; }

    // Nome completo da etapa (ex: "GP do Brasil", "Sprint de Miami")
    [Required, MaxLength(100)]
    public string Nome { get; set; } = "";

    // Nome do circuito (ex: "Interlagos", "Silverstone", "Monza")
    [Required, MaxLength(100)]
    public string Circuito { get; set; } = "";

    // Cidade onde a corrida ocorre (ex: "São Paulo", "Monte Carlo")
    [Required, MaxLength(100)]
    public string Cidade { get; set; } = "";

    // Emoji da bandeira do país (ex: "🇧🇷", "🇬🇧", "🇮🇹")
    // Exibido no frontend ao lado do nome da etapa
    [MaxLength(10)]
    public string Pais { get; set; } = "";

    // true = corrida sprint (formato curto, sábado)
    // false = Grande Prêmio normal (domingo) — padrão
    public bool Sprint { get; set; } = false;

    // Data/hora limite para enviar palpites. Após esse prazo, nenhum
    // palpite pode ser enviado ou alterado. Geralmente = início do qualifying.
    public DateTime PrazoQualify { get; set; }

    // Data/hora da largada da corrida. "?" = pode ser null se não confirmada.
    public DateTime? DataCorrida { get; set; }

    // true = etapa finalizada e pontos calculados (bloqueada para alterações).
    // O PontuacaoService marca como true ao processar um resultado.
    public bool Encerrada { get; set; } = false;

    // Dados do circuito — editáveis diretamente no banco
    [MaxLength(50)]
    public string CircuitoTipo { get; set; } = "";        // Ex: "Circuito permanente"

    [MaxLength(20)]
    public string CircuitoComprimento { get; set; } = "";  // Ex: "5.451 km"

    public int Voltas { get; set; }                        // Número de voltas (Sprint ou GP)

    [MaxLength(20)]
    public string Distancia { get; set; } = "";            // Ex: "305.1 km"

    [MaxLength(50)]
    public string Recordista { get; set; } = "";           // Ex: "Charles Leclerc"

    [MaxLength(20)]
    public string TempoRecord { get; set; } = "";          // Ex: "1:19.813"

    public int AnoRecord { get; set; }                     // Ex: 2022

    // Horários das sessões do fim de semana (UTC)
    // Nullable porque Sprint weekends não têm TL3
    public DateTime? TreinoLivre1 { get; set; }
    public DateTime? TreinoLivre2 { get; set; }
    public DateTime? TreinoLivre3 { get; set; }
    public DateTime? Classificacao { get; set; }

    // SVG path data do traçado do circuito (renderizado no frontend)
    public string CircuitoSvg { get; set; } = "";

    // true = etapa cancelada (não aceita palpites, não envia lembretes,
    // não aparece como próxima). Exibida como "Cancelada" no frontend.
    public bool Cancelada { get; set; } = false;

    // true = e-mail de lembrete (quarta-feira) já foi enviado para esta etapa.
    // Evita envios duplicados pelo LembreteBackgroundService.
    public bool LembreteEnviado { get; set; } = false;

    // true = e-mail de lembrete urgente (30 min antes do qualify) já foi enviado.
    // Segundo aviso, apenas para quem ainda não fez o palpite.
    public bool LembreteUrgenteEnviado { get; set; } = false;

    // true = e-mail/push "Semana de F1!" (segunda-feira) já foi enviado para esta etapa.
    // Evita reenvio pelo LembreteBackgroundService.
    public bool SemanaEnviada { get; set; } = false;

    // Todos os palpites enviados para esta etapa (propriedade de navegação)
    public ICollection<Palpite> Palpites { get; set; } = [];

    // Resultado oficial desta etapa. Null antes do admin lançar o resultado.
    public Resultado? Resultado { get; set; }
}
