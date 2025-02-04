namespace RatesProvider.Application.Interfaces
{
    public interface IFixerApiService
    {
        Task Execute();
        Task GetCurrencyRateWithTypedClientAsync();
    }
}