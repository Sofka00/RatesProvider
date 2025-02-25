using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MYPBackendMicroserviceIntegrations.Enums;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.FixerApiModels;
using System.Data;

namespace RatesProvider.Application.Integrations
{
    public class FixerClient : ICurrencyRateProvider
    {
        private readonly FixerClientSettings _fixerClienSettings;
        private readonly ICommonHttpClient _commonHttpClient;
        private readonly ILogger<FixerClient> _logger;

        public FixerClient(IOptions<FixerClientSettings> fixerClienSettings, ICommonHttpClient ratesProviderHttpRequest, ILogger<FixerClient> logger)
        {
            _fixerClienSettings = fixerClienSettings.Value;
            _commonHttpClient = ratesProviderHttpRequest;
            _logger = logger;
        }
        public async Task<CurrencyRateMessage> GetCurrencyRatesAsync()
        {
            var url = $"{_fixerClienSettings.BaseUrl}{_fixerClienSettings.QueryOption}{_fixerClienSettings.ApiKey}";
            try
            {
                var response = await _commonHttpClient.SendRequestAsync<FixerResponse>(url.ToString());

                if (response == null)
                {
                    throw new Exception("No response received from Fixer API.");
                }

                _logger.LogDebug("Response content from Fixer API: {ResponseContent}", response);

                var currencyRate = new CurrencyRateMessage
                {
                    BaseCurrency = Enum.Parse<Currency>(response.Base),
                    Rates = new Dictionary<string, decimal>(),
                    Date = response.Date,
                };

                _logger.LogDebug("Parsed currency rate response: {CurrencyRate}", currencyRate);

                var allCurrencies = AvailableCurrencies.AvailableCurrencyList;

                foreach (var rate in response.Rates)
                {
                    var baseCurrencyEnum = Enum.Parse<Currency>(response.Base);
                    if (Enum.TryParse<Currency>(rate.Key, out var parsedTargetCurrencyEnum))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching currency rates from Fixer API.");
                throw ex;
            }
        }
    }

}


