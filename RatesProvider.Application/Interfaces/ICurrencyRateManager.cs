using MYPBackendMicroserviceIntegrations.Messages;

namespace RatesProvider.Application.Interfaces
{
    public interface ICurrencyRateManager
    {
        Task<CurrencyRateMessage> GetRatesAsync();
        void SetNextProvider();
    }
}