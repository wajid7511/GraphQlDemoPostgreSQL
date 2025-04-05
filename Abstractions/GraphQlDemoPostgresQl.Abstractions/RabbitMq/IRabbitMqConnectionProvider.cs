using System;
using RabbitMQ.Client;

namespace GraphQlDemoPostgresQl.Abstractions.RabbitMq;

public interface IRabbitMqConnectionProvider : IDisposable
{
    IModel GetChannel();
}