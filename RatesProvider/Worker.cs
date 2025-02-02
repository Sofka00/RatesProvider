using RatesProvider.Application.Interfaces;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICurrencyRateManager _currencyRateManager;
    private readonly string _baseCurrency = "USD"; //  базовую валюту

    public Worker(ILogger<Worker> logger,
        IOpenExchangeRatesService openExchangeRatesService,
        ICurrencyRateManager currencyRateManager)
    {
        _logger = logger;
        _currencyRateManager = currencyRateManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("RatesProviderService running at: {time}", DateTimeOffset.Now);
            await _currencyRateManager.GetRatesAsync();

            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
