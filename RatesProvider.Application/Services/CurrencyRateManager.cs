using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;

namespace RatesProvider.Application.Services;

public class CurrencyRateManager : ICurrencyRateManager
{
    private IRatesProviderContext _ratesProviderContext;
    private readonly HttpClient _httpclient;

    public CurrencyRateManager(IRatesProviderContext ratesProviderContext, HttpClient httpclient)
    {
        _ratesProviderContext = ratesProviderContext;
        _httpclient = httpclient;
    }

    public async Task<CurrencyRateResponse> GetRatesAsync()
    {
        for (int i = 1; i < 4; i++)
        {
            var context = new RatesProviderContext();

            var providerChoise = i;

            switch (providerChoise)
            {
                case 1:
                    context.SetCurrencyRate(new CurrencyApiClient(_httpclient));
                    break;
                case 2:
                    context.SetCurrencyRate(new OpenExchangeRatesClient(_httpclient));
                    break;
                case 3:
                    context.SetCurrencyRate(new FixerClient(_httpclient));
                    break;
                default:
                    return null;
            }

            await context.Execute();
        }

        return null;
    }
}