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

    public OpenExchangeRatesClient(IOptions<OpenExchangeClientSettings> openExchangeSettings, ICommonHttpClient ratesProviderHttpRequest)
    {
        _openExchangeSettings = openExchangeSettings.Value;
        _commonHttpClient = ratesProviderHttpRequest;
    }

    public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
    {
        var url = $"{_openExchangeSettings.BaseUrl}{_openExchangeSettings.QueryOption}{_openExchangeSettings.ApiKey}";
        var response = await _commonHttpClient.SendRequestAsync<OpenExchangeRatesResponse>(url);

        var currencyRate = new CurrencyRateResponse
        {
            BaseCurrency = Enum.Parse<BaseCurrency>(response.Base),
            Rates = response.Rates,
            Date = response.Date,
        };

        return currencyRate;
    }
}
