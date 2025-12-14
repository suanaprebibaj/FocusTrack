using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Infrastructure.Identity;
using FocusTrack.Infrastructure.Messaging;
using FocusTrack.Infrastructure.Persistence;
using FocusTrack.Infrastructure.Repositories;
using FocusTrack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("ConnectionStrings:Default is not configured.");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

        
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

            
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdminSessionReadRepository, AdminSessionReadRepository>();

         
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

          
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IPublicLinkService, PublicLinkService>();

         
            services.AddSingleton<IDomainEventSerializer, DomainEventSerializer>();

            // RabbitMQ
            //services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
            //services.AddSingleton<IMessageBus, RabbitMqMessageBus>();// In DependencyInjection.cs
            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
           

            services.AddSingleton<IMessageBus>(provider =>
            {
                
                var options = provider.GetRequiredService<IOptions<RabbitMqOptions>>();
                var logger = provider.GetRequiredService<ILogger<RabbitMqMessageBus>>();

             
                return RabbitMqMessageBus.CreateAsync(options, logger).GetAwaiter().GetResult();
            });


            
            services.AddHostedService<OutboxDispatcher>();

            return services;
        }
    }
}
