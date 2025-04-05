using GraphQlDemoPostgresQl.Abstractions.RabbitMq;
using RabbitMQ.Client;

namespace GraphQlDemoPostgresQl.Common.Core.RabbitMq;

public class RabbitMqConnectionProvider(IConnectionFactory connectionFactory) : IRabbitMqConnectionProvider
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    private IConnection? _connection;
    private IModel? _channel;
    private readonly object _lock = new();

    public IModel GetChannel()
    {
        // Use double-check locking to ensure thread safety
        if (_channel == null || _channel.IsClosed)
        {
            lock (_lock)
            {
                if (_channel == null || _channel.IsClosed)
                {
                    _connection ??= _connectionFactory.CreateConnection();
                    _channel = _connection.CreateModel();
                    _channel.BasicQos(0, 1, false); // Fair dispatch
                }
            }
        }

        return _channel;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        _channel?.Dispose();
        _connection?.Dispose();
    }
}