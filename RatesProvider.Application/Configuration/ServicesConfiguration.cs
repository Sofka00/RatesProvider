using Microsoft.Extensions.DependencyInjection;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Services;
using MassTransit;
using RatesProvider.Application.RabbitMQ;

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
        services.AddSingleton<ICommonHttpClient, CommonHttpClient>();

        services.AddMassTransit(x =>
        {

            x.AddConsumer<OrderCreatedConsumer>();


            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost");


                cfg.ReceiveEndpoint("currencyRateQueue", e =>
                {
                    e.ConfigureConsumer<OrderCreatedConsumer>(context);
                });
            });
        });

    }
}
    
    



