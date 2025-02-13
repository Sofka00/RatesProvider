using MassTransit;
using System.Text.Json;
using static SharedModels.Class1;


namespace RatesProvider.Application.RabbitMQ
{
    public class OrderCreatedConsumer : IConsumer<CurrencyRate>
    {
        public async Task Consume(ConsumeContext<CurrencyRate> context)
        {
            try
            {
                var currencyRateMessage = context.Message;
                var jsonMessage = JsonSerializer.Serialize(currencyRateMessage);

                Console.WriteLine($"Received OrderCreated message: {jsonMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }
    }
}

