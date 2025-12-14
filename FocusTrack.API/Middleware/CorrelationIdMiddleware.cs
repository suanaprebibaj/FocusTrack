using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FocusTrack.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(HeaderName, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Append(HeaderName, correlationId);
            }

            context.TraceIdentifier = correlationId!;

            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append(HeaderName, correlationId!);
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
