using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using EventBus.Messages.IntegrationEvents.Interfaces;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Inventory.Grpc.Protos;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;

namespace Basket.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services) =>
        services.AddScoped<IBasketRepository, BasketRepository>()
            .AddTransient<ISerializeService, SerializeService>();

    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
        var cacheSettings = configuration.GetSection(nameof(CacheSettings)).Get<CacheSettings>();
        var grpcSettings = configuration.GetSection(nameof(GrpcSettings)).Get<GrpcSettings>();
        
        services.AddSingleton(eventBusSettings);
        services.AddSingleton(cacheSettings);
        services.AddSingleton(grpcSettings);
        return services;
    }
    
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.GetOptions<CacheSettings>("CacheSettings");
        if (string.IsNullOrEmpty(settings.ConnectionString))
            throw new ArgumentNullException($"Redis connection string is not configured");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = settings.ConnectionString;
        });
    }

    public static IServiceCollection ConfigureGrpcServices(this IServiceCollection services)
    {
        var settings = services.GetOptions<GrpcSettings>(nameof(GrpcSettings));
        services.AddGrpcClient<StockProtoService.StockProtoServiceClient>(x => x.Address = new Uri(settings.StockUrl));
        services.AddScoped<StockItemGrpcService>();
        return services;
    }

    public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
        if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
            throw new ArgumentNullException($"Event bus settings is not configured");

        var mqConnection = new Uri(settings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance); // Example: BasketCheckoutEventQueue => basket-checkout-event-queue
        services.AddMassTransit(cfg =>
        {
            cfg.UsingRabbitMq((ctx, config) =>
            {
              config.Host(mqConnection);  
            });
            cfg.AddRequestClient<IBasketCheckoutEvent>();
        });
    }
}