namespace RatesProvider.Application.Interfaces
{
    public interface ICommonHttpClient
    {
        Task<T> SendRequestAsync<T>(string url, CancellationToken cancellationToken = default);
    }
}