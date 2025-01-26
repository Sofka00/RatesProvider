using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services;

public class CurrencyRateManager
{
    private readonly IRatesProviderContext _ratesProviderContext;

    public CurrencyRateManager(IRatesProviderContext ratesProviderContext)
    {
        _ratesProviderContext = ratesProviderContext;
    }

    public async Task<CurrencyRate> GetRate()
    {

        return null;
    }
}
