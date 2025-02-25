using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Interfaces;

namespace RatesProvider.Application.Services;

public class CurrencyRateManager : ICurrencyRateManager
{
    public int CurrentProviderId { get; private set; }
    private IRatesProviderContext _context;
    private readonly List<ICurrencyRateProvider> _providers;
    private readonly ILogger _logger;
    private readonly IBus _bus;

    public CurrencyRateManager(IRatesProviderContext context,
        [FromKeyedServices("Fixer")] ICurrencyRateProvider providerFixer,
        [FromKeyedServices("CurrencyApi")] ICurrencyRateProvider providerCurrencyApi,
        [FromKeyedServices("OpenExchangeRates")] ICurrencyRateProvider providerOpenExchangeRates,
         ILogger<CurrencyRateManager> logger, IBus bus)
    {
        _context = context;
        _logger = logger;
        _bus = bus;
        CurrentProviderId = 0; // провайдер с которого начинаем
        _providers = new List<ICurrencyRateProvider>
            {
                providerFixer,
                providerCurrencyApi,
                providerOpenExchangeRates
            };
        _context.SetCurrencyRatesProvider(_providers[CurrentProviderId]);
    }

    public async Task<CurrencyRateMessage> GetRatesAsync()
    {
        _context.SetCurrencyRatesProvider(_providers[CurrentProviderId]);
        var result = await _context.GetRatesAsync();
        if (result?.Rates.Any() == true)
        {
            _logger.LogInformation("Successfully retrieved currency rates from {ProviderType}", _providers[CurrentProviderId].GetType().Name);
            await _bus.Publish(result);
            return result;
        }

        _logger.LogWarning("No data returned from {ProviderType}. Throwing exception to switch provider.");
        throw new Exception($"Provider {_providers[CurrentProviderId].GetType().Name} failed to fetch rates.");

    }

    public void SetNextProvider()
    {
        if (CurrentProviderId != 2)
        {
            CurrentProviderId++;
        }
        else
        {
            CurrentProviderId = 0;
        }

        _context.SetCurrencyRatesProvider(_providers[CurrentProviderId]);

        _logger.LogInformation("Switched to provider: {ProviderType}", _providers[CurrentProviderId].GetType().Name);
    }
}










