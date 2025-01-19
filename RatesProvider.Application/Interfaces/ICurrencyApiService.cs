namespace RatesProvider.Application.Interfaces
{
    public interface ICurrencyApiService
    {
        Task Execute();
        Task GetCurrencyRateWithTypedClientAsync();
    }
}