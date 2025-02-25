using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces
{
    public interface IRatesProviderContext
    {
        Task<CurrencyRateMessage> GetRatesAsync();
        void SetCurrencyRatesProvider(ICurrencyRateProvider currencyRateProvider);
    }
}