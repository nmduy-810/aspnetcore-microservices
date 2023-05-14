using Infrastructure.Extensions;
using Inventory.Grpc.Repositories;
using Inventory.Grpc.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Grpc.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IInventoryRepository, InventoryRepository>();
    }
    
    public static void AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        services.AddSingleton(settings);
    }

    private static string GetMongoConnectionString(this IServiceCollection services)
    {
        var settings = services.GetOptions<MongoDbSettings>(nameof(MongoDbSettings));
        if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
            throw new ArgumentNullException($"DatabaseSettings is not configured");

        var databaseName = settings.DatabaseName;
        var mongoDbConnectionString = settings.ConnectionString + "/" + databaseName + "?authSource=admin";
        return mongoDbConnectionString;
    }

    public static void ConfigureMongoDbClient(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(
                new MongoClient(GetMongoConnectionString(services)))
            .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
    }
}