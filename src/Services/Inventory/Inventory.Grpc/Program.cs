/*using Inventory.Grpc.Services;*/

using Common.Logging;
using Inventory.Grpc.Extensions;
using Inventory.Grpc.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    Log.Information("Start {EnvironmentApplicationName} up", builder.Environment.ApplicationName);
    
    // Add services to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureMongoDbClient();
    builder.Services.AddInfrastructureServices();
    
    // Additional configuration is required to successfully run gRPC on macOS.
    // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
    builder.Services.AddGrpc();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    /*app.MapGrpcService<GreeterService>();*/
    app.MapGrpcService<InventoryService>();
    
    app.MapGet("/",
        () =>
            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Unhandled exception: {EMessage}", e.Message);
}
finally
{
    Log.Information("Shut down {EnvironmentApplicationName} down", builder.Environment.ApplicationName);
    Log.CloseAndFlush();
}

