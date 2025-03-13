using System;
using GraphQlDemoPostgresQl.ApiModels;
using HotChocolate.Subscriptions;

namespace GraphQlDemoPostgresQl.Mutations;

[ExtendObjectType<Mutation>]
public class CustomerMutation(ITopicEventSender eventSender)
{
    private readonly ITopicEventSender _eventSender = eventSender;

    public async Task<CustomerSchema> CreateCustomer(CreateCustomerSchema customerSchema)
    {
        var customer = new CustomerSchema
        {
            Id = 100,
            FirstName = customerSchema.FirstName,
            LastName = customerSchema.LastName,
            DateOfBirth = customerSchema.DateOfBirth,
        };

        // Publish event to the "newCustomer" topic
        await _eventSender.SendAsync("onCustomerCreated", customer);
        return customer;
    }
}
