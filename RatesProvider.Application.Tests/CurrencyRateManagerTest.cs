using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.OpenExchangeRatesModels;
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
        public async Task CurrencyRateManager_SuccessfulReceiptOfExchangeRates_ReturnSaccessfulResponse()
        {
            var expectedRates = new CurrencyRateResponse
            {
                BaseCurrency = Currency.USD,
                Rates = new Dictionary<string, decimal>
                {
                     { "RUB", 90.50m },
                     { "EUR", 0.92m },
                },
                Date = DateTime.UtcNow,
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
            Assert.Contains("USDRUB", result.Rates.Keys);
            Assert.Contains("USDEUR", result.Rates.Keys);
            Assert.Equal(90.50m, result.Rates["USDRUB"]);
            Assert.Equal(0.75m, result.Rates["USDEUR"]);
        }

    }
}
