using FocusTrack.NotificationWorker.Persistence;
using FocusTrack.NotificationWorker.Services;
using FocusTrack.Infrastructure.Services;   
using Microsoft.EntityFrameworkCore;
using FocusTrack.NotificationWorker;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<NotificationDbContext>(opt =>
            opt.UseNpgsql(ctx.Configuration.GetConnectionString("Default")));

        services.AddScoped<IDomainEventSerializer, DomainEventSerializer>();

        services.AddSingleton<UserPresenceTracker>();
        services.AddSingleton<RealTimeNotifier>();
        services.AddSingleton<EmailNotifier>();
        services.AddSingleton<RabbitMqEventSubscriber>();

        services.AddScoped<NotificationProcessor>();

        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();
