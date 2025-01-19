using RatesProvider.Application.Interfaces;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICurrencyApiService _currencyProviderService;
    private readonly string _baseCurrency = "USD"; //  базовую валюту

    public Worker(ILogger<Worker> logger, ICurrencyApiService currencyProviderService)
    {
        _logger = logger;
        _currencyProviderService = currencyProviderService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("RatesProviderService running at: {time}", DateTimeOffset.Now);
            await _currencyProviderService.GetCurrencyRateWithTypedClientAsync();
            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}
