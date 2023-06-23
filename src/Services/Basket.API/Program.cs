using Basket.API;
using Basket.API.Extensions;
using Common.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start {ApplicationName} up", builder.Environment.ApplicationName);

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureHttpClientServices();
    
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    
    // Add services to the container.
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
    
    // Configure Mass Transit
    builder.Services.ConfigureMassTransit(builder.Configuration);
    
    builder.Services.ConfigureServices();
    builder.Services.ConfigureRedis(builder.Configuration);

    // Config grpc services
    builder.Services.ConfigureGrpcServices();
    
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

    /*app.UseHttpsRedirection();*/

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
    Log.Information("Shut down {ApplicationName} API complete", builder.Environment.ApplicationName);
    Log.CloseAndFlush();
}