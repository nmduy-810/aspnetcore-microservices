using Basket.API.Extensions;
using Common.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start {ApplicationName} with {EnvironmentName} environment up", 
    builder.Environment.ApplicationName, builder.Environment.EnvironmentName);

try
{
    // Add serilog
    builder.Host.UseSerilog(Serilogger.Configure);
    
    // Add host to the container from ConfigureHostExtensions.cs
    builder.Host.AddAppConfiguration();

    // Add services to the container from ServiceExtensions.cs
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    // Add app to the container from ApplicationExtensions.cs
    app.UseInfrastructure();
    
    app.Run();
}
catch (Exception exception)
{
    var type = exception.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
        throw;
    
    Log.Fatal(exception, "Unhandled exception: {ExceptionMessage}", exception.Message);
}
finally
{
    Log.Information("Shut down Basket API complete");
    Log.CloseAndFlush();
}