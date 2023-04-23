using EventBus.Messages.IntegrationEvents.Events;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Application.IntegrationEvents.EventsHandler;
using Shared.Configurations;

namespace Ordering.API.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection(nameof(SmtpEmailSetting)).Get<SmtpEmailSetting>();
        services.AddSingleton(emailSettings); // duy tri xuyen suot toan bo ung dung

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
            cfg.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();
            cfg.UsingRabbitMq((ctx, config) =>
            {
                config.Host(mqConnection);
                /*config.ReceiveEndpoint("basket-checkout-queue", c =>
                {
                    c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                });*/
                
                config.ConfigureEndpoints(ctx); // Any 
            });

        });
    }
}