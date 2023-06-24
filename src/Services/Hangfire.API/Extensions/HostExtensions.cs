using Shared.Configurations;

namespace Hangfire.API.Extensions;

public static class HostExtensions
{
    public static void AddAppConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        });
    }

    public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
    {
        var configDashboard = configuration.GetSection("HangFireSettings:Dashboard").Get<DashboardOptions>();
        var hangfireSettings = configuration.GetSection("HangFireSettings").Get<HangFireSettings>();
        var hangfireRoute = hangfireSettings.Route;

        app.UseHangfireDashboard(hangfireRoute, new DashboardOptions
        {
            // if tren moi truong docker ma khong khai bao Authorization se khong mo duoc dashboard
            Authorization = new [] { new AuthorizationFilter() } ,
           DashboardTitle = configDashboard.DashboardTitle,
           StatsPollingInterval = configDashboard.StatsPollingInterval,
           AppPath = configDashboard.AppPath,
           IgnoreAntiforgeryToken = true
        });
        
        return app;
    }
}