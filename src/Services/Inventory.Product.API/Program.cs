using Common.Logging;
using Inventory.Product.API.Extensions;
using Serilog;

Log.Information("Start Inventory API up");

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Host.UseSerilog(Serilogger.Configure);

    // Add services to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);
    
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    
    builder.Services.AddInfrastructureServices();
    
    builder.Services.ConfigureMongoDbClient(); // Config mongo Db

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection(); // for production only

    app.UseAuthorization();

    app.MapDefaultControllerRoute();

    app.MigrateDatabase().Run();
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
    Log.Information("Shut down Inventory API complete");
    Log.CloseAndFlush();
}

