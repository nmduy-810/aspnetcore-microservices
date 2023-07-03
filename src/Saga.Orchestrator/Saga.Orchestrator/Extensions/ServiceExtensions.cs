using Common.Logging;
using Saga.Orchestrator.HttpRepositories;
using Saga.Orchestrator.HttpRepositories.Interfaces;
using Saga.Orchestrator.Services;
using Saga.Orchestrator.Services.Interfaces;

namespace Saga.Orchestrator.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services
            .AddTransient<ICheckoutSagaService, CheckoutSagaService>()
            .AddTransient<LoggingDelegatingHandler>();
        
        return services;
    }

    public static IServiceCollection ConfigureHttpRepository(this IServiceCollection services)
    {
        services.AddScoped<IOrderHttpRepository, OrderHttpRepository>()
            .AddScoped<IBasketHttpRepository, BasketHttpRepository>()
            .AddScoped<IInventoryHttpRepository, InventoryHttpRepository>();

        return services;
    }

    public static void ConfigureHttpClients(this IServiceCollection services)
    {
        ConfigureOrderHttpClient(services);
        ConfigureBasketHttpClient(services);
        ConfigureInventoryHttpClient(services);
    }

    private static void ConfigureOrderHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IOrderHttpRepository, OrderHttpRepository>("OrdersAPI", (sp, cl) =>
        {
            cl.BaseAddress = new Uri("http://localhost:5005/api/v1/");
        }).AddHttpMessageHandler<LoggingDelegatingHandler>();
        
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
            .CreateClient("OrdersAPI"));
    }
    
    private static void ConfigureBasketHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IBasketHttpRepository, BasketHttpRepository>("BasketsAPI", (sp, cl) =>
        {
            cl.BaseAddress = new Uri("http://localhost:5004/api/");
        }).AddHttpMessageHandler<LoggingDelegatingHandler>();
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
            .CreateClient("BasketsAPI"));
    }
    
    private static void ConfigureInventoryHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<IInventoryHttpRepository, InventoryHttpRepository>("InventoryAPI", (sp, cl) =>
        {
            cl.BaseAddress = new Uri("http://localhost:5006/api/");
        }).AddHttpMessageHandler<LoggingDelegatingHandler>();
        
        services.AddScoped(sp => sp.GetService<IHttpClientFactory>()
            .CreateClient("InventoryAPI"));
    }
}