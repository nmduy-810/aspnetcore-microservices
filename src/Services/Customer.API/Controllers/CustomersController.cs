using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers;

public static class CustomersController
{
    public static void MapCustomersApi(this WebApplication app)
    {
        app.MapGet("/api/customers/{username}",
            async (string username, ICustomerService customerService) =>
            {
                var result = await customerService.GetCustomerByUserNameAsync(username);
                return result ?? Results.NotFound();
            });
    }
}