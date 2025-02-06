using Microsoft.Extensions.Logging;
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
        private readonly ILogger<FixerClient> _logger;




        public FixerClient(IOptions<ApiSettings> apiSettings, ICommonHttpClient ratesProviderHttpRequest, ILogger<FixerClient> logger)
        {
            _apiSettings = apiSettings.Value;
            _commonHttpClient = ratesProviderHttpRequest;
            _logger = logger;
        }
        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
        {
            var url = $"https://data.fixer.io/api/latest?access_key={_apiSettings.FixerApiKey}";
            try
            {
                _logger.LogInformation("Sending request to Fixer API: {Url}", url.ToString()); //тут логируем запрос 
                var response = await _commonHttpClient.SendRequestAsync<FixerResponse>(url.ToString());

                _logger.LogInformation("Received response from Fixer API: {StatusCode}", response?.Base);


                var currencyRate = new CurrencyRateResponse
                {
                    BaseCurrency = Enum.Parse<Currences>(response.Base),
                    Rates = response.Rates,
                    Date = response.Date,

                };
                return currencyRate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching currency rates from Fixer API.");
                throw;
            }

        }
    }
}
