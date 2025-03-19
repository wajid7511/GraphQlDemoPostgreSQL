using System.Text;
using GraphQlDemoPostgresQl.Common.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GraphQlDemoPostgresQl.Common.Core.RabbitMq;

public class DefaultRabbitMqService : IMessageQueueService, IDisposable
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly ILogger<DefaultRabbitMqService> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public DefaultRabbitMqService(IOptions<RabbitMqOptions> rabbitMqOptions, ILogger<DefaultRabbitMqService> logger)
    {
        _rabbitMqOptions = rabbitMqOptions.Value ?? throw new ArgumentNullException(nameof(rabbitMqOptions));
        _logger = logger;
        EnsureConnection();
    }

    private void EnsureConnection()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqOptions.HostName,
                Port = _rabbitMqOptions.Port,
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Ensure Queue Declaration Matches
            _channel.QueueDeclare(
                queue: _rabbitMqOptions.QueueName,
                durable: true,  // Make sure this matches the existing queue
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _logger.LogInformation("RabbitMQ connection established.");
        }
    }

    public bool PublishMessage(string message)
    {
        try
        {
            EnsureConnection();

            if (_channel == null || !_channel.IsOpen)
            {
                _logger.LogError("RabbitMQ channel is not open.");
                return false;
            }

            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "",
                routingKey: _rabbitMqOptions.QueueName,
                basicProperties: null,
                body: body
            );

            _logger.LogInformation("Message published: {Message}", message);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to push message to queue");
            return false;
        }
    }

    public void RegisterConsumer(AsyncEventHandler<BasicDeliverEventArgs> eventHandler)
    {
        EnsureConnection();

        if (_channel == null)
        {
            _logger.LogError("Channel is not available for consuming messages.");
            return;
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += eventHandler;

        _channel.BasicConsume(
            queue: _rabbitMqOptions.QueueName,
            autoAck: true,
            consumer: consumer
        );

        _logger.LogInformation("Consumer registered on queue: {QueueName}", _rabbitMqOptions.QueueName);
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        _channel?.Dispose();
        _connection?.Dispose();
        _logger.LogInformation("RabbitMQ resources disposed.");
        GC.SuppressFinalize(this);
    }
}
