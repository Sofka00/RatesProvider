using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using RatesProvider.Application;
using RatesProvider.Application.Configuration;
using RatesProvider.Application.Integrations;
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

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
Console.WriteLine(builder.Configuration.GetDebugView());
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



//IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureServices((hostContext, services) =>
//                {
//                    services.AddLogging(builder =>
//                        builder.AddSerilog());
//                    services.AddHostedService<Worker>();
//                });



//Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
//builder.Services.AddWindowsService(options =>
//{
//    options.ServiceName = "RatesProviderService";
//});
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
//builder.Configuration.AddEnvironmentVariables();
//if (builder.Environment.IsDevelopment())
//{
//    builder.Configuration.AddUserSecrets<Program>();
//}

//// Регистрация настроек ApiSettings
//builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
//Console.WriteLine(builder.Configuration.GetDebugView());

//var configuration = builder.Configuration;

//LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);
//builder.Services.AddHostedService<Worker>();
//builder.Logging.AddConfiguration(
//    builder.Configuration.GetSection("Logging"));
//builder.Services.AddApplicationServices();


//var host = builder.Build();
//host.Run();

