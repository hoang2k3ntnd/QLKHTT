using OnlineCourse.Data;
using OnlineCourse.Models.Entities;
using System.Diagnostics;

namespace OnlineCourse.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var log = new Log
                {
                    UserName = context.User.Identity?.IsAuthenticated == true
                        ? int.Parse(context.User.FindFirst("sub")?.Value ?? "0")
                        : null,
                    Action = $"{context.Request.Method} {context.Request.Path}",
                    StatusCode = context.Response.StatusCode,
                    HttpMethod = context.Request.Method,
                    RequestUrl = context.Request.Path,
                    IP = context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    ApplicationName = "OnlineCourse",
                    BrowserInfo = context.Request.Headers["User-Agent"].ToString(),
                    CreatedAt = DateTime.Now,
                    Duration = stopwatch.ElapsedMilliseconds
                };

                db.Logs.Add(log);
                await db.SaveChangesAsync();
            }
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
