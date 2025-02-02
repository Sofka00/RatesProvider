using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces
{
    public interface IRatesProviderContext
    {
        Task Execute();
        void SetCurrencyRate(ICurrencyRateProvider currencyRateProvider);
    }
}