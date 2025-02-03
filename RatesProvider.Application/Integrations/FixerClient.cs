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
                BaseCurrency = Enum.Parse<Currences>(response.Base),
                Rates = response.Rates,
                Date = response.Date,

            };
            return currencyRate;

        }
    }
}
