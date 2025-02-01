using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

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

        public async Task<CurrencyRateResponse> Execute()
        {
            CurrencyRateResponse response = null;
            
            for (int i = 0; i < 3; i++)
            {
                response = await _currencyRateProvider.GetCurrencyRatesAsync();
                
                if (response != null) 
                {
                    return response;
                }
            }
            return null;
            //_currencyRateProvider.GetCurrencyRatesAsync();
        }
    }
}
