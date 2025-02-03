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
            services.AddSingleton<ICurrencyRateManager, CurrencyRateManager>();
            services.AddKeyedSingleton<ICurrencyRateProvider, FixerClient>("Fixer"); 
            services.AddSingleton<IRatesProviderContext, RatesProviderContext>();
            services.AddSingleton<ICommonHttpClient, CommonHttpClient>();
            services.AddHttpClient();
            
        }
    }
}
