using RatesProvider.Application.Models;
using RatesProvider.Application.Models.OpenExchangeRatesModels;

namespace RatesProvider.Application.Interfaces;

public interface ICurrencyRateProvider
{
    Task<CurrencyRate> GetCurrencyRatesAsync();
}
