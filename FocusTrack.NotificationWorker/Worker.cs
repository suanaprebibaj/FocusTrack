using FocusTrack.NotificationWorker.Services;
using Microsoft.Extensions.Hosting;

namespace FocusTrack.NotificationWorker;

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
        _logger.LogInformation("Notification worker started…");

        _subscriber.Subscribe(async (routingKey, json) =>
        {
            using var scope = _scopeFactory.CreateScope();

            var processor = scope.ServiceProvider.GetRequiredService<NotificationProcessor>();

            await processor.ProcessAsync(routingKey, json);
        });

        return Task.CompletedTask;
    }
}
