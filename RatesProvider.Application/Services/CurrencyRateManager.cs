using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services;

public class CurrencyRateManager : ICurrencyRateManager
{
    private readonly ICurrencyRateProvider _providerFixer;
    private readonly ICurrencyRateProvider _providerCurrencyApi;
    private readonly ICurrencyRateProvider _providerOpenExchangeRates;
    private IRatesProviderContext _context;

    public CurrencyRateManager(IRatesProviderContext context, 
        [FromKeyedServices("Fixer")] ICurrencyRateProvider providerFixer, 
        [FromKeyedServices("CurrencyApi")] ICurrencyRateProvider providerCurrencyApi,
        [FromKeyedServices("OpenExchangeRates")] ICurrencyRateProvider providerOpenExchangeRates
        )
    {
        _context = context;
        _providerFixer = providerFixer;
        _providerCurrencyApi = providerCurrencyApi;
        _providerOpenExchangeRates = providerOpenExchangeRates;

    }

    public async Task<CurrencyRateResponse> GetRatesAsync()
    {
        CurrencyRateResponse result = default;

        _context.SetCurrencyRatesProvider(_providerCurrencyApi);

        result = await _context.GetRatesAsync();

        return result;
    }
}