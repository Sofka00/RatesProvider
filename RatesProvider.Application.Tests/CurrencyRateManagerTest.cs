using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using MYPBackendMicroserviceIntegrations.Messages;
using RatesProvider.Application.Interfaces;
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
        private readonly Mock<IBus> _mockBus;
        private readonly CurrencyRateManager _manager;
        public CurrencyRateManagerTest()
        {
            _mockContext = new Mock<IRatesProviderContext>();
            _mockProviderFixer = new Mock<ICurrencyRateProvider>();
            _mockProviderCurrencyApi = new Mock<ICurrencyRateProvider>();
            _mockProviderOpenExchangeRates = new Mock<ICurrencyRateProvider>();
            _mockLogger = new Mock<ILogger<CurrencyRateManager>>();
            _mockBus = new Mock<IBus>();

            _manager = new CurrencyRateManager(
               _mockContext.Object,
               _mockProviderFixer.Object,
               _mockProviderCurrencyApi.Object,
               _mockProviderOpenExchangeRates.Object,
               _mockLogger.Object,
               _mockBus.Object
            );
        }
        [Fact]
        public async Task GetRatesAsync_ShouldReturnRates_WhenProviderSucceeds()
        {
            // Arrange
            var fakeResponse = new CurrencyRateMessage
            {
                Rates = new Dictionary<string, decimal> { { "USD", 1.1m } }
            };

            _mockContext.Setup(c => c.GetRatesAsync()).ReturnsAsync(fakeResponse);

            // Act
            var result = await _manager.GetRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Rates);
            _mockBus.Verify(bus => bus.Publish(fakeResponse, default), Times.Once);
        }

        [Fact]
        public async Task GetRatesAsync_ShouldThrowException_WhenProviderFails()
        {
            // Arrange
            _mockContext.Setup(c => c.GetRatesAsync()).ReturnsAsync((CurrencyRateMessage)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _manager.GetRatesAsync());
        }

        [Fact]
        public void SetNextProvider_ShouldSwitchProvidersCorrectly()
        {
            // Arrange
            Assert.Equal(0, _manager.CurrentProviderId);

            // Act 
            _manager.SetNextProvider();

            // Assert
            Assert.Equal(1, _manager.CurrentProviderId);

            _manager.SetNextProvider();
            Assert.Equal(2, _manager.CurrentProviderId);

            _manager.SetNextProvider();
            Assert.Equal(0, _manager.CurrentProviderId);
        }
    }
}