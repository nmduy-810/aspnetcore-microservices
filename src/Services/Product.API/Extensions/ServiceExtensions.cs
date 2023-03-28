using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;

namespace Product.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.ConfigureProductDbContext(configuration); // Config database
        
        return services;
    }

    private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString"); // Get connection string in application.json
        var builder = new MySqlConnectionStringBuilder(connectionString); // Create builder use build connection

        services.AddDbContext<ProductContext>(options => options.UseMySql(builder.ConnectionString,
            ServerVersion.AutoDetect(builder.ConnectionString), e =>
            {
                e.MigrationsAssembly("Product.API"); // Migration project name
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));

        return services;
    }
}