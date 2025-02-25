using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MYPBackendMicroserviceIntegrations.Enums;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.CurrencyApiModels;

namespace RatesProvider.Application.Integrations;

public class CurrencyApiClient : ICurrencyRateProvider
{
    private readonly CurrencyApiClientSettings _currencyApiSettings;
    private readonly ICommonHttpClient _commonHttpClient;
    private readonly ILogger<CurrencyApiClient> _logger;

    public CurrencyApiClient(
        IOptions<CurrencyApiClientSettings> currencyApiSettings,
        ICommonHttpClient ratesProviderHttpRequest,
        ILogger<CurrencyApiClient> logger)
    {
        _currencyApiSettings = currencyApiSettings.Value;
        _commonHttpClient = ratesProviderHttpRequest;
        _logger = logger;
    }

    public async Task<CurrencyRateMessage> GetCurrencyRatesAsync()
    {
        var url = $"{_currencyApiSettings.BaseUrl}{_currencyApiSettings.QueryOption}{_currencyApiSettings.ApiKey}";

        try
        {
            var response = await _commonHttpClient.SendRequestAsync<CurrencyResponse>(url);
            _logger.LogDebug("Response content from CurrencyApiClient API: {ResponseContent}", response);

            var currencyRate = ConvertCurrencyApiToCurrencyRates(response);
            _logger.LogDebug("Parsed currency rate response: {CurrencyRate}", currencyRate);

            return currencyRate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching currency rates from CurrencyApiClient API.");
            throw;
        }
    }

    private CurrencyRateMessage ConvertCurrencyApiToCurrencyRates(CurrencyResponse response)
    {
        var baseCurrency = Currency.USD;
        var date = response.Meta?.LastUpdatedAt ?? DateTime.Now;
        var rates = new Dictionary<string, decimal>();

        rates.Add(baseCurrency.ToString() + baseCurrency.ToString(), 1);

        if (response.Data.RUB != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.RUB.Code, response.Data.RUB.Value);
            _logger.LogDebug("Added currency RUB with value: {Value}", response.Data.RUB.Value);
        }

        if (response.Data.EUR != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.EUR.Code, response.Data.EUR.Value);
            _logger.LogDebug("Added currency EUR with value: {Value}", response.Data.EUR.Value);
        }

        if (response.Data.JPY != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.JPY.Code, response.Data.JPY.Value);
            _logger.LogDebug("Added currency JPY with value: {Value}", response.Data.JPY.Value);
        }

        if (response.Data.CNY != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.CNY.Code, response.Data.CNY.Value);
            _logger.LogDebug("Added currency CNY with value: {Value}", response.Data.CNY.Value);
        }

        if (response.Data.RSD != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.RSD.Code, response.Data.RSD.Value);
            _logger.LogDebug("Added currency RSD with value: {Value}", response.Data.RSD.Value);
        }

        if (response.Data.BGN != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.BGN.Code, response.Data.BGN.Value);
            _logger.LogDebug("Added currency BGN with value: {Value}", response.Data.BGN.Value);
        }

        if (response.Data.ARS != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.ARS.Code, response.Data.ARS.Value);
            _logger.LogDebug("Added currency ARS with value: {Value}", response.Data.ARS.Value);
        }

        return new CurrencyRateMessage
        {
            BaseCurrency = baseCurrency,
            Rates = rates,
            Date = date
        };
    }
}