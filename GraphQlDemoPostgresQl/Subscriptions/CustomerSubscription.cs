using GraphQlDemoPostgresQl.ApiModels;

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
