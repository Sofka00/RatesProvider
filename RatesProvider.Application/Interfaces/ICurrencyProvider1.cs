using RatesProvider.Application.Models;

public interface ICurrencyProvider1
{
    Task<Quota[]> GetRatesAsync(string baseCurrency);
}