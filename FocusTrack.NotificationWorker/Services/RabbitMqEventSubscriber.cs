using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FocusTrack.NotificationWorker.Services
{
    public class RabbitMqEventSubscriber : IAsyncDisposable, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly string _exchange = "focustrack.events";

        public RabbitMqEventSubscriber(IConfiguration config)
        {
            var host = config["RabbitMq:HostName"] ?? "localhost";

            var factory = new ConnectionFactory
            {
                HostName = host
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.ExchangeDeclareAsync(
                exchange: _exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false).GetAwaiter().GetResult();
        }

        public void Subscribe(Func<string, string, Task> handler)
        {
            var queueResult = _channel.QueueDeclareAsync(
                queue: "",
                durable: false,
                exclusive: true,
                autoDelete: true).GetAwaiter().GetResult();

            var queue = queueResult.QueueName;

            _channel.QueueBindAsync(
                queue: queue,
                exchange: _exchange,
                routingKey: "#").GetAwaiter().GetResult();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.Span);
                var routingKey = ea.RoutingKey;

                await handler(routingKey, json);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            _channel.BasicConsumeAsync(
                queue: queue,
                autoAck: false,
                consumer: consumer).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _channel?.CloseAsync().GetAwaiter().GetResult();
            _channel?.Dispose();
            _connection?.CloseAsync().GetAwaiter().GetResult();
            _connection?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
        }
    }
}
