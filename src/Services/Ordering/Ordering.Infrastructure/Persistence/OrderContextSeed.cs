using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    private readonly OrderContext _context;

    public OrderContextSeed(OrderContext context)
    {
        _context = context;
    }
    
    public async Task InitialiseAsync()
    {
        try
        {
            // Check have sql server database
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.Orders.Any())
        {
            await _context.Orders.AddRangeAsync(
                new Order
                {
                    UserName = "customer1", FirstName = "customer1", LastName = "customer",
                    EmailAddress = "customer1@local.com",
                    ShippingAddress = "82 Vo Van Ngan", InvoiceAddress = "Vietnam", TotalPrice = 250
                });
        }
    }
}