using System.Reflection;
using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var modified = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added or EntityState.Deleted);

        foreach (var item in modified)
        {
            switch (item.State)
            {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                case EntityState.Added:
                    if (item.Entity is IDateTracking addEntity)    
                    {
                        addEntity.CreatedDate = DateTimeOffset.UtcNow;
                        item.State = EntityState.Added;
                    }
                    break;
                case EntityState.Modified:
                    Entry(item.Entity).Property("Id").IsModified = false; // Disable modified id value
                    if (item.Entity is IDateTracking modifiedEntity)
                    {
                        modifiedEntity.LastModifiedDate = DateTimeOffset.UtcNow;
                        item.State = EntityState.Modified;
                    }
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}