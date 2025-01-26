using RatesProvider.Application.Models;
using RatesProvider.Application.Models.FixerApiModels;
using System.Text.Json;

namespace RatesProvider.Application.Integrations
{
    public class FixerClient : ICurrencyRateProvider
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly string _apiKey = "d8997419331d2484d18e9fa0b9dede91";

        public FixerClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://data.fixer.io/api/");
            _client.Timeout = TimeSpan.FromSeconds(30);
            _client.DefaultRequestHeaders.Clear();

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
        {
            var url = $"latest?access_key={_apiKey}";
            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var exchangeRateResponse  = await JsonSerializer.DeserializeAsync<ExchangeRateResponse>(stream, _options);
            var currencyRate = new CurrencyRateResponse
            {
                BaseCurrency = Enum.Parse<Currences>(exchangeRateResponse.Base),
                Rates = exchangeRateResponse.Rates,
                Date = exchangeRateResponse.Date,


            };

            return currencyRate;

        }
    }
}
