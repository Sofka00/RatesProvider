using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services;

public class CurrencyRateManager : ICurrencyRateManager
{
    private readonly ICurrencyRateProvider _providerFixer;
    private readonly ICurrencyRateProvider _providerCurrencyApi;
    private readonly ICurrencyRateProvider _providerOpenExchangeRates;
    private IRatesProviderContext _context;
    private readonly IBus _bus;
    private readonly ILogger _logger;

    public CurrencyRateManager(IRatesProviderContext context,
        [FromKeyedServices("Fixer")] ICurrencyRateProvider providerFixer,
        [FromKeyedServices("CurrencyApi")] ICurrencyRateProvider providerCurrencyApi,
        [FromKeyedServices("OpenExchangeRates")] ICurrencyRateProvider providerOpenExchangeRates,
         ILogger<CurrencyRateManager> logger,
        IBus bus)
    {

        _providerOpenExchangeRates = providerOpenExchangeRates;
        _providerCurrencyApi = providerCurrencyApi;
        _providerFixer = providerFixer;
        _context = context;

        _logger = logger;
        _bus = bus;
    }
    
    public async Task<CurrencyRateResponse> GetRatesAsync()
    {
        CurrencyRateResponse result = default;
        _context.SetCurrencyRatesProvider(_providerOpenExchangeRates);

        try
        {
            _logger.LogInformation("Setting the currency rate provider to {ProviderType}", _providerOpenExchangeRates.GetType().Name);
            _context.SetCurrencyRatesProvider(_providerOpenExchangeRates);

            _logger.LogInformation("Attempting to retrieve currency rates...");

            result = await _context.GetRatesAsync();

            if (result != null)
            {
                _logger.LogInformation("Successfully retrieved currency rates.");
            }
            else
            {
                _logger.LogError("No currency rates were returned.");
            }
        }


        catch (Exception ex)

        {
            _logger.LogError(ex, "An error occurred while retrieving currency rates.");

        }

        await _bus.Publish(result);
        return result;
    }

}






