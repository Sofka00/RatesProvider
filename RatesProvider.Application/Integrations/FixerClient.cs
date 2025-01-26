using Microsoft.Extensions.Options;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.FixerApiModels;
using System.Net;
using System.Text.Json;

namespace RatesProvider.Application.Integrations
{
    public class FixerClient : ICurrencyRateProvider
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly ApiSettings _apiSettings;


        public FixerClient(HttpClient client, IOptions<ApiSettings> apiSettings)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://data.fixer.io/api/");
            _client.Timeout = TimeSpan.FromSeconds(30);
            _client.DefaultRequestHeaders.Clear();
            _apiSettings = apiSettings.Value;

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
        {
            var url = $"latest?access_key={_apiSettings.FixerApiKey}";
            using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var exchangeRateResponse  = await JsonSerializer.DeserializeAsync<ExchangeRateResponse>(stream, _options);
            var currencyRate = new CurrencyRateResponse
            {
                BaseCurrency = Enum.Parse<Currences>(exchangeRateResponse.Base),
                Rates = exchangeRateResponse.Rates,
                Date = exchangeRateResponse.Date,


            };

            if (exchangeRateResponse != null)
            {



                return currencyRate;

            }
            return null;

        }
    }
}
