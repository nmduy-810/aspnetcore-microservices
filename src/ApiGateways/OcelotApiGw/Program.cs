using Common.Logging;
using Infrastructure.Middlewares;
using Ocelot.Middleware;
using OcelotApiGw.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Start {ApplicationName} up", builder.Environment.ApplicationName);

try
{
    // Add services to the container.
    builder.Host.UseSerilog(Serilogger.Configure);
    
    builder.Host.AddAppConfigurations();
    
    builder.Services.AddConfigurationSettings(builder.Configuration);
    
    // Config authentication
    builder.Services.AddJwtAuthentication();

    builder.Services.AddInfrastructureServices();
    
    builder.Services.AddControllers();
    
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.ConfigureOcelot(builder.Configuration);
    builder.Services.ConfigureCors(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", 
            $"{builder.Environment.ApplicationName} v1"));
    }

    app.UseCors("CorsPolicy");
    app.UseMiddleware<ErrorWrappingMiddleware>();

    app.UseAuthentication();

    app.UseRouting();

    /*app.UseHttpsRedirection();*/

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World");
        });
    });

    app.MapControllers();

    await app.UseOcelot();

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
    Log.Information("Shut down {ApplicationName} complete", builder.Environment.ApplicationName);
    Log.CloseAndFlush();
}