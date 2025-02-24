using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces
{
    public interface ICurrencyRateManager
    {
        Task<CurrencyRateMessage> GetRatesAsync();
    }
}