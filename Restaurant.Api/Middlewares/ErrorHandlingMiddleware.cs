
using Domain.Exceptions;
using System.Net;
using System.Text.Json;

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
        
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // Xatoni private metodda qayta ishlaymiz
        }
    }
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        string message = "Internal Server Error";

        // Xatolik turiga qarab status va message belgilash
        switch (exception)
        {
            case NotFoundException: // Masalan, resurs topilmasa
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case UnauthorizedAccessException: // Ruxsat yo'q
                statusCode = HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;

            case BadRequestException: // Notog'ri argument
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case ServerErrorException: // Notog'ri argument
                statusCode = HttpStatusCode.InternalServerError;
                message = exception.Message;
                break;

            default: // Boshqa barcha xatolar
                statusCode = HttpStatusCode.InternalServerError;
                message = exception.Message; // productionda umumiy xabar berish tavsiya qilinadi
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = message,
            StackTrace = exception.StackTrace
        };

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}

