using Contracts.ScheduleJobs;
using Contracts.Services;
using Hangfire.API.Services;
using Hangfire.API.Services.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.ScheduledJobs;
using Infrastructure.Services;
using Shared.Configurations;

namespace Hangfire.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var hangFireSettings = configuration.GetSection(nameof(HangFireSettings)).Get<HangFireSettings>();
        services.AddSingleton(hangFireSettings);
        
        var emailSettings = configuration.GetSection(nameof(SmtpEmailSettings)).Get<SmtpEmailSettings>();
        services.AddSingleton(emailSettings);
        
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IScheduledJobService, HangfireService>();
        services.AddScoped<ISmtpEmailService, SmtpEmailService>();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        return services;
    }
}