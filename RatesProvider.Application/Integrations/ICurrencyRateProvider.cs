using RatesProvider.Application.Models;

namespace RatesProvider.Application.Integrations
{
    public interface ICurrencyRateProvider
    {
        Task<CurrencyRateResponse> GetCurrencyRatesAsync();
    }
}