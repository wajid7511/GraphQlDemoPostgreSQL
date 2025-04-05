using GraphQlDemoPostgresQl.Abstractions;
using GraphQlDemoPostgresQl.Abstractions.RabbitMq;
using GraphQlDemoPostgresQl.Common.Core.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace GraphQlDemoPostgresQl.Common.Extensions;

public class CommonRegisterServices : IServiceRegistrationModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.CONFIG_PATH));
        services.AddSingleton<IConnectionFactory>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

            return new ConnectionFactory
            {
                HostName = options.HostName,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password,
                DispatchConsumersAsync = true
            };
        });

        // Register RabbitMQ connection provider
        services.AddSingleton<IRabbitMqConnectionProvider, RabbitMqConnectionProvider>();

        services.AddSingleton<IMessageQueueService, DefaultRabbitMqService>();
    }
}