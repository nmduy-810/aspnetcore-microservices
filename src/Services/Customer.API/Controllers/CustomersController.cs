using AutoMapper;
using Customer.API.Entities;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Intefaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Customer;

namespace Customer.API.Controllers;

public static class CustomersController
{
   public static void MapCustomersApi(this WebApplication app)
   {
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
      /*app.MapPost("/api/customers/", async (CatalogCustomer customer, ICustomerRepository customerRepository) =>
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
         });*/
   }
}