using System.Text.Json.Serialization;

namespace RatesProvider.Application.Models.CurrencyApiModels;

public class Data
{
    [JsonPropertyName("RUB")]
    public CurrencyValue RUB { get; set; }

    [JsonPropertyName("USD")]
    public CurrencyValue USD { get; set; }

    [JsonPropertyName("EUR")]
    public CurrencyValue EUR { get; set; }

    [JsonPropertyName("JPY")]
    public CurrencyValue JPY { get; set; }

    [JsonPropertyName("CNY")]
    public CurrencyValue CNY { get; set; }

    [JsonPropertyName("RSD")]
    public CurrencyValue RSD { get; set; }

    [JsonPropertyName("BGN")]
    public CurrencyValue BGN { get; set; }

    [JsonPropertyName("ARS")]
    public CurrencyValue ARS { get; set; }
}
