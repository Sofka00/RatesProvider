using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.OpenExchangeRatesModels;

namespace RatesProvider.Application.Integrations;

public class OpenExchangeRatesClient : ICurrencyRateProvider
{
    private readonly ApiSettings _appSettings;
    private readonly ICommonHttpClient _commonHttpClient;
    private readonly ILogger<OpenExchangeRatesClient> _logger;

    public OpenExchangeRatesClient(IOptions<ApiSettings> appSettings, ICommonHttpClient ratesProviderHttpRequest)
    {
        _appSettings = appSettings.Value;
        _commonHttpClient = ratesProviderHttpRequest;
    }

    public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
    {
        var url = $"https://openexchangerates.org/api/latest.json?app_id={_appSettings.OpenExchangeRatesApiKey}";
        try
        {
            _logger.LogInformation("Sending request to  OpenExchangeRates API: {Url}", url.ToString());

            _logger.LogDebug("Request URL to  OpenExchangeRates API: {Url}", url);
            var response = await _commonHttpClient.SendRequestAsync<OpenExchangeRatesResponse>(url);

            _logger.LogDebug("Response content from Fixer API: {ResponseContent}", response);
            var currencyRate = new CurrencyRateResponse
            {
                BaseCurrency = Enum.Parse<Currences>(response.Base),
                Rates = response.Rates,
                Date = response.Date,
            };
            _logger.LogDebug("Parsed currency rate response: {CurrencyRate}", currencyRate);
            return currencyRate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching currency rates from Fixer API.");
            throw;
        }
    }
}
