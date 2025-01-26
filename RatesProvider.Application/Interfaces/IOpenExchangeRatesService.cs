namespace RatesProvider.Application.Interfaces
{
    public interface IOpenExchangeRatesService
    {
        Task ExecuteAsync();
        Task GetCurrencyRateWithTypedClientAsync();
    }
}