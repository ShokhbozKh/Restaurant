
using System.Diagnostics;

namespace Restaurants.Api.Middlewares
{
    public class RequestTimeLoggingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        public RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            await next.Invoke(context);
            stopwatch.Stop();
            if(stopwatch.ElapsedMilliseconds / 1000 > 4)
            {
                _logger.LogInformation("Request [{Method}] at [{Path}] took {ElapsedSeconds} seconds",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds/1000);
            }
        }
    }
}
