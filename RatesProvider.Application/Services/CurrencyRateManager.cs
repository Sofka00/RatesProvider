using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using System.Data;

namespace RatesProvider.Application.Services;

public class CurrencyRateManager : ICurrencyRateManager
{
    private readonly List<ICurrencyRateProvider> _providers;
    private IRatesProviderContext _context;
    private readonly ILogger _logger;
    private int _currentProviderId;
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
        _currentProviderId = 0; // провайдер с которого начинаем
        _providers = new List<ICurrencyRateProvider>
            {
                providerFixer,
                providerCurrencyApi,
                providerOpenExchangeRates
            };
        _context.SetCurrencyRatesProvider(_providers[_currentProviderId]);
    }

    public async Task<CurrencyRateMessage> GetRatesAsync()
    {
        for (int i = 0; i < _providers.Count; i++)
        {
      
            var result = await _context.GetRatesAsync();

            if (result != null && result.Rates.Any())
            {
                _logger.LogInformation("Successfully retrieved currency rates from {ProviderType}", _providers[_currentProviderId].GetType().Name);
                await _bus.Publish(result);

                return result;
            }
            else
            {
                _logger.LogWarning("No data returned from {ProviderType}. Switching to next provider...", _providers[_currentProviderId].GetType().Name);
            }

            _logger.LogWarning("No data returned from {ProviderType}. Switching to next provider...", _providers[_currentProviderId].GetType().Name);

        }

        _logger.LogError("All providers failed to fetch currency rates.");
        throw new Exception("Unable to get exchange rates from all providers");

    }


    private void SetNextProvider()
    {
        if (_currentProviderId !=2)
        {
            _currentProviderId++;
        }
        else
        {
            _currentProviderId = 0;
        }
  
        _context.SetCurrencyRatesProvider(_providers[_currentProviderId]);

        _logger.LogInformation("Switched to provider: {ProviderType}", _providers[_currentProviderId].GetType().Name);
    }
}





    




