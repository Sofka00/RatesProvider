using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Services;

namespace RatesProvider.Application.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencyApiService, CurrencyApiService>();
        }
    }
}
