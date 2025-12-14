using FocusTrack.Application.Common.Interfaces;
using FocusTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.Infrastructure.Messaging
{
    public class OutboxDispatcher : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<OutboxDispatcher> _logger;

        public OutboxDispatcher(IServiceProvider provider, ILogger<OutboxDispatcher> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OutboxDispatcher started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _provider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

                    var pending = await db.OutboxMessages
                        .Where(m => m.ProcessedOnUtc == null)
                        .OrderBy(m => m.OccurredOnUtc)
                        .Take(50)
                        .ToListAsync(stoppingToken);

                    if (pending.Count == 0)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                        continue;
                    }

                    foreach (var msg in pending)
                    {
                        try
                        {
                            await bus.PublishAsync(msg.Type, msg.Content, stoppingToken);
                            msg.MarkProcessed();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error publishing outbox message {MessageId}", msg.Id);
                            msg.MarkFailed(ex.Message);
                        }
                    }

                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // normal shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in OutboxDispatcher loop.");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            _logger.LogInformation("OutboxDispatcher stopped.");
        }
    }
}
