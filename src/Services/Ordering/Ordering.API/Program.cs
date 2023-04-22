using Common.Logging;
using Contracts.Common.Interfaces;
using Contracts.Messages;
using Infrastructure.Common;
using Infrastructure.Messages;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start Ordering API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplicationServices();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    // Initialise and seed Order database
    var scope = app.Services.CreateScope();
    var orderContextSeed = scope.ServiceProvider.GetRequiredService<OrderContextSeed>();
    await orderContextSeed.InitialiseAsync();
    await orderContextSeed.SeedAsync();

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    var type = e.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
        throw;
    
    Log.Fatal(e, "Unhandled exception: {EMessage}", e.Message);
}
finally
{
    Log.Information("Shut down Ordering API complete");
    Log.CloseAndFlush();
}
