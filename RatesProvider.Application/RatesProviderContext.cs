using RatesProvider.Application.Interfaces;

namespace RatesProvider.Application
{
    public class RatesProviderContext : IRatesProviderContext
    {
        public ICurrencyRateProvider _currencyRateProvider;

        public RatesProviderContext()
        { }

        public RatesProviderContext(ICurrencyRateProvider currencyRateProvider)
        {
            _currencyRateProvider = currencyRateProvider;
        }

        public void SetCurrencyRate(ICurrencyRateProvider currencyRateProvider)
        {
            _currencyRateProvider = currencyRateProvider;
        }

        public async Task Execute()
        {
            await _currencyRateProvider.GetCurrencyRatesAsync();
        }
    }
}