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
        var context = new RatesProviderContext();
        //context.SetCurrencyRate(new CurrencyApiClient(httpclient));
        //context.SetCurrencyRate(new FixerClient(httpclient));
        //context.SetCurrencyRate(new OpenExchangeRatesClient(httpclient));
        //context.Execute();
        var providerChoise = 1;
        

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

        var response = await context.Execute();

        return response;
    }
}
