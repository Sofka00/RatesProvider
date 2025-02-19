using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.OpenExchangeRatesModels;

namespace RatesProvider.Application.Integrations;

public class OpenExchangeRatesClient : ICurrencyRateProvider
{
    private readonly OpenExchangeClientSettings _openExchangeSettings;
    private readonly ICommonHttpClient _commonHttpClient;
    private readonly ILogger<OpenExchangeRatesClient> _logger;

    public OpenExchangeRatesClient(IOptions<OpenExchangeClientSettings> openExchangeSettings, ICommonHttpClient ratesProviderHttpRequest, ILogger<OpenExchangeRatesClient> logger)
    {
        _openExchangeSettings = openExchangeSettings.Value;
        _commonHttpClient = ratesProviderHttpRequest;
        _logger = logger;
    }

    public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
    {
        var url = $"https://openexchangerates.org/api/latest.json?app_id={_openExchangeSettings.ApiKey}";
        try
        {
            var response = await _commonHttpClient.SendRequestAsync<OpenExchangeRatesResponse>(url);

            _logger.LogDebug("Response content from OpenExchangeRates API: {ResponseContent}", response);
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
}
