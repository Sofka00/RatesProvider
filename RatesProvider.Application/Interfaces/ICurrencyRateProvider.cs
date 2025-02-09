using RatesProvider.Application.Models;

namespace RatesProvider.Application.Interfaces;

public interface ICurrencyRateProvider
{
    Task<CurrencyRateResponse> GetCurrencyRatesAsync();
}