using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Integrations;
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

            int currentProviderIndex = 0;
            CurrencyRateResponse rates = null;
            int attemps = 0;

            do
            {
                if(currentProviderIndex == 0)
                {
                    rates = await _providerFixer.GetCurrencyRatesAsync();
                }
                else
                {
                    rates = await _providerExhengerate.GetCurrencyRatesAsync();

                }

                if (currentProviderIndex ==0 && attemps>3 )
                {
                    currentProviderIndex = 1;
                    attemps = 0;
                }


                currentProviderIndex++;

            }
            while (rates == null || !rates.Rates.Any());


            return rates;
        }
    }
}