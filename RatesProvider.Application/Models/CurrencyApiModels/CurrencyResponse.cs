using System.Text.Json.Serialization;

namespace RatesProvider.Application.Models.CurrencyApiModels;

public class CurrencyResponse
{
    [JsonPropertyName("meta")]
    public Meta Meta { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }
}
