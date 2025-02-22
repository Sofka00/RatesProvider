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
    private readonly ILogger _logger;

    public CurrencyRateManager(IRatesProviderContext context,
        [FromKeyedServices("Fixer")] ICurrencyRateProvider providerFixer,
        [FromKeyedServices("CurrencyApi")] ICurrencyRateProvider providerCurrencyApi,
        [FromKeyedServices("OpenExchangeRates")] ICurrencyRateProvider providerOpenExchangeRates,
         ILogger<CurrencyRateManager> logger
        )
    {

        _providerOpenExchangeRates = providerOpenExchangeRates;
        _providerCurrencyApi = providerCurrencyApi;
        _providerFixer = providerFixer;
        _context = context;

        _logger = logger;
    }

    public async Task<CurrencyRateResponse> GetRatesAsync()
    {
        CurrencyRateResponse result = default;

        var providers = new List<ICurrencyRateProvider>
        {
            _providerFixer,
            _providerCurrencyApi,
            _providerOpenExchangeRates
        };

        foreach (var provider in providers)
        {
            try
            {
                _logger.LogInformation("Trying to fetch currency rates from {ProviderType}", provider.GetType().Name);

                _context.SetCurrencyRatesProvider(provider);
                result = await _context.GetRatesAsync();

                if (result != null && result.Rates.Any())
                {
                    _logger.LogInformation("Successfully retrieved currency rates from {ProviderType}", provider.GetType().Name);
                    return result;
                }
                else
                {
                    _logger.LogWarning("No data returned from {ProviderType}. Trying next provider...", provider.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching rates from {ProviderType}. Trying next provider...", provider.GetType().Name);
            }
        }

        _logger.LogError("Failed to retrieve currency rates from all providers.");
        return result; 
    }
}








