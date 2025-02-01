using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.FixerModels;
using System.Text.Json;

namespace RatesProvider.Application.Integrations
{
    public class FixerClient : ICurrencyRateProvider
    {
        private HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly string _apiKey = "d8997419331d2484d18e9fa0b9dede91";

        public FixerClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://data.fixer.io/api/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
        {
            using var response = await _client.GetAsync(_apiKey, HttpCompletionOption.ResponseHeadersRead);
            
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                var currency = await JsonSerializer.DeserializeAsync<FixerResponseModel>(stream, _options);
            
            var currencyRateResponse = new CurrencyRateResponse
            {
                BaseCurrency = currency.Base,
                Rates = currency.Rates,
                Date = currency.Date

            };
            return currencyRateResponse;
            
        }
    }
}
