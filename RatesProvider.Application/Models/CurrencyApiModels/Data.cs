using System.Text.Json.Serialization;

namespace RatesProvider.Application.Models.CurrencyApiModels
{
    public class Data
    {
        [JsonPropertyName("EUR")]
        public CurrencyValue EUR { get; set; }

        [JsonPropertyName("USD")]
        public CurrencyValue USD { get; set; }

        [JsonPropertyName("CAD")]
        public CurrencyValue CAD { get; set; }

        [JsonPropertyName("BND")]
        public CurrencyValue Bnd { get; set; }

        [JsonPropertyName("BOB")]
        public CurrencyValue Bob { get; set; }

    }
}
