using Microsoft.Extensions.Logging;
using Moq;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Services;

namespace RatesProvider.Application.Tests
{
    public class CurrencyRateManagerTest
    {
        private readonly Mock<IRatesProviderContext> _mockContext;
        private readonly Mock<ICurrencyRateProvider> _mockProviderFixer;
        private readonly Mock<ICurrencyRateProvider> _mockProviderCurrencyApi;
        private readonly Mock<ICurrencyRateProvider> _mockProviderOpenExchangeRates;
        private readonly Mock<ILogger<CurrencyRateManager>> _mockLogger;

        public CurrencyRateManagerTest()
        {
            _mockContext = new Mock<IRatesProviderContext>();
            _mockProviderFixer = new Mock<ICurrencyRateProvider>();
            _mockProviderCurrencyApi = new Mock<ICurrencyRateProvider>();
            _mockProviderOpenExchangeRates = new Mock<ICurrencyRateProvider>();
            _mockLogger = new Mock<ILogger<CurrencyRateManager>>();
        }
        [Fact]
        public async Task GetRatesAsync_ShouldReturnRates_WhenFirstProviderSucceeds()
        {

            var expectedRates = new CurrencyRateResponse
            {
                BaseCurrency = Currency.USD,
                Rates = new Dictionary<string, decimal> { { "USDEUR", 0.85m } },
                Date = DateTime.UtcNow
            };

            _mockContext.Setup(ctx => ctx.GetRatesAsync()).ReturnsAsync(expectedRates);

            var manager = new CurrencyRateManager(
                _mockContext.Object,
                _mockProviderFixer.Object,
                _mockProviderCurrencyApi.Object,
                _mockProviderOpenExchangeRates.Object,
                _mockLogger.Object
            );


            var result = await manager.GetRatesAsync();


            Assert.NotNull(result);
            Assert.Equal(Currency.USD, result.BaseCurrency);
            Assert.Contains("USDEUR", result.Rates);
            Assert.Equal(0.85m, result.Rates["USDEUR"]);
        }

        [Fact]
        public async Task GetRatesAsync_ShouldFallbackToNextProvider_WhenFirstFails()
        {
            var failedResponse = new CurrencyRateResponse { BaseCurrency = Currency.USD, Rates = new Dictionary<string, decimal>() };
            var expectedRates = new CurrencyRateResponse
            {
                BaseCurrency = Currency.USD,
                Rates = new Dictionary<string, decimal> { { "EUR", 1.05m } },
                Date = DateTime.UtcNow
            };

            _mockContext
                .SetupSequence(ctx => ctx.GetRatesAsync())
                .ReturnsAsync(failedResponse)
                .ReturnsAsync(expectedRates);


            var manager = new CurrencyRateManager(
                _mockContext.Object,
                _mockProviderFixer.Object,
                _mockProviderCurrencyApi.Object,
                _mockProviderOpenExchangeRates.Object,
                _mockLogger.Object
            );

            var result = await manager.GetRatesAsync();

            Assert.NotNull(result);
            Assert.Equal(Currency.USD, result.BaseCurrency);
            Assert.Contains("EUR", result.Rates);
            Assert.Equal(1.05m, result.Rates["EUR"]);
        }

        public async Task GetRatesAsync_ShouldReturnNull_WhenAllProvidersFail()
        {

            _mockContext.Setup(ctx => ctx.GetRatesAsync()).ReturnsAsync((CurrencyRateResponse)null);

            var manager = new CurrencyRateManager(
                _mockContext.Object,
                _mockProviderFixer.Object,
                _mockProviderCurrencyApi.Object,
                _mockProviderOpenExchangeRates.Object,
                _mockLogger.Object
            );

            var result = await manager.GetRatesAsync();

            Assert.Null(result);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to retrieve currency rates")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }


}


