using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces
{
    public interface IRatesProviderContext
    {
        Task<CurrencyRateResponse> GetRatesAsync();
        void SetCurrencyRatesProvider(ICurrencyRateProvider currencyRateProvider);
    }
}