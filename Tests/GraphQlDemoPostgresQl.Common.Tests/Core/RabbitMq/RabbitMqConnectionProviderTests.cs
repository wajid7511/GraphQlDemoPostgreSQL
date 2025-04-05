
using GraphQlDemoPostgresQl.Common.Core.RabbitMq;
using Moq;
using RabbitMQ.Client;
namespace GraphQlDemoPostgresQl.Common.Tests.Core.RabbitMq;

[TestClass]
public class RabbitMqConnectionProviderTests
{
    private Mock<IConnectionFactory> _mockConnectionFactory = null!;
    private Mock<IConnection> _mockConnection = null!;
    private Mock<IModel> _mockChannel = null!;
    private RabbitMqConnectionProvider _provider = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockConnectionFactory = new Mock<IConnectionFactory>();
        _mockConnection = new Mock<IConnection>();
        _mockChannel = new Mock<IModel>();

        // Setup IsClosed to simulate channel status
        _mockChannel.Setup(c => c.IsClosed).Returns(false);
        _mockConnection.Setup(c => c.CreateModel()).Returns(_mockChannel.Object);
        _mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(_mockConnection.Object);

        _provider = new RabbitMqConnectionProvider(_mockConnectionFactory.Object);
    }

    [TestMethod]
    public void GetChannel_ShouldCreateConnectionAndChannel_WhenNotAlreadyCreated()
    {
        // Act
        var channel = _provider.GetChannel();

        // Assert
        Assert.IsNotNull(channel);
        _mockConnectionFactory.Verify(f => f.CreateConnection(), Times.Once);
        _mockConnection.Verify(c => c.CreateModel(), Times.Once);
        _mockChannel.Verify(c => c.BasicQos(0, 1, false), Times.Once);
    }

    [TestMethod]
    public void GetChannel_ShouldReturnSameChannel_OnMultipleCalls()
    {
        // Act
        var channel1 = _provider.GetChannel();
        var channel2 = _provider.GetChannel();

        // Assert
        Assert.AreSame(channel1, channel2, "GetChannel should return the same instance");
        _mockConnectionFactory.Verify(f => f.CreateConnection(), Times.Once);
        _mockConnection.Verify(c => c.CreateModel(), Times.Once);
    }

    [TestMethod]
    public void GetChannel_ShouldRecreateChannel_WhenPreviousIsClosed()
    {
        // Arrange
        _mockChannel.SetupSequence(c => c.IsClosed)
            .Returns(true)   // First call returns closed
            .Returns(false); // Second call returns open (new channel)

        var mockNewChannel = new Mock<IModel>();
        mockNewChannel.Setup(c => c.IsClosed).Returns(false);
        _mockConnection.Setup(c => c.CreateModel()).Returns(mockNewChannel.Object);

        // Act
        var channel = _provider.GetChannel();

        // Assert
        Assert.AreSame(mockNewChannel.Object, channel);
        _mockConnection.Verify(c => c.CreateModel(), Times.Once);
        mockNewChannel.Verify(c => c.BasicQos(0, 1, false), Times.Once);
    }

    [TestMethod]
    public void Dispose_ShouldCloseAndDisposeConnectionAndChannel()
    {
        // Act
        _provider.GetChannel(); // Ensure channel and connection are created
        _provider.Dispose();

        // Assert
        _mockChannel.Verify(c => c.Close(), Times.Once);
        _mockChannel.Verify(c => c.Dispose(), Times.Once);
        _mockConnection.Verify(c => c.Close(), Times.Once);
        _mockConnection.Verify(c => c.Dispose(), Times.Once);
    }
}

