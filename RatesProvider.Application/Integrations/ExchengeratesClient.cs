using RatesProvider.Application.Models;
using RatesProvider.Application.Models.FixerApiModels;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RatesProvider.Application.Integrations
{
    public class ExchengeratesClient : ICurrencyRateProvider
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly string _apiKey = "5b770aba259d9fa84d4eae92140ada35";


        public ExchengeratesClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://api.exchangeratesapi.io/v1/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
        {
            var url = $"latest?access_key={_apiKey}";
            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var currencyExchenge = await JsonSerializer.DeserializeAsync<ExchangeRateResponse>(stream, _options);
            var currencyRate = new CurrencyRateResponse
            {
                BaseCurrency = Enum.Parse<Currences>(currencyExchenge.Base),
                Rates = currencyExchenge.Rates,
                Date = currencyExchenge.Date
            };


            return currencyRate;

        }
    }
}
