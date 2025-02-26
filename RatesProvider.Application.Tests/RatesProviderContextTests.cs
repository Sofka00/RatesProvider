using Microsoft.Extensions.Logging;
using Moq;
using RatesProvider.Application.Exeptions;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Services;

namespace RatesProvider.Application.Tests;

public class RatesProviderContextTests
{
    private readonly Mock<ILogger<RatesProviderContext>> _logger;
    private readonly Mock<ICurrencyRateProvider> _currencyRateProvider;

    public RatesProviderContextTests()
    {
        _logger = new Mock<ILogger<RatesProviderContext>>();
        _currencyRateProvider = new Mock<ICurrencyRateProvider>();
    }

    [Fact]
    public void SetCurrencyRatesProvider_NullProvider_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new RatesProviderContext(_logger.Object);

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => context.SetCurrencyRatesProvider(null));
    }

    [Fact]
    public async Task GetRatesAsync_NoProviderSet_ThrowsInvalidOperationException()
    {
        // Arrange
        var context = new RatesProviderContext(_logger.Object);

        // Act and Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => context.GetRatesAsync());
    }

    [Fact]
    public async Task GetRatesAsync_ProviderFailsAllAttempts_ThrowsClientAttemptsExceededException()
    {
        // Arrange
        var context = new RatesProviderContext(_logger.Object);
        context.SetCurrencyRatesProvider(_currencyRateProvider.Object);
        _currencyRateProvider.SetupSequence(p => p.GetCurrencyRatesAsync())
            .Throws<Exception>()
            .Throws<Exception>()
            .Throws<Exception>();

        // Act and Assert
        await Assert.ThrowsAsync<ClientAttemptsExceededException>(() => context.GetRatesAsync());
    }
}