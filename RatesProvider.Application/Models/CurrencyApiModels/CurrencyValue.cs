using System.Text.Json.Serialization;

namespace RatesProvider.Application.Models.CurrencyApiModels;

public class CurrencyValue
{
    [JsonPropertyName("Currency")]
    public string Currency { get; set; }

    [JsonPropertyName("Value")]
    public decimal Value { get; set; }
}
