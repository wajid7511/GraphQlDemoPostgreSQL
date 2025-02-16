using HotChocolate;

namespace GraphQlDemoPostgresQl.ApiModels;

public class CustomerSchema
{
    [GraphQLName("id")]
    public long Id { get; set; }

    [GraphQLName("firstName")]
    public string FirstName { get; set; } = string.Empty;
    [GraphQLName("lastName")]
    public string LastName { get; set; } = string.Empty;
    [GraphQLName("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

}
