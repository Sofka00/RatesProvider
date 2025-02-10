using RatesProvider.Application.Models;

namespace RatesProvider.Application.Integrations
{
    public interface ICommonHttpClient
    {
        List<Currencies> GetAvailableCurrencies();
        Task<T> SendRequestAsync<T>(string url);
    }
}