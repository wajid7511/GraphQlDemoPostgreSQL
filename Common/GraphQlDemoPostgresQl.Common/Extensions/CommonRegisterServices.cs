using GraphQlDemoPostgresQl.Abstractions;
using GraphQlDemoPostgresQl.Common.Abstractions;
using GraphQlDemoPostgresQl.Common.Core.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQlDemoPostgresQl.Common.Extensions;

public class CommonRegisterServices : IServiceRegistrationModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.CONFIG_PATH));
        services.AddSingleton<IMessageQueueService, DefaultProducerRabbitMQ>();
    }
}