using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Serilog;
using Shared.SeedWork;
using ValidationException = Infrastructure.Exceptions.ValidationException;

namespace Infrastructure.Middlewares;

public class ErrorWrappingMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    
    public ErrorWrappingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task Invoke(HttpContext context)
    {
        var errorMsg = string.Empty;
        try
        {
            await _next.Invoke(context);
        }
        catch (ValidationException ex)
        {
            _logger.Error(ex, "{Message}",ex.Message);
            errorMsg = ex.Errors.FirstOrDefault().Value.FirstOrDefault();
            context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Message}", ex.Message);
            errorMsg = ex.Message;
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        if (!context.Response.HasStarted && (context.Response.StatusCode == StatusCodes.Status403Forbidden ||
                                             context.Response.StatusCode == StatusCodes.Status401Unauthorized))
        {
            context.Response.ContentType = "application/json";

            var response = new ApiErrorResult<bool>("You are not authorized!");

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }

        if (!context.Response.HasStarted && context.Response.StatusCode != 204 &&
            context.Response.StatusCode != 202)
        {
            context.Response.ContentType = "application/json";

            if (errorMsg != null)
            {
                var response = new ApiErrorResult<bool>(errorMsg);

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}