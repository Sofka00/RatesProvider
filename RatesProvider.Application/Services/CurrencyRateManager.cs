using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services
{
    public class CurrencyRateManager : ICurrencyRateManager
    {
        private readonly ICurrencyRateProvider _providerFixer;
        private IRatesProviderContext _context;

        public CurrencyRateManager(IRatesProviderContext context, [FromKeyedServices("Fixer")] ICurrencyRateProvider providerFixer)
        {
            _context = context;
            _providerFixer = providerFixer;

        }

        public async Task<CurrencyRateResponse> GetRatesAsync()
        {
            CurrencyRateResponse result = default;

            _context.SetCurrencyRatesProvider(_providerFixer);

            result = await _context.GetRatesAsync();

            return result;
        }


    }
}