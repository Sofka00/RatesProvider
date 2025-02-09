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

    public OpenExchangeRatesClient(IOptions<ApiSettings> appSettings, ICommonHttpClient ratesProviderHttpRequest)
    {
        _appSettings = appSettings.Value;
        _commonHttpClient = ratesProviderHttpRequest;
    }

    public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
    {
        var url = $"https://openexchangerates.org/api/latest.json?app_id={_appSettings.OpenExchangeRatesApiKey}";
        var response = await _commonHttpClient.SendRequestAsync<OpenExchangeRatesResponse>(url);

        var currencyRate = new CurrencyRateResponse
        {
            BaseCurrency = Enum.Parse<Currences>(response.Base),
            Rates = response.Rates,
            Date = response.Date,
        };

        return currencyRate;
    }
}
