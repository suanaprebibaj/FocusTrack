using FocusTrack.Application;


namespace FocusTrack.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddApplication();
            return services;
        }
    }
}
