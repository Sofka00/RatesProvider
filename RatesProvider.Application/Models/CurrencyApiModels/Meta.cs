using System.Text.Json.Serialization;

namespace RatesProvider.Application.Models.CurrencyApiModels
{
    public class Meta
    {
        [JsonPropertyName("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }
    }

}
