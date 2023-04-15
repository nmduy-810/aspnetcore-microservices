using Customer.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence;

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
    {
        
    }

    public DbSet<CatalogCustomer> Customers { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CatalogCustomer>().HasIndex(x => x.UserName).IsUnique();
        modelBuilder.Entity<CatalogCustomer>().HasIndex(x => x.EmailAddress).IsUnique();
    }
}