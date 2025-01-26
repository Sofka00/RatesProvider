namespace RatesProvider.Application.Interfaces
{
    public interface IRatesProviderContext
    {
        Task GetRatesAsync();
        void SetCurrencyRate(ICurrencyRateProvider currencyRateProvider);
    }
}