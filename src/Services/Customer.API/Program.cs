using Customer.API;
using Customer.API.Controllers;
using Customer.API.Extensions;
using Customer.API.Persistence;
using Customer.API.Services.Interfaces;
using Infrastructure.ScheduledJobs;
using Serilog;
using Shared.DTOs.Customer;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
Log.Information("Start {ApplicationName} up", builder.Environment.ApplicationName);

try
{
    builder.Host.AddAppConfigurations();

    // Add services to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile())); // Config auto mapper
    
    builder.Services.ConfigureCustomerContext();
    builder.Services.AddInfrastructureServices();
    builder.Services.AddHangFireServicesCore();
    
    var app = builder.Build();

    app.MapGet("/", () => "Welcome to Customer API!");

    /*app.MapGet("/api/customers", 
        async (ICustomerService customerService) => await customerService.GetCustomerAsync());

    app.MapGet("/api/customers/{id}",
        async (int id, ICustomerService customerService) => await customerService.GetCustomer(id));*/
    
    app.MapCustomersApi();

    // 1st code
    /*app.MapGet("/api/customers/{username}",
        async (string userName, ICustomerService customerService) => await customerService.GetCustomerByUserNameAsync(userName));*/
    
    // 2st code
    /*app.MapGet("/api/customers/{username}", 
        async (string username, ICustomerService customerService) => 
        { 
            var customer = await customerService.GetCustomerByUserNameAsync(username); 
            return customer != null ? Results.Ok(customer) : Results.NotFound(); 
        });*/

    app.MapPost("/api/customers", 
        async (CreateCustomerDto customerDto, ICustomerService customerService) =>
        {
            await customerService.CreateCustomer(customerDto);
        });

    app.MapPut("/api/customers/{id:int}", 
        async (int id, UpdateCustomerDto customerDto, ICustomerService customerService) => 
        { 
            await customerService.UpdateCustomer(id, customerDto);
        });

    app.MapDelete("/api/customers/{id:int}", 
        async (int id, ICustomerService customerService) =>
        {
            await customerService.DeleteCustomer(id);
        });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseHangfireDashboard(builder.Configuration);
    
    app.MapControllers();
    
    // Seed customer data
    app.SeedCustomerData().Run();
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
    Log.Information("Shut down Customer API complete");
    Log.CloseAndFlush();
}