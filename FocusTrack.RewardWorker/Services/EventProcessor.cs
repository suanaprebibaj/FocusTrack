using FocusTrack.Infrastructure.Persistence;
using FocusTrack.RewardWorker.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FocusTrack.RewardWorker.Services
{
    public class EventProcessor : IDisposable, IAsyncDisposable
    {
        private readonly AppDbContext _db;
        private readonly RewardCalculator _calc;
        private readonly IConnection _publishConnection;
        private readonly IChannel _publishChannel;
        private readonly string _exchange = "focustrack.events";
        private readonly ILogger<EventProcessor> _logger; 

        public EventProcessor(AppDbContext db, RewardCalculator calc, ILogger<EventProcessor> logger)
        {
            _db = db;
            _calc = calc;
            _logger = logger;

            var factory = new ConnectionFactory { HostName = "rabbitmq" };

            
            _publishConnection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _publishChannel = _publishConnection.CreateChannelAsync().GetAwaiter().GetResult();

            _publishChannel.ExchangeDeclareAsync(
                exchange: _exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null).GetAwaiter().GetResult();

            _logger.LogInformation("EventProcessor initialized with RabbitMQ connection");
        }

        public async Task ProcessEventAsync(string routingKey, string json, CancellationToken ct)
        {
            if (routingKey.EndsWith("SessionCreatedDomainEvent"))
            {
                var evt = JsonSerializer.Deserialize<SessionCreatedIntegrationEvent>(json)!;
                await HandleSessionChanged(evt.UserId, evt.StartTime.Date, ct);
            }
            else if (routingKey.EndsWith("SessionUpdatedDomainEvent"))
            {
                var evt = JsonSerializer.Deserialize<SessionUpdatedIntegrationEvent>(json)!;
                await HandleSessionChanged(evt.UserId, evt.StartTime.Date, ct);
            }
        }

        private async Task HandleSessionChanged(Guid userId, DateTime date, CancellationToken ct)
        {
            var sessions = await _db.Sessions
                .Where(s => s.UserId == userId && s.StartTime.Date == date)
                .ToListAsync(ct);

            var total = sessions.Sum(s => s.Duration.Value);

            var previouslyTriggered = sessions.Any(s => s.IsDailyGoalAchieved);

            if (_calc.ShouldTriggerBadge(
                    previousTotal: previouslyTriggered ? 120 : total - sessions.Last().Duration.Value,
                    newTotal: total))
            {
                foreach (var session in sessions)
                {
                    session.MarkDailyGoalAchieved();
                }

                await _db.SaveChangesAsync(ct);

                await PublishDailyGoalAchieved(userId, date);
            }
        }

        private async Task PublishDailyGoalAchieved(Guid userId, DateTime date)
        {
            var evt = new DailyGoalAchievedIntegrationEvent
            {
                UserId = userId,
                Date = date
            };

            var json = JsonSerializer.Serialize(evt);
            var body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(json));

        
            var properties = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            
            await _publishChannel.BasicPublishAsync<BasicProperties>(
                exchange: _exchange,
                routingKey: "DailyGoalAchievedIntegrationEvent",
                mandatory: false,
                basicProperties: properties,
                body: body);

          
        }

        public void Dispose()
        {
           
            _publishChannel?.CloseAsync().GetAwaiter().GetResult();
            _publishChannel?.Dispose();
            _publishConnection?.CloseAsync().GetAwaiter().GetResult();
            _publishConnection?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_publishChannel != null)
            {
                await _publishChannel.CloseAsync();
                _publishChannel.Dispose();
            }

            if (_publishConnection != null)
            {
                await _publishConnection.CloseAsync();
                _publishConnection.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}