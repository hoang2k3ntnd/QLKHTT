using OnlineCourse.Interfaces;
using System.Diagnostics;

namespace OnlineCourse.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, ILogService logService)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var userName = context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anonymous";

            await logService.LogAction(
                action: $"{context.Request.Method} {context.Request.Path}",
                details: null,

                statusCode: context.Response.StatusCode
            );
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
            => builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
