using GraphQlDemoPostgresQl.ApiModels.Inputs.Customers;

namespace GraphQlDemoPostgresQl.ApiModels.Tests.Inputs.Customers;
[TestClass]
public class CreateCustomerSchemaTests : BaseGraphqlSchemaTests
{
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
