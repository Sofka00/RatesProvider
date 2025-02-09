using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using RatesProvider.Application.Configuration;


var builder = Host.CreateApplicationBuilder(args);

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "RatesProviderService";
});
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Регистрация настроек ApiSettings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
Console.WriteLine(builder.Configuration.GetDebugView());

var configuration = builder.Configuration;

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);
builder.Services.AddHostedService<Worker>();
builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));
builder.Services.AddApplicationServices();


var host = builder.Build();
host.Run();
