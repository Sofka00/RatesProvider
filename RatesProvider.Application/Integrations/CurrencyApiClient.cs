using Microsoft.Extensions.Options;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.CurrencyApiModels;

namespace RatesProvider.Application.Integrations;

public class CurrencyApiClient : ICurrencyRateProvider
{
    private readonly ApiSettings _appSettings;
    private readonly ICommonHttpClient _commonHttpClient;
    public CurrencyApiClient(IOptions<ApiSettings> appSettings, ICommonHttpClient ratesProviderHttpRequest)
    {
        _appSettings = appSettings.Value;
        _commonHttpClient = ratesProviderHttpRequest;
    }

    public async Task<CurrencyRateResponse> GetCurrencyRatesAsync()
    {
        var url = $"https://api.currencyapi.com/v3/latest?apikey={_appSettings.CurrencyApiKey}";
        var response = await _commonHttpClient.SendRequestAsync<CurrencyResponse>(url);

        var currencyRate = new CurrencyRateResponse
        {
            BaseCurrency = Enum.Parse<Currencies>(response.Data.ToString()),
            //Rates = Enum.Parse<Currences>(response),
            Date = DateTime.Parse(response.Meta.ToString())
        };

        return currencyRate;
    }
}