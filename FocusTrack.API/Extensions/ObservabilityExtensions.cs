using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FocusTrack.API.Extensions
{
    public static class ObservabilityExtensions
    {
        public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration config)
        {
            services.AddOpenTelemetry()
                .WithTracing(tracer =>
                {
                    tracer.SetResourceBuilder(
                        ResourceBuilder.CreateDefault().AddService("FocusTrack.Api"))
                           .AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddEntityFrameworkCoreInstrumentation()
                           .AddJaegerExporter();
                });

            services.AddMetrics();

            return services;
        }
    }
}
