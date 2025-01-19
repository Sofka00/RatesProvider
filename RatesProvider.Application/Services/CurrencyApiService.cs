using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;



namespace RatesProvider.Application.Services;

public class CurrencyApiService : ICurrencyApiService
{
    private readonly CurrencyApiClient _currencyProviderClient;

    public CurrencyApiService(CurrencyApiClient currencyProviderClient)
    {
        _currencyProviderClient = currencyProviderClient;
    }

    public async Task Execute()
    {
        await GetCurrencyRateWithTypedClientAsync();
    }

    public async Task GetCurrencyRateWithTypedClientAsync() => await _currencyProviderClient.GetCurrencyRatesAsync();
}
