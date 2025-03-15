using System;

namespace GraphQlDemoPostgresQl.Common.Abstractions;

public interface IMessageQueueService
{
    bool PublishMessage(string message);
}
