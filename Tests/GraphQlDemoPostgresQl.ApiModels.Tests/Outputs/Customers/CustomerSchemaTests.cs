using GraphQlDemoPostgresQl.ApiModels.Outputs.Customers;

namespace GraphQlDemoPostgresQl.ApiModels.Tests.Outputs.Customers;

[TestClass]
public class CustomerSchemaTests : BaseGraphqlSchemaTests
{
    [TestMethod]
    public void CustomerSchema_Default_Values()
    {
        //Act & Arange 
        var obj = (CustomerSchema)GetObj();
        //Assert
        Assert.AreEqual("", obj.FirstName);
        Assert.AreEqual("", obj.LastName);
    }
    [TestMethod]
    public void CustomerSchema_Verify_Assigned_Value()
    {
        //Arange 
        var dateTime = new DateTime();
        var obj = (CustomerSchema)GetObj();
        //Act
        obj.Id = 100;
        obj.FirstName = "User";
        obj.LastName = "New";
        obj.DateOfBirth = dateTime;

        //Assert
        Assert.AreEqual(100, obj.Id);
        Assert.AreEqual("User", obj.FirstName);
        Assert.AreEqual("New", obj.LastName);
        Assert.AreEqual(dateTime, obj.DateOfBirth);
    }
    public override void AssertGraphQlName(Dictionary<string, string> graphQlNameDictionary)
    {
        Assert.AreEqual("id", graphQlNameDictionary[nameof(CustomerSchema.Id)]);
        Assert.AreEqual("firstName", graphQlNameDictionary[nameof(CustomerSchema.FirstName)]);
        Assert.AreEqual("lastName", graphQlNameDictionary[nameof(CustomerSchema.LastName)]);
        Assert.AreEqual("dateOfBirth", graphQlNameDictionary[nameof(CustomerSchema.DateOfBirth)]);
    }

    public override object GetObj()
    {
        return new CustomerSchema();
    }
}
