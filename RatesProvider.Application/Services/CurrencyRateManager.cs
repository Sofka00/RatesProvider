using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services
{
    public class CurrencyRateManager : ICurrencyRateManager
    {
        private readonly ICurrencyRateProvider _providerFixer;
        private readonly ICurrencyRateProvider _providerExhengerate;
        private IFixerApiService _fixerApiService;

        public CurrencyRateManager([FromKeyedServices("Fixer")] ICurrencyRateProvider providerFixer, [FromKeyedServices("Exchengerates")] ICurrencyRateProvider providerExhengerate)
        {
            _providerFixer = providerFixer;
            _providerExhengerate = providerExhengerate;
        }

        public async Task<CurrencyRateResponse> GetRatesAsync()
        {

            CurrencyRateResponse rateResponse = null;

            TimeSpan interval = new TimeSpan(0, 0, 2);
            for (int i = 0; i < 3; i++)
            {
                rateResponse = await _providerExhengerate.GetCurrencyRatesAsync();
                if (rateResponse != null)
                {
                    return rateResponse;
                }
                Thread.Sleep(interval * 2);
            }
            for (int i = 0; i < 3; i++)
            {
                rateResponse = await _providerFixer.GetCurrencyRatesAsync();
                if (rateResponse != null)
                {

                    return rateResponse;
                }
                Thread.Sleep(interval * 2);
            }

            return null;

        }
    }
}