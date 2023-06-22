using Contracts.ScheduleJobs;
using Infrastructure.ScheduledJobs;
using Shared.Configurations;

namespace Hangfire.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var hangFireSettings = configuration.GetSection(nameof(HangFireSettings)).Get<HangFireSettings>();
        services.AddSingleton(hangFireSettings);
        
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IScheduledJobService, HangfireService>();
        return services;
    }
}