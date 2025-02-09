using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services
{
    public class CurrencyRateManager : ICurrencyRateManager
    {
        private readonly ICurrencyRateProvider _providerFixer;
        private IRatesProviderContext _context;
        ILogger<CurrencyRateManager> _logger;

        public CurrencyRateManager(IRatesProviderContext context, [FromKeyedServices("Fixer")] ICurrencyRateProvider providerFixer, ILogger<CurrencyRateManager> logger)
        {
            _context = context;
            _providerFixer = providerFixer;
            _logger = logger;

            _logger.LogInformation("CurrencyRateManager initialized with provider: {ProviderType}", _providerFixer.GetType().Name);
        }

        public async Task<CurrencyRateResponse> GetRatesAsync()
        {
            CurrencyRateResponse result = default;

            try
            {
                _logger.LogInformation("Setting the currency rate provider to: {ProviderType}", _providerFixer.GetType().Name);
                _context.SetCurrencyRatesProvider(_providerFixer);

                _logger.LogInformation("Attempting to retrieve currency rates...");

                result = await _context.GetRatesAsync();

                if (result != null)
                {
                    _logger.LogInformation("Successfully retrieved currency rates.");
                }
                else
                {
                    _logger.LogWarning("No currency rates were returned.");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while retrieving currency rates.");
            }

            return result;
        }


    }
}