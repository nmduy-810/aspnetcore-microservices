using AutoMapper;
using Common.Logging;
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
    app.MapGet("/", 
        () => "Welcome to Customer API");
    
    app.MapGet("/api/customers", 
        async (ICustomerService customerService) => await customerService.GetCustomersAsync());

    app.MapGet("/api/customers/{username}",
        async (string username, ICustomerService customerServicec) =>
        {
            var customer = await customerServicec.GetCustomerByUserNameAsync(username);
            return Results.Ok(customer);
        });

    // Have change ICustomerRepository to ICustomerService (using CatalogCustomer entity)
    app.MapPost("/api/customers/", async (CatalogCustomer customer, ICustomerRepository customerRepository) =>
    {
        await customerRepository.CreateAsync(customer);
        await customerRepository.SaveChangesAsync();
    });

    app.MapDelete("/api/customers/{id}", async (int id, ICustomerRepository customerRepository) =>
    {
        var customer = await customerRepository.FindByCondition(x => x.Id.Equals(id)).SingleOrDefaultAsync();
        if (customer == null) return Results.NotFound();

        await customerRepository.DeleteAsync(customer);
        await customerRepository.SaveChangesAsync();

        return Results.NoContent();
    });

    // Using UpdateCustomer dto
    app.MapPut("/api/customers/{id}",
        async (int id, UpdateCustomerDto customerDto, ICustomerRepository customerRepository, IMapper mapper) =>
        {
            var customer = await customerRepository.FindByCondition(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            if (customer == null) return Results.NotFound();

            var updateCustomer = mapper.Map(customerDto, customer);
            await customerRepository.UpdateAsync(updateCustomer);
            await customerRepository.SaveChangesAsync();
            var result = mapper.Map<CustomerDto>(customer);
            return Results.Ok(result);
        });

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