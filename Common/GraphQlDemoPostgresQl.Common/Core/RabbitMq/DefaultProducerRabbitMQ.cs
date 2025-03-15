using System.Text;
using GraphQlDemoPostgresQl.Common.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace GraphQlDemoPostgresQl.Common.Core.RabbitMq;

public class DefaultProducerRabbitMQ(IOptions<RabbitMqOptions> rabbitMqOptions, ILogger<DefaultProducerRabbitMQ>? logger) : IMessageQueueService
{

    private readonly RabbitMqOptions _rabbitMqOptions = rabbitMqOptions.Value ?? throw new ArgumentNullException(nameof(rabbitMqOptions));
    private readonly ILogger<DefaultProducerRabbitMQ>? _logger = logger;
    public bool PublishMessage(string message)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqOptions.HostName,
                Port = _rabbitMqOptions.Port,
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password,
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _rabbitMqOptions.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: _rabbitMqOptions.QueueName, basicProperties: null, body: body);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unable to push message to queue");
            return false;
        }
    }
}