using FocusTrack.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
namespace FocusTrack.Infrastructure.Messaging
{
    public class RabbitMqMessageBus : IMessageBus, IAsyncDisposable, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly RabbitMqOptions _options;
        private readonly ILogger<RabbitMqMessageBus> _logger;

      
        private RabbitMqMessageBus(IConnection connection, IChannel channel, RabbitMqOptions options, ILogger<RabbitMqMessageBus> logger)
        {
            _connection = connection;
            _channel = channel;
            _options = options;
            _logger = logger;
        }

       
        public static async Task<RabbitMqMessageBus> CreateAsync(IOptions<RabbitMqOptions> options, ILogger<RabbitMqMessageBus> logger)
        {
            var opts = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = opts.HostName,
                Port = opts.Port,
                UserName = opts.UserName,
                Password = opts.Password
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: opts.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);

            logger.LogInformation("RabbitMQ connection established to {HostName}", opts.HostName);

            return new RabbitMqMessageBus(connection, channel, opts, logger);
        }

        public async Task PublishAsync(string type, string payload, CancellationToken ct = default)
        {
            var body = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(payload));

            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            await _channel.BasicPublishAsync(
                exchange: _options.ExchangeName,
                routingKey: type,
                mandatory: false,
                basicProperties: props,
                body: body);
        }

        
        public void Dispose()
        {
            _channel?.CloseAsync().GetAwaiter().GetResult();
            _channel?.Dispose();
            _connection?.CloseAsync().GetAwaiter().GetResult();
            _connection?.Dispose();
            GC.SuppressFinalize(this);
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

            GC.SuppressFinalize(this);
        }
    }
}