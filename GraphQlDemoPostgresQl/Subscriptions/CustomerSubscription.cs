using GraphQlDemoPostgresQl.ApiModels.Outputs.Customers;

namespace GraphQlDemoPostgresQl.Subscriptions;

public class CustomerSubscription
{
    [Subscribe]
    [Topic("onCustomerCreated")]
    public CustomerSchema OnCustomerCreated([EventMessage] CustomerSchema customerSchema)
    {
        return customerSchema;
    }
}
