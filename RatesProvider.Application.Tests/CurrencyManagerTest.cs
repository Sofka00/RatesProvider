using MassTransit;
using Moq;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Services;
using Microsoft.Extensions.Logging;

namespace CurrencyRateManagerTests
{
    public class CurrencyManagerTest
    {
        [Fact]
        public async void CurrencyRateManager_GetRatesAsync_SuccsesfulPuplishingMassegeToRabbitMq()
        {
            var mockContext = new Mock<IRatesProviderContext>();
            var mockLogger = new Mock<ILogger<CurrencyRateManager>>();
            var mockBus = new Mock<IBus>();

            var fakeRates = new CurrencyRateResponse
            {
                BaseCurrency = Currency.USD,
                Rates = new Dictionary<string, decimal>
            {
                { "USD", 1.00m },
                { "RUB", 0.85m }
            },
                Date = DateTime.UtcNow
            };

            mockContext.Setup(x => x.GetRatesAsync()).ReturnsAsync(fakeRates);

            var service = new CurrencyRateManager(
                mockContext.Object,
                Mock.Of<ICurrencyRateProvider>(),
                Mock.Of<ICurrencyRateProvider>(),
                Mock.Of<ICurrencyRateProvider>(),
                mockLogger.Object,
                mockBus.Object
            );


            var result = await service.GetRatesAsync();


            Assert.NotNull(result);

            mockBus.Verify(bus => bus.Publish(
                It.Is<CurrencyRateResponse>(msg =>
                    msg.BaseCurrency == Currency.USD &&
                    msg.Rates.ContainsKey("USD") &&
                    msg.Rates["RUB"] == 0.85m),
                It.IsAny<CancellationToken>()),
                Times.Once);

        }

    }

}
