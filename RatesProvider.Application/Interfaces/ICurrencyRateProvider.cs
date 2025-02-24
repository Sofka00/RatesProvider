using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces;

public interface ICurrencyRateProvider
{
    Task<CurrencyRateMessage> GetCurrencyRatesAsync();
}