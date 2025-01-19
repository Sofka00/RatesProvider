using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Integrations;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "RatesProviderService";
});

builder.Configuration
    .AddJsonFile("appsetings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsetings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsetings.secrets.json", optional: true, reloadOnChange: true)
    .AddCommandLine(args)
    .AddEnvironmentVariables()
    .Build();

var configuration = builder.Configuration;

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);
builder.Services.AddHostedService<Worker>();
builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));


builder.Services.AddHttpClient<CurrencyApiClient>();
builder.Services.AddHttpClient<CurrencyLayerClient>();
builder.Services.AddHttpClient<FixerClient>();
builder.Services.AddApplicationServices();


var host = builder.Build(); 
host.Run();
