using System.Text;
using GraphQlDemoPostgresQl.Abstractions.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GraphQlDemoPostgresQl.Common.Core.RabbitMq;

public class DefaultRabbitMqService : IMessageQueueService, IDisposable
{
    private readonly IRabbitMqConnectionProvider _connectionProvider;
    private readonly ILogger<DefaultRabbitMqService>? _logger;
    private readonly string _queueName;

    public DefaultRabbitMqService(
        IOptions<RabbitMqOptions> rabbitMqOptions,
        IRabbitMqConnectionProvider connectionProvider,
        ILogger<DefaultRabbitMqService>? logger = null)
    {
        var options = rabbitMqOptions.Value ?? throw new ArgumentNullException(nameof(rabbitMqOptions));
        _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        _queueName = options.QueueName;
        _logger = logger;

        EnsureQueueExists();
    }

    private void EnsureQueueExists()
    {
        var channel = _connectionProvider.GetChannel();
        channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }
    public bool PublishMessage(string message)
    {
        try
        {
            var channel = _connectionProvider.GetChannel();
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error publishing message.");
            return false;
        }
    }

    public void RegisterConsumer(AsyncEventHandler<BasicDeliverEventArgs> eventHandler)
    {
        var channel = _connectionProvider.GetChannel();

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += eventHandler;

        channel.BasicConsume(
            queue: _queueName,
            autoAck: true,
            consumer: consumer
        );
    }

    public void Dispose()
    {
        (_connectionProvider as IDisposable)?.Dispose();
        GC.SuppressFinalize(this);
    }
}
