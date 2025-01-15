using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using RatesProvider.Application.Configuration;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "RatesProviderService";
});

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);
builder.Services.AddHostedService<Worker>();
builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));

builder.Services.AddApplicationServices();

var host = builder.Build(); 
host.Run();
