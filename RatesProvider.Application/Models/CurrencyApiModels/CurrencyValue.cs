using System.Text.Json.Serialization;

namespace RatesProvider.Application.Models.CurrencyApiModels
{
    public class CurrencyValue
    {

        [JsonPropertyName("code")]
        public string Currency { get; set; }

        [JsonPropertyName("value")]
        public decimal Value { get; set; }

    }
}
