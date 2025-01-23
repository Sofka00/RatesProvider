using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services
{
    public class CurrencyRateManager : ICurrencyRateManager
    {
        private readonly ICurrencyRateProvider _providers;
        private IFixerApiService _fixerApiService;

        public CurrencyRateManager(ICurrencyRateProvider providers)
        {
            _providers = providers;
        }

        public async Task<CurrencyRateResponse> GetRatesAsync(string baseCurrency)
        {

            int currentProviderIndex = 0;// индекс текущего поставщика 
            CurrencyRateResponse rates = null; //Хранение  курсов (под вопросом) 

            do
            {

                //if (currentProviderIndex >= _providers.Count())
                //{
                //    throw new Exception("Не удалось ");
                //}

               // var provider = _providers.ElementAt(currentProviderIndex);
                
                rates = await _providers.GetCurrencyRatesAsync();// тут получпем курсы и в теории где-то тут нужно предусмотерть baseCurrency 

                currentProviderIndex++; // тут идем к следующему


            }
            while (rates == null || !rates.Rates.Any()); // повторяем пока не будет успешно
            return rates; // если сюда пришло, значит есть успешный результат
        }
    }
}