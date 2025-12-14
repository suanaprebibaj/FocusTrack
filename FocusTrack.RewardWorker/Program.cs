using FocusTrack.RewardWorker;
using FocusTrack.RewardWorker.Services;
using FocusTrack.Infrastructure.Persistence;
using FocusTrack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using FocusTrack.NotificationWorker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("Default"));
        });

        
        services.AddScoped<IDomainEventSerializer, DomainEventSerializer>();

        services.AddSingleton<RewardCalculator>();
        services.AddSingleton<RabbitMqEventSubscriber>();

      
        services.AddScoped<EventProcessor>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
