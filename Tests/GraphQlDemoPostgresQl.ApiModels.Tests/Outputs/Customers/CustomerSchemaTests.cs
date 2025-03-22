using GraphQlDemoPostgresQl.ApiModels.Outputs.Customers;

namespace GraphQlDemoPostgresQl.ApiModels.Tests.Outputs.Customers;

[TestClass]
public class CustomerSchemaTests : BaseGraphqlSchemaTests
{
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
