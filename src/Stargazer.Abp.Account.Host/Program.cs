using Scalar.AspNetCore;
using Microsoft.AspNetCore.OpenApi;
using Serilog;
using Stargazer.Abp.Account.Host;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseAutofac().UseSerilog();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Async(c =>
        c.File("Logs/log.txt",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 31,
            rollOnFileSizeLimit: true,
            fileSizeLimitBytes: 31457280,
            buffered: true))
    .WriteTo.Console()
    .CreateLogger();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
try
{
    // Add services to the container.
    builder.Services.ReplaceConfiguration(builder.Configuration);
    builder.Services.AddApplication<StargazerAbpAccountHostModule>();
    builder.Services.AddOpenApi();
    var app = builder.Build();
    app.InitializeApplication();
    if (!app.Environment.IsProduction())
    {
        app.MapScalarApiReference();
        app.MapOpenApi();
    }
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
