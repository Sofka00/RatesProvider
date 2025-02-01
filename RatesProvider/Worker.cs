using RatesProvider.Application.Interfaces;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICurrencyApiService _currencyApiService;
    private readonly IOpenExchangeRatesService _openExchangeRatesService;
    private readonly string _baseCurrency = "USD"; //  базовую валюту

    public Worker(ILogger<Worker> logger,
        IOpenExchangeRatesService openExchangeRatesService,
        ICurrencyApiService currencyApiService)
    {
        _logger = logger;
        _openExchangeRatesService = openExchangeRatesService;
        _currencyApiService = currencyApiService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("RatesProviderService running at: {time}", DateTimeOffset.Now);
            await _openExchangeRatesService.GetCurrencyRateWithTypedClientAsync();


            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}
