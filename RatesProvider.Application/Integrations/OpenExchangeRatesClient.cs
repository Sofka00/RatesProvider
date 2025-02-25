using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MYPBackendMicroserviceIntegrations.Enums;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models.OpenExchangeRatesModels;

namespace RatesProvider.Application.Integrations;

public class OpenExchangeRatesClient : ICurrencyRateProvider
{
    private readonly OpenExchangeRatesClientSettings _openExchangeRatesSettings;
    private readonly ICommonHttpClient _commonHttpClient;
    private readonly ILogger<OpenExchangeRatesClient> _logger;

    public OpenExchangeRatesClient(
        IOptions<OpenExchangeRatesClientSettings> openExchangeSettings,
        ICommonHttpClient ratesProviderHttpRequest,
        ILogger<OpenExchangeRatesClient> logger)
    {
        _openExchangeRatesSettings = openExchangeSettings.Value;
        _commonHttpClient = ratesProviderHttpRequest;
        _logger = logger;
    }

    public async Task<CurrencyRateMessage> GetCurrencyRatesAsync()
    {
        var url = $"{_openExchangeRatesSettings.BaseUrl}{_openExchangeRatesSettings.QueryOption}{_openExchangeRatesSettings.ApiKey}";
        try
        {
            var response = await _commonHttpClient.SendRequestAsync<OpenExchangeRatesResponse>(url);
            _logger.LogDebug("Response content from OpenExchangeRates API: {ResponseContent}", response);

            var currencyRate = ConvertOpenExcangeRatesToCurrencyRates(response);
            _logger.LogDebug("Parsed currency rate response: {CurrencyRate}", currencyRate);

            return currencyRate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching currency rates from OpenExchangeRates API.");
            throw;
        }
    }

    private CurrencyRateMessage ConvertOpenExcangeRatesToCurrencyRates(OpenExchangeRatesResponse response)
    {
        if (response == null)
        {
            throw new ArgumentNullException(nameof(response), "Response cannot be null.");
        }

        if (response.Rates == null || !response.Rates.Any())
        {
            throw new InvalidOperationException("No currency rates available in the response.");
        }

        var currencyRateMessage = new CurrencyRateMessage
        {
            BaseCurrency = Currency.USD,
            Rates = new Dictionary<string, decimal>(),
            Date = response.Date
        };

        foreach (var rate in response.Rates)
        {
            _logger.LogDebug("Processed currency: {CurrencyKey}, exchange rate: {CurrencyValue}", rate.Key, rate.Value);
            if (Enum.TryParse<Currency>(rate.Key, out var currency))
            {
                string currencyPair = $"{response.Base}{rate.Key}";
                currencyRateMessage.Rates[currencyPair] = rate.Value;
                _logger.LogDebug("Added currency pair: {CurrencyPair}, exchange rate: {Rate}", currencyPair, rate.Value);
            }
            else
            {
                _logger.LogDebug("Failed to fetch currency rates");
            }
        }

        _logger.LogDebug("Parsed currency rate response: {CurrencyRate}", currencyRateMessage);
        return currencyRateMessage;
    }
}
