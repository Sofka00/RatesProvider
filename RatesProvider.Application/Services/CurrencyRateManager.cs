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

    public async Task<CurrencyRateResponse> GetRatesAsync(HttpClient httpclient)
    {
        var context = new RatesProviderContext();
        //context.SetCurrencyRate(new CurrencyApiClient(httpclient));
        //context.SetCurrencyRate(new FixerClient(httpclient));
        //context.SetCurrencyRate(new OpenExchangeRatesClient(httpclient));
        //context.Execute();

       

        switch (model)
        {
            case 1:
                context.SetCurrencyRate(new CurrencyApiClient(httpclient));
                //for (int i = 0; i < length; i++)
                //{

                //}
                break;
            case 2:
                context.SetCurrencyRate(new FixerClient(httpclient));
                break;
            case 3:
                context.SetCurrencyRate(new OpenExchangeRatesClient(httpclient));
                break;
        }
        context.Execute();
        
        return null;
    }
}
