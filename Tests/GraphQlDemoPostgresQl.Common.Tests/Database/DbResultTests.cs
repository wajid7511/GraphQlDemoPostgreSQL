using GraphQlDemoPostgresQl.Common.Database;

namespace GraphQlDemoPostgresQl.Common.Tests.Database;
[TestClass]
public class DbResultTests
{
    [TestMethod]
    public void Constructor_ShouldSetIsSuccessAndEntity()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Test" };
        bool isSuccess = true;

        // Act
        var result = new DbResult<TestEntity>(isSuccess, entity);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual(entity, result.Data);
        Assert.IsFalse(result.IsError);
    }

    [TestMethod]
    public void Constructor_ShouldSetIsError_WhenExceptionIsPassed()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var result = new DbResult<TestEntity>(false) { Exception = exception };

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.IsError);
        Assert.AreEqual(exception, result.Exception);
    }

    [TestMethod]
    public void Constructor_ShouldNotSetException_WhenNotProvided()
    {
        // Act
        var result = new DbResult<TestEntity>(true);

        // Assert
        Assert.IsFalse(result.IsError);
        Assert.IsNull(result.Exception);
    }
    [TestMethod]
    public void Constructor_ShouldSetException_WhenNotProvided()
    {
        // Act
        var result = new DbResult<TestEntity>(new Exception("This is test"));

        // Assert
        Assert.IsTrue(result.IsError);
        Assert.IsNotNull(result.Exception);
        Assert.IsFalse(result.IsSuccess);
    }

    private class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}