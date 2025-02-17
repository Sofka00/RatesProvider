using Microsoft.Extensions.Options;
using Moq;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;
using RatesProvider.Application.Models;
using RatesProvider.Application.Models.CurrencyApiModels;

namespace RatesProvider.Application.Tests
{
    public class CurrencyApiClientTests
    {
        private Mock<ICommonHttpClient> _commonHttpClient;
        private Mock<IOptions<CurrencyApiClientSettings>> _currencyApiSettings;
        private CurrencyApiClient _sut;

        public CurrencyApiClientTests()
        {
            _currencyApiSettings = new Mock<IOptions<CurrencyApiClientSettings>>();
            _currencyApiSettings.Setup(o => o.Value).Returns(new CurrencyApiClientSettings()
            { ApiKey = "cur_live_IiK9hxawHQROFiX3rUYiEMzPzQ6oa12EmOqDrSiN", BaseUrl = "https://openexchangerates.org/api", QueryOption = "/latest?apikey=" });
            _commonHttpClient = new Mock<ICommonHttpClient>();
            _sut = new(_currencyApiSettings.Object, _commonHttpClient.Object);
        }


        [Fact]
        public async Task GetCurrencyRatesAsync()
        {
            //range
            var url = $"https://openexchangerates.org/api/latest?apikey=cur_live_IiK9hxawHQROFiX3rUYiEMzPzQ6oa12EmOqDrSiN";
            var currencyResponse = new CurrencyResponse()
            { Data = new Data() { RUB = new CurrencyValue() { Code = "RUB", Value = 100 } }, Meta = new Meta() { LastUpdatedAt = DateTime.Now } };
            var currencyRateResponse = new CurrencyRateResponse();
            //{ BaseCurrency = BaseCurrency.USD, Rates = new Dictionary<string, decimal>(), Date = DateTime.Now };
            //var currencyRate = ConvertToCurrencyRateResponse(response, BaseCurrency.USD);
            _commonHttpClient.Setup(t => t.SendRequestAsync<CurrencyResponse>(url)).ReturnsAsync(currencyResponse);
            //_commonHttpClient.Setup(t => t.ConvertToCurrencyRateResponse(currencyResponse, BaseCurrency.USD)).ReturnsAsync(currencyResponse);

            //act

            var response = await _sut.GetCurrencyRatesAsync();



            //assert

        }
    }
}