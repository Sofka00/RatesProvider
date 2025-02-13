using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services
{
    public class RatesProviderContext : IRatesProviderContext
    {
        private ICurrencyRateProvider _currencyRateProvider;

        public RatesProviderContext()
        {
            _currencyRateProvider = null;
        }

        public void SetCurrencyRatesProvider(ICurrencyRateProvider currencyRateProvider)
        {
            _currencyRateProvider = currencyRateProvider;
        }

        public async Task<CurrencyRateResponse> GetRatesAsync()
        {
            CurrencyRateResponse response = default;
            TimeSpan interval = new TimeSpan(0, 0, 2);

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    response = await _currencyRateProvider.GetCurrencyRatesAsync();

                }
                catch(Exception ex)
                {
                    var c = ex;
                    await Task.Delay(interval * i);

                }
            }

            return response;
        }
    }
}
