using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Services;

namespace RatesProvider.Application.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencyApiService, CurrencyApiService>();
            services.AddSingleton<IOpenExchangeRatesService, OpenExchangeRatesService>();
            services.AddSingleton<IRatesProviderContext, RatesProviderContext>();
            // разное
            services.AddHttpClient<CurrencyApiClient>();
            services.AddHttpClient<OpenExchangeRatesClient>();
            services.AddHttpClient<FixerClient>();
        }
    }
}
