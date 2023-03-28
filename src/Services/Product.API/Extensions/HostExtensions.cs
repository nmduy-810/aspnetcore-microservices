using Microsoft.EntityFrameworkCore;

namespace Product.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
            logger.LogInformation("Migrating MySQL database.");
            ExcuteMigrations(context);
            
            logger.LogInformation("Migrated MySQL database.");
            InvokeSeeder(seeder!, context, services);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while migrating the MySQL database.");
        }

        return host;
    }

    private static void ExcuteMigrations<TContext>(TContext context) where TContext : DbContext?
    {
        context?.Database.Migrate();
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services) where TContext : DbContext?
    {
        seeder(context, services);
    }
}