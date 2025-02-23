using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
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

    public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
    {
        var url = $"{_openExchangeRatesSettings.BaseUrl}{_openExchangeRatesSettings.QueryOption}{_openExchangeRatesSettings.ApiKey}";
        try
        {
            var response = await _commonHttpClient.SendRequestAsync<OpenExchangeRatesResponse>(url);
            _logger.LogDebug("Response content from OpenExchangeRates API: {ResponseContent}", response);

            _logger.LogDebug("Response content from Fixer API: {ResponseContent}", response);
            var currencyRate = new CurrencyRateResponse
            {
                BaseCurrency = Enum.Parse<Currency>(response.Base),
                Rates = response.Rates,
                Date = response.Date,
            };
            _logger.LogDebug("Parsed currency rate response: {CurrencyRate}", currencyRate);

            return currencyRate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching currency rates from OpenExchangeRates API.");
            throw;
        }
    }

    private CurrencyRateResponse ConvertOpenExcangeRatesToCurrencyRates(OpenExchangeRatesResponse response)
    {
        var currencyRateResponse = new CurrencyRateResponse
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
                currencyRateResponse.Rates[currencyPair] = rate.Value;
                _logger.LogDebug("Added currency pair: {CurrencyPair}, exchange rate: {Rate}", currencyPair, rate.Value);
            }
            else
            {
                _logger.LogError("Could not parse key '{rate.Key}' into a valid Currency enum value.", rate.Key);
            }
        }

        return currencyRateResponse;
    }
}
