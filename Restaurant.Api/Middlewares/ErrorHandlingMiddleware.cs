
using Domain.Exceptions;

namespace Restaurants.Api.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch(NotFoundException nfEx)
        {
            _logger.LogWarning(nfEx, "A not found exception has occurred: {Message}", nfEx.Message);
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";
            var response = new { error = nfEx.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var response = new { error = "An unexpected error occurred. Please try again later." };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
