using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces
{
    public interface ICurrencyRateManager
    {
        Task<CurrencyRateResponse> GetRatesAsync(string baseCurrency);
    }
}