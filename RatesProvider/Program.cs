using RatesProvider.Application.Configuration;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

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

builder.Services.Configure<OpenExchangeRatesClientSettings>(builder.Configuration.GetSection("OpenExchangeClient"));
builder.Services.Configure<CurrencyApiClientSettings>(builder.Configuration.GetSection("CurrencyApiClient"));
builder.Services.Configure<FixerClientSettings>(builder.Configuration.GetSection("FixerClient"));
builder.Services.Configure<CommonHttpClientSettings>(builder.Configuration.GetSection("CommonHttpClient"));
builder.Services.AddHostedService<Worker>();
builder.Services.AddApplicationServices();

var host = builder.Build();

try
{
    Log.Information("Starting up the service...");
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during startup.");
}
finally
{
    Log.CloseAndFlush();
}