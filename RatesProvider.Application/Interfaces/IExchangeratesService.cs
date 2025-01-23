namespace RatesProvider.Application.Interfaces
{
    public interface IExchangeratesService
    {
        Task Execute();
        Task GetCurrencyRateWithTypedClientAsync();
    }
}