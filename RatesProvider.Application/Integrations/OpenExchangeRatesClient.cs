using Polly.Retry;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.OpenExchangeRatesModels;
using System.Text.Json;

namespace RatesProvider.Application.Integrations
{
    public class OpenExchangeRatesClient : ICurrencyRateProvider
    {
        private HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly string _apiKey = "latest.json?app_id=0fecdffcab43483b9df15a4ef3bd99d9";

        public OpenExchangeRatesClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://openexchangerates.org/api/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
        {
            using var response = await _client.GetAsync(_apiKey, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var currency = await JsonSerializer.DeserializeAsync<OpenExchangeRatesResponseModel>(stream, _options);
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
