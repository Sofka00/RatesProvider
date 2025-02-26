using MYPBackendMicroserviceIntegrations.Messages;

namespace RatesProvider.Application.Interfaces;

public interface ICurrencyRateProvider
{
    Task<CurrencyRateMessage> GetCurrencyRatesAsync();
}