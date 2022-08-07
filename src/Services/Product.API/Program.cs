using Common.Logging;
using Product.API.Extensions;
using Product.API.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Starting Product API up");

try
{
    // Add serilog
    builder.Host.UseSerilog(Serilogger.Configure);

    // Add host to the container from ConfigureHostExtensions.cs
   builder.Host.AddAppConfigurations();
   
   // Add services to the container from ServiceExtension.cs
   builder.Services.AddInfrastructure(builder.Configuration); 

    var app = builder.Build();

    // Add app to the container from ApplicationExtensions.cs
    app.UseInfrastructure();
    
    // Automatic migration when run programs (after migrations, call seed class
    app.MigrateDatabase<ProductContext>((context, _) =>
        {
            ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
        }).Run();

    app.Run();
}
catch (Exception exception)
{
    var type = exception.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) // If migration fail, block fail
    {
        throw;
    }
    
    Log.Fatal(exception, "Unhandled exception: {ExceptionMessage}", exception.Message);
}
finally
{
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}