using FocusTrack.NotificationWorker.Services;
using FocusTrack.RewardWorker.Services;
using Microsoft.Extensions.Hosting;

namespace FocusTrack.RewardWorker;

public class Worker : BackgroundService
{
    private readonly RabbitMqEventSubscriber _subscriber;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Worker> _logger;

    public Worker(
        RabbitMqEventSubscriber subscriber,
        IServiceScopeFactory scopeFactory,
        ILogger<Worker> logger)
    {
        _subscriber = subscriber;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Reward worker started…");

        _subscriber.Subscribe(async (routingKey, json) =>
        {
        
            using var scope = _scopeFactory.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<EventProcessor>();

            await processor.ProcessEventAsync(routingKey, json, stoppingToken);
        });

        return Task.CompletedTask;
    }
}
