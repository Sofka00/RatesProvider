using System.Text.Json.Serialization;

namespace RatesProvider.Application.Models.CurrencyApiModels;

public class CurrencyValue
{
    [JsonPropertyName("Code")]
    public string Code { get; set; }

    [JsonPropertyName("Value")]
    public decimal Value { get; set; }
}
