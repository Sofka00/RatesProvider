using Microsoft.Extensions.Options;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.CurrencyApiModels;

namespace RatesProvider.Application.Integrations;

public class CurrencyApiClient : ICurrencyRateProvider
{
    private readonly CurrencyApiClientSettings _currencyApiSettings;
    private readonly ICommonHttpClient _commonHttpClient;

    public CurrencyApiClient(IOptions<CurrencyApiClientSettings> currencyApiSettings, ICommonHttpClient ratesProviderHttpRequest)
    {
        _currencyApiSettings = currencyApiSettings.Value;
        _commonHttpClient = ratesProviderHttpRequest;
    }

    public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
    {
        var url = $"{_currencyApiSettings.BaseUrl}{_currencyApiSettings.QueryOption}{_currencyApiSettings.ApiKey}";
        var response = await _commonHttpClient.SendRequestAsync<CurrencyResponse>(url);
        var currencyRate = Convert(response);

        return currencyRate;
    }

    private CurrencyRateResponse Convert(CurrencyResponse response)
    {
        var baseCurrency = Currency.USD;
        var date = response.Meta?.LastUpdatedAt ?? DateTime.Now;
        var rates = new Dictionary<string, decimal>();

        rates.Add(baseCurrency.ToString() + baseCurrency.ToString(), 1);

        if (response.Data.RUB != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.RUB.Code, response.Data.RUB.Value);
        }

        if (response.Data.EUR != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.EUR.Code, response.Data.EUR.Value);
        }

        if (response.Data.JPY != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.JPY.Code, response.Data.JPY.Value);
        }

        if (response.Data.CNY != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.CNY.Code, response.Data.CNY.Value);
        }

        if (response.Data.RSD != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.RSD.Code, response.Data.RSD.Value);
        }

        if (response.Data.BGN != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.BGN.Code, response.Data.BGN.Value);
        }

        if (response.Data.ARS != null)
        {
            rates.Add(baseCurrency.ToString() + response.Data.ARS.Code, response.Data.ARS.Value);
        }

        return new CurrencyRateResponse
        {
            BaseCurrency = baseCurrency,
            Rates = rates,
            Date = date
        };
    }
}