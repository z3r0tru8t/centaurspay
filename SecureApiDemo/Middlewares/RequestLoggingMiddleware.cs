using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SecureApiDemo.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anonim";

            var path = context.Request.Path;
            var method = context.Request.Method;

            _logger.LogInformation($"📥 [{DateTime.UtcNow}] {user} -> {method} {path}");

            await _next(context);
        }
    }
}
