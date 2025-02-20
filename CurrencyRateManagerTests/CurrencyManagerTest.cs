using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Moq;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Services;

namespace CurrencyRateManagerTests
{
    public class CurrencyManagerTest
    {
        [Fact]
        public async void CurrencyRateManager_GetRatesAsync_SuccsesfulPuplishingMassegeToRabbitMq()
        {
            var mockContext = new Mock<IRatesProviderContext>();
            var mockBus = new Mock<IBus>();

            var fakeRates = new CurrencyRateResponse
            {
                BaseCurrency = Currences.USD,
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
                mockBus.Object
            );


            var result = await service.GetRatesAsync();


            Assert.NotNull(result);

            mockBus.Verify(bus => bus.Publish(
                It.Is<CurrencyRateResponse>(msg =>
                    msg.BaseCurrency == Currences.USD &&
                    msg.Rates.ContainsKey("USD") &&
                    msg.Rates["RUB"] == 0.85m),
                It.IsAny<CancellationToken>()),
                Times.Once);

        }

    }

}
