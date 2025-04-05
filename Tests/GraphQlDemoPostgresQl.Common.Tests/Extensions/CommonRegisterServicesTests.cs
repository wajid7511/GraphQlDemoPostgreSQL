using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using GraphQlDemoPostgresQl.Abstractions.RabbitMq;
using GraphQlDemoPostgresQl.Common.Extensions;
using GraphQlDemoPostgresQl.Common.Core.RabbitMq;

namespace GraphQlDemoPostgresQl.Common.Tests.Extensions;

[TestClass]
public class CommonRegisterServicesTests
{
    private ServiceCollection _services = null!;
    private IConfiguration _configuration = null!;
    private Mock<IConnectionFactory> _mockConnectionFactory = null!;
    private Mock<IRabbitMqConnectionProvider> _mockRabbitMqConnectionProvider = null!;
    private Mock<IMessageQueueService> _mockMessageQueueService = null!;
    private ServiceProvider _serviceProvider = null!;

    [TestInitialize]
    public void Setup()
    {
        // Create a mock configuration
        var configData = new Dictionary<string, string?>
        {
            { "RabbitMQ:HostName", "localhost" },
            { "RabbitMQ:QueueName", "test_queue" },
            { "RabbitMQ:ExchangeName", "test_exchange" },
            { "RabbitMQ:Port", "257" },
            { "RabbitMQ:UserName", "guest" },
            { "RabbitMQ:Password", "257Guest" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        _services = new ServiceCollection();

        // Register actual services first
        var commonRegisterServices = new CommonRegisterServices();
        commonRegisterServices.RegisterServices(_services, _configuration);

        // Initialize mocks
        _mockConnectionFactory = new Mock<IConnectionFactory>();
        _mockRabbitMqConnectionProvider = new Mock<IRabbitMqConnectionProvider>();
        _mockMessageQueueService = new Mock<IMessageQueueService>();

        // Replace real implementations with mocks
        _services.AddSingleton(_mockConnectionFactory.Object);
        _services.AddSingleton(_mockRabbitMqConnectionProvider.Object);
        _services.AddSingleton(_mockMessageQueueService.Object);

        // Build provider
        _serviceProvider = _services.BuildServiceProvider();
    }

    [TestMethod]
    public void RegisterServices_Should_Register_Dependencies_Correctly()
    {
        // Assert that IOptions<RabbitMqOptions> is registered correctly
        var rabbitMQOptions = _serviceProvider.GetService<IOptions<RabbitMqOptions>>();
        Assert.IsNotNull(rabbitMQOptions);
        Assert.AreEqual("localhost", rabbitMQOptions.Value.HostName);
        Assert.AreEqual("test_queue", rabbitMQOptions.Value.QueueName);
        Assert.AreEqual("test_exchange", rabbitMQOptions.Value.ExchangeName);
        Assert.AreEqual(257, rabbitMQOptions.Value.Port);
        Assert.AreEqual("guest", rabbitMQOptions.Value.UserName);
        Assert.AreEqual("257Guest", rabbitMQOptions.Value.Password);

        // Verify mocked services are returned instead of actual implementations
        var connectionFactoryService = _serviceProvider.GetService<IConnectionFactory>();
        Assert.IsNotNull(connectionFactoryService);
        Assert.AreSame(_mockConnectionFactory.Object, connectionFactoryService, "Mocked IConnectionFactory should be used.");

        var messageQueueService = _serviceProvider.GetService<IMessageQueueService>();
        Assert.IsNotNull(messageQueueService);
        Assert.AreSame(_mockMessageQueueService.Object, messageQueueService, "Mocked IMessageQueueService should be used.");

        var rabbitMqConnectionProviderService = _serviceProvider.GetService<IRabbitMqConnectionProvider>();
        Assert.IsNotNull(rabbitMqConnectionProviderService);
        Assert.AreSame(_mockRabbitMqConnectionProvider.Object, rabbitMqConnectionProviderService, "Mocked IRabbitMqConnectionProvider should be used.");
    }
}
