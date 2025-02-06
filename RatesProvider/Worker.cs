using RatesProvider.Application.Integrations;
using RatesProvider.Application.Interfaces;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICurrencyRateManager _currencyRateManager;
    public Worker(ILogger<Worker> logger, ICurrencyRateManager currencyRateManager)
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
            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}
