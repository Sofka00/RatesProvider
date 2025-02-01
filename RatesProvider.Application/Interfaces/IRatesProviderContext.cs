using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces
{
    public interface IRatesProviderContext
    {
        Task<CurrencyRateResponse> Execute();
        void SetCurrencyRate(ICurrencyRateProvider currencyRateProvider);
    }
}