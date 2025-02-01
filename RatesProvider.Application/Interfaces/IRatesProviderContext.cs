namespace RatesProvider.Application.Interfaces
{
    public interface IRatesProviderContext
    {
        void Execute();
        void SetCurrencyRate(ICurrencyRateProvider currencyRateProvider);
    }
}