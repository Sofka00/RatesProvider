using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Services;
using System;

namespace RatesProvider.Application.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencyRateManager, CurrencyRateManager>();
            services.AddSingleton<IRatesProviderContext, RatesProviderContext>();
            services.AddKeyedSingleton<ICurrencyRateProvider, CurrencyApiClient>("CurrencyApi");
            services.AddKeyedSingleton<ICurrencyRateProvider, OpenExchangeRatesClient>("OpenExchangeRatesClient");

            services.AddHttpClient<CurrencyApiClient>();
            services.AddHttpClient<OpenExchangeRatesClient>();
        }
    }
}
