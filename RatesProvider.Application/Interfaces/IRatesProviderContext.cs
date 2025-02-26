using MYPBackendMicroserviceIntegrations.Messages;

namespace RatesProvider.Application.Interfaces
{
    public interface IRatesProviderContext
    {
        Task<CurrencyRateMessage> GetRatesAsync();
        void SetCurrencyRatesProvider(ICurrencyRateProvider currencyRateProvider);
    }
}