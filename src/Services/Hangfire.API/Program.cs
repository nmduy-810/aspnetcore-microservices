using Common.Logging;
using Hangfire.API.Extensions;
using Infrastructure.ScheduledJobs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Start {ApplicationName} up", builder.Environment.ApplicationName);

try
{
    // Add services to the container.
    builder.Host.UseSerilog(Serilogger.Configure);

    builder.Services.AddConfigurationSettings(builder.Configuration);

    builder.Services.ConfigureServices();

    builder.Host.AddAppConfigurations();

    builder.Services.AddHangFireServicesCore();
    
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));
    }

    app.UseRouting();

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    // here (after app.UseAuthorization)
    app.UseHangfireDashboard(builder.Configuration);

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapDefaultControllerRoute();
    });

    app.Run();
}
catch (Exception e)
{
    var type = e.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
    Log.Fatal(e, "Unhandled exception: {EMessage}", e.Message);
}
finally
{
    Log.Information("Shut down {ApplicationName} API complete", builder.Environment.ApplicationName);
    Log.CloseAndFlush();
}