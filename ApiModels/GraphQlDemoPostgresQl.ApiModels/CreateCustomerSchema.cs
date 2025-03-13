using HotChocolate;

namespace GraphQlDemoPostgresQl.ApiModels;

public class CreateCustomerSchema
{
    [GraphQLName("firstName")]
    public string FirstName { get; set; } = string.Empty;
    [GraphQLName("lastName")]
    public string LastName { get; set; } = string.Empty;
    [GraphQLName("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }
}
