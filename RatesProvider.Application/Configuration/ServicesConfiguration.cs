using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Services;
using System.Net.Http;

namespace RatesProvider.Application.Configuration;

public static class ServicesConfiguration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyRateManager, CurrencyRateManager>();
        services.AddKeyedSingleton<ICurrencyRateProvider, FixerClient>("Fixer");
        services.AddKeyedSingleton<ICurrencyRateProvider, CurrencyApiClient>("CurrencyApi");
        services.AddKeyedSingleton<ICurrencyRateProvider, OpenExchangeRatesClient>("OpenExchangeRates");
        services.AddSingleton<IRatesProviderContext, RatesProviderContext>();
        services.AddHttpClient<ICommonHttpClient, CommonHttpClient>();


    }
}
