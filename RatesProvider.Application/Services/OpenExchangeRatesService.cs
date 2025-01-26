using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;

namespace RatesProvider.Application.Services
{
    public class OpenExchangeRatesService : IOpenExchangeRatesService
    {
        private readonly OpenExchangeRatesClient _openExchangeRatesClient;

        public OpenExchangeRatesService(OpenExchangeRatesClient openExchangeRatesClientClient)
        {
            _openExchangeRatesClient = openExchangeRatesClientClient;
        }

        public async Task ExecuteAsync()
        {
            await GetCurrencyRateWithTypedClientAsync();
        }

        public async Task GetCurrencyRateWithTypedClientAsync() => await _openExchangeRatesClient.GetCurrencyRatesAsync();
    }
}
