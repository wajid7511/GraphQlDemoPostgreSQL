using System;
using RabbitMQ.Client.Events;

namespace GraphQlDemoPostgresQl.Abstractions.RabbitMq;

public interface IMessageQueueService
{
    public void RegisterConsumer(AsyncEventHandler<BasicDeliverEventArgs> eventHandler);
    bool PublishMessage(string message);
}