using Microsoft.Extensions.Options;
using Moq;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Models.FixerApiModels;
namespace RatesProvider.Tests

{
    public class UnitTest1
    {
        private readonly Mock<ICommonHttpClient> _mockHttpClient;
        private readonly FixerClient _fixerClient;
        private readonly Mock<IOptions<ApiSettings>> _mockOptions;

        public UnitTest1()
        {
            _mockHttpClient = new Mock<ICommonHttpClient>();
            _fixerClient = new FixerClient(_mockOptions.Object, _mockHttpClient.Object); ;
            _mockOptions = new Mock<IOptions<ApiSettings>>();
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_ShouldReturnCurrencyRateResponse_WhenResponseIsValid()
        {
            _mockHttpClient
                 .Setup(client => client.SendRequestAsync<FixerResponse>(It.IsAny<string>()))
                .ReturnsAsync(new FixerResponse
                {
                    Base = "EUR",
                    Rates = new Dictionary<string, decimal>
                    {
                        { "USD", 1.1m },
                        { "ARS",1109.8m},
                        { "BGN",1109.8m}
                    },
                    Date = DateTime.Now
                });

            _mockOptions
                .Setup(opt => opt.Value)
                .Returns(new ApiSettings { FixerApiKey = "valid-api-key" });

            var fixerClient = new FixerClient(_mockOptions.Object, _mockHttpClient.Object);

            // Act
            var result = await fixerClient.GetCurrencyRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("EUR", result.BaseCurrency.ToString());
            Assert.Contains("EURUSD", result.Rates.Keys);
            Assert.Equal(1.1m, result.Rates["EURUSD"]);
            Assert.Contains("EURARS", result.Rates.Keys);
            Assert.Equal(1109.8m, result.Rates["EURARS"]);
            Assert.Contains("EURBGN", result.Rates.Keys);
            Assert.Equal(1109.8m, result.Rates["EURBGN"]);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_ShouldReturnEmptyRates_WhenResponseIsInvalid()
        {


            _mockHttpClient
                .Setup(client => client.SendRequestAsync<FixerResponse>(It.IsAny<string>()))
                .ReturnsAsync(new FixerResponse
                {
                    Base = "EUR",
                    Rates = new Dictionary<string, decimal>(),
                    Date = DateTime.Now
                });

            _mockOptions
                .Setup(opt => opt.Value)
                .Returns(new ApiSettings { FixerApiKey = "valid-api-key" });

            var result = await _fixerClient.GetCurrencyRatesAsync();

            Assert.NotNull(result);
            Assert.Equal("EUR", result.BaseCurrency.ToString());
            Assert.Empty(result.Rates);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_ShouldThrowException_WhenApiFails()
        {

            _mockHttpClient
                .Setup(client => client.SendRequestAsync<FixerResponse>(It.IsAny<string>()))
                .ThrowsAsync(new Exception("API request failed"));

            _mockOptions
                .Setup(opt => opt.Value)
                .Returns(new ApiSettings { FixerApiKey = "valid-api-key" });

            var exception = await Assert.ThrowsAsync<Exception>(() => _fixerClient.GetCurrencyRatesAsync());
            Assert.Equal("API request failed", exception.Message);
        }
    }

}



