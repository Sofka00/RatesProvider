using RatesProvider.Application.Models.CurrencyApiModels;
using System.Text.Json;

namespace RatesProvider.Application.Integrations
{
    public class CurrencyLayerClient
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public CurrencyLayerClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://currencylayer.com/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<CurrencyResponse>> GetCurrencyRatesAsync()
        {
            using (var response = await _client.GetAsync("CurrencyLayerRates", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                var currency = await JsonSerializer.DeserializeAsync<List<CurrencyResponse>>(stream, _options);
                return currency;
            }
        }
    }
}
