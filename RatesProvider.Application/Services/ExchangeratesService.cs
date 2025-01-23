using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Services
{
    public class ExchangeratesService : IExchangeratesService
    {
        private readonly ExchengeratesClient _client;

        public ExchangeratesService(ExchengeratesClient client)
        {
            _client = client;
        }

        public async Task Execute()
        {
            await GetCurrencyRateWithTypedClientAsync();
        }

        public async Task GetCurrencyRateWithTypedClientAsync() => await _client.GetCurrencyRatesAsync();
    }

}

