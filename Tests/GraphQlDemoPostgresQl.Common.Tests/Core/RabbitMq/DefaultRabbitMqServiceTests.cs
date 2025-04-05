using System.Text;
using GraphQlDemoPostgresQl.Abstractions.RabbitMq;
using GraphQlDemoPostgresQl.Common.Core.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GraphQlDemoPostgresQl.Common.Tests.Core.RabbitMq;

[TestClass]
public class DefaultRabbitMqServiceTests
{
    private readonly Mock<IOptions<RabbitMqOptions>> _mockOptions = null!;
    private readonly Mock<ILogger<DefaultRabbitMqService>> _mockLogger = null!;
    private readonly Mock<IRabbitMqConnectionProvider> _mockRabbitMqConnectionProvider = null!;
    private readonly DefaultRabbitMqService _rabbitMqService = null!;
    public DefaultRabbitMqServiceTests()
    {
        _mockOptions = new Mock<IOptions<RabbitMqOptions>>();
        _mockLogger = new Mock<ILogger<DefaultRabbitMqService>>();
        _mockRabbitMqConnectionProvider = new Mock<IRabbitMqConnectionProvider>();

        var rabbitMqConfig = new RabbitMqOptions
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            QueueName = "test_queue"
        };

        _mockOptions.Setup(o => o.Value).Returns(rabbitMqConfig);
        Mock<IModel> model = new();
        _mockRabbitMqConnectionProvider.Setup(s => s.GetChannel()).Returns(model.Object);
        _rabbitMqService = new DefaultRabbitMqService(_mockOptions.Object, _mockRabbitMqConnectionProvider.Object, _mockLogger.Object);
    }
    [TestMethod]
    public void Constructor_Throw_ArgumentNullException_When_RabbitMqOptions_Is_Null()
    {
        //Arrange 
        var mockOptions = new Mock<IOptions<RabbitMqOptions>>();

        mockOptions.Setup(o => o.Value).Returns(null as RabbitMqOptions);

        //Act 
        var result = Assert.ThrowsException<ArgumentNullException>(() => new DefaultRabbitMqService(mockOptions.Object, _mockRabbitMqConnectionProvider.Object, _mockLogger.Object));

        //Assert
        Assert.IsInstanceOfType<ArgumentNullException>(result);
    }
    [TestMethod]
    public void Constructor_Throw_ArgumentNullException_When_RabbitMqConnectionProvider_Is_Null()
    {
        //Arrange & Act 
        var result = Assert.ThrowsException<ArgumentNullException>(() => new DefaultRabbitMqService(_mockOptions.Object, null, _mockLogger.Object));

        //Assert
        Assert.IsInstanceOfType<ArgumentNullException>(result);
    }
    [TestMethod]
    public void PublishMessage_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange 

        // Act
        var result = _rabbitMqService.PublishMessage("Test Message");

        // Assert
        Assert.IsTrue(result);
        _mockLogger.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void PublishMessage_ShouldReturnFalse_OnException()
    {
        // Arrange 
        _mockRabbitMqConnectionProvider.Setup(s => s.GetChannel()).Throws(new Exception("Cannot open the connection"));
        // Act
        var result = _rabbitMqService.PublishMessage("Test Message");

        // Assert
        Assert.IsFalse(result);

        _mockLogger.Verify(
         x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error publishing message.")),
                It.IsAny<Exception?>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
            ),
            Times.Once // Ensure it was logged exactly once
        );
        _mockLogger.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task RegisterConsumer_ShouldInvokeEventHandler()
    {
        // Arrange
        var eventHandlerCalled = false;
        AsyncEventHandler<BasicDeliverEventArgs> eventHandler = async (model, ea) =>
        {
            eventHandlerCalled = true;
            await Task.CompletedTask;
        };

        // Act
        _rabbitMqService.RegisterConsumer(eventHandler);

        // Simulate receiving a message
        var body = Encoding.UTF8.GetBytes("Test Message");
        var eventArgs = new BasicDeliverEventArgs { Body = new ReadOnlyMemory<byte>(body) };
        await eventHandler.Invoke(null, eventArgs);

        // Assert
        Assert.IsTrue(eventHandlerCalled);
        _mockLogger.VerifyNoOtherCalls();

    }
    [TestMethod]
    public void Dispose_Should_Dispose_ConnectionProvider_If_Disposable()
    {
        // Arrange
        var mockConnectionProvider = new Mock<IRabbitMqConnectionProvider>();
        var mockDisposable = mockConnectionProvider.As<IDisposable>();
        Mock<IModel> model = new();
        mockConnectionProvider.Setup(s => s.GetChannel()).Returns(model.Object);

        var service = new DefaultRabbitMqService(_mockOptions.Object, mockConnectionProvider.Object, _mockLogger.Object);

        // Act
        service.Dispose();

        // Assert
        mockConnectionProvider.As<IDisposable>().Verify(d => d.Dispose(), Times.Once);
    }

}