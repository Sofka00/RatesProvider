using RatesProvider.Application.Services;


public class Worker : BackgroundService

{
    private readonly ILogger<Worker> _logger;
    private readonly CurrencyProvider1 _currencyProvider;
    private readonly string _baseCurrency = "USD"; //  базовую валюту

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<CurrencyProvider1>(); // или другой подходящий жизненный цикл
        services.AddHostedService<Worker>();
    }

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _currencyProvider = new CurrencyProvider1("cur_live_IiK9hxawHQROFiX3rUYiEMzPzQ6oa12EmOqDrSiN");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while(!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("RatesProviderService running at: {time}", DateTimeOffset.Now);
            await _currencyProvider.GetRatesAsync(_baseCurrency);
            await Task.Delay(TimeSpan.FromMinutes(1));
        }

        
    }
}
