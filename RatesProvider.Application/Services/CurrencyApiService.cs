using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;

namespace RatesProvider.Application.Services;

public class CurrencyApiService : ICurrencyApiService
{
    private CurrencyApiClient _currencyApiClient;

    public CurrencyApiService(CurrencyApiClient currencyApiClient)
    {
        _currencyApiClient = currencyApiClient;
    }

    public async Task ExecuteAsync()
    {
        await GetCurrencyRateWithTypedClientAsync();
    }

    public async Task GetCurrencyRateWithTypedClientAsync() => await _currencyApiClient.GetCurrencyRatesAsync();
}
