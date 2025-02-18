using Microsoft.Extensions.Options;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.FixerApiModels;

namespace RatesProvider.Application.Integrations
{
    public class FixerClient : ICurrencyRateProvider
    {
        private readonly ApiSettings _apiSettings;
        private readonly ICommonHttpClient _commonHttpClient;


        public FixerClient(IOptions<ApiSettings> apiSettings, ICommonHttpClient ratesProviderHttpRequest)
        {
            _apiSettings = apiSettings.Value;
            _commonHttpClient = ratesProviderHttpRequest;
        }
        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
        {
            var url = $"https://data.fixer.io/api/latest?access_key={_apiSettings.FixerApiKey}";
            var response = await _commonHttpClient.SendRequestAsync<FixerResponse>(url);

            var currencyRate = new CurrencyRateResponse
            {
                BaseCurrency = Enum.Parse<Currencies>(response.Base),
                Rates = new Dictionary<string, decimal>(),
                Date = response.Date,

            };

            var allCurrencies = AvailableCurrencies.AvailableCurrencyList;

            foreach (var rate in response.Rates)
            {
                var baseCurrencyEnum = Enum.Parse<Currencies>(response.Base);
                if (Enum.TryParse<Currencies>(rate.Key, out var parsedTargetCurrencyEnum))
                {
                     if (allCurrencies.Contains(parsedTargetCurrencyEnum))
                    {
                        var currencyPair = $"{baseCurrencyEnum}{parsedTargetCurrencyEnum}";
                        currencyRate.Rates[currencyPair] = rate.Value;
                    }
                }

            }
            return currencyRate;
        }
    }
}

