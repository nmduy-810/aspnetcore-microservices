using AutoMapper;
using Common.Logging;
using Customer.API.Controllers;
using Customer.API.Entities;
using Customer.API.Extensions;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Intefaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.DTOs.Customer;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Starting Customer API up");

try
{
    // Add serilog
    builder.Host.UseSerilog(Serilogger.Configure);
    
    // Add host to the container from ConfigureHostExtensions.cs
    builder.Host.AddAppConfiguration();

    // Add services to the container from ServiceExtension.cs
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    
    // Map minimal apis
    app.MapCustomersApi();
    
    // Add app to the container from ApplicationExtensions.cs
    app.UseInfrastructure();

    app.SeedCustomerAsync()
        .Run();
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
    Log.Information("Shut down Customer API complete");
    Log.CloseAndFlush();
}