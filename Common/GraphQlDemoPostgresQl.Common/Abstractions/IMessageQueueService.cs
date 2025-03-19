using RabbitMQ.Client.Events;

namespace GraphQlDemoPostgresQl.Common.Abstractions;

public interface IMessageQueueService
{
    public void RegisterConsumer(AsyncEventHandler<BasicDeliverEventArgs> eventHandler);
    bool PublishMessage(string message);
}
