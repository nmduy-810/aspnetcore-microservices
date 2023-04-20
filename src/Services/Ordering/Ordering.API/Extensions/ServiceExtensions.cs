using Infrastructure.Configurations;

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
}