using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces
{
    public interface ICurrencyRateManager
    {
        Task<CurrencyRateResponse> GetRatesAsync(HttpClient httpclient);
    }
}