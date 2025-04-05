using GraphQlDemoPostgresQl.ApiModels.Inputs.Customers;

namespace GraphQlDemoPostgresQl.ApiModels.Tests.Inputs.Customers;
[TestClass]
public class CreateCustomerSchemaTests : BaseGraphqlSchemaTests
{
    [TestMethod]
    public void CreateCustomerSchema_Default_Values()
    {
        //Act & Arange 
        var obj = (CreateCustomerSchema)GetObj();
        //Assert
        Assert.AreEqual("", obj.FirstName);
        Assert.AreEqual("", obj.LastName);
    }
    [TestMethod]
    public void CreateCustomerSchema_Verify_Assigned_Value()
    {
        //Arange 
        var dateTime = new DateTime();
        var obj = (CreateCustomerSchema)GetObj();
        //Act
        obj.FirstName = "User";
        obj.LastName = "New";
        obj.DateOfBirth = dateTime;

        //Assert
        Assert.AreEqual("User", obj.FirstName);
        Assert.AreEqual("New", obj.LastName);
        Assert.AreEqual(dateTime, obj.DateOfBirth);
    }
    public override void AssertGraphQlName(Dictionary<string, string> graphQlNameDictionary)
    {
        Assert.AreEqual("firstName", graphQlNameDictionary[nameof(CreateCustomerSchema.FirstName)]);
        Assert.AreEqual("lastName", graphQlNameDictionary[nameof(CreateCustomerSchema.LastName)]);
        Assert.AreEqual("dateOfBirth", graphQlNameDictionary[nameof(CreateCustomerSchema.DateOfBirth)]);
    }

    public override object GetObj()
    {
        return new CreateCustomerSchema();
    }
}
