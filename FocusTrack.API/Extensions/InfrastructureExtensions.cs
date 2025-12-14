using FocusTrack.Infrastructure;


namespace FocusTrack.API.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddInfrastructure(config);
            return services;
        }
    }
}
