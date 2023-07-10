using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace Infrastructure.Policies;

public static class HttpClientRetryPolicy
{
    
    public static IHttpClientBuilder UseImmediateHttpRetryPolicy(this IHttpClientBuilder builder)
    {
        return builder.AddPolicyHandler(ConfigureImmediateHttpRetry());
    }
    
    public static IHttpClientBuilder UseLinearHttpRetryPolicy(this IHttpClientBuilder builder)
    {
        return builder.AddPolicyHandler(ConfigureLinearHttpRetry());
    }
    
    public static IHttpClientBuilder UseExponentialHttpRetryPolicy(this IHttpClientBuilder builder)
    {
        return builder.AddPolicyHandler(ConfigureExponentialHttpRetry());
    }
    
    public static IHttpClientBuilder UseCircuitBreakerPolicy(this IHttpClientBuilder builder)
    {
        return builder.AddPolicyHandler(ConfigCircuitBreakerPolicy());
    }

    private static IAsyncPolicy<HttpResponseMessage> ConfigureImmediateHttpRetry() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .RetryAsync(3, (exception, retryCount, context) =>
            {
                Log.Error("Retry {RetryCount} of {PolicyKey} at {OperationKey}, due to: {Message}", 
                    retryCount, context.PolicyKey, context.OperationKey, exception.Exception.Message);
            });
    
    private static IAsyncPolicy<HttpResponseMessage> ConfigureLinearHttpRetry() =>
        HttpPolicyExtensions
            .HandleTransientHttpError().WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(3),
                (exception, retryCount, context) =>
                {
                    Log.Error("Retry {RetryCount} of {PolicyKey} at {OperationKey}, due to: {Message}", 
                        retryCount, context.PolicyKey, context.OperationKey, exception.Exception.Message);
                });
    
    private static IAsyncPolicy<HttpResponseMessage> ConfigureExponentialHttpRetry() =>
        HttpPolicyExtensions
            .HandleTransientHttpError().WaitAndRetryAsync(3, retryAttempt 
                    => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2 ^ 1 = 2s, 2 ^ 2 = 4,....
                (exception, retryCount, context) =>
                {
                    Log.Error("Retry {RetryCount} of {PolicyKey} at {OperationKey}, due to: {Message}", 
                        retryCount, context.PolicyKey, context.OperationKey, exception.Exception.Message);
                });

    private static IAsyncPolicy<HttpResponseMessage> ConfigCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromSeconds(30));
    }
}