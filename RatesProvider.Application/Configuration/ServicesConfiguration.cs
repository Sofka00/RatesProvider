using Microsoft.Extensions.DependencyInjection;

namespace RatesProvider.Application.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services ) 
        {
            services.AddScoped<ICurrencyProvider1, CurrencyProvider1>();
        }
    }
}
