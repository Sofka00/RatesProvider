using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;

namespace RatesProvider.Application.Services;

public class FixerApiService : IFixerApiService
{
    private readonly FixerClient _fixerClient;

    public FixerApiService(FixerClient fixerClient)
    {
        _fixerClient = fixerClient;
    }

    public async Task Execute()
    {
        await GetCurrencyRateWithTypedClientAsync();
    }

    public async Task GetCurrencyRateWithTypedClientAsync() => await _fixerClient.GetCurrencyRatesAsync();
}
