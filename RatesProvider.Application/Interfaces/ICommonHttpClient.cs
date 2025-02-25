using RatesProvider.Application.Models;

namespace RatesProvider.Application.Integrations
{
    public interface ICommonHttpClient
    {
        Task<T> SendRequestAsync<T>(string url);
    }
}