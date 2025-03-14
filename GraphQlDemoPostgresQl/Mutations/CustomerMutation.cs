using AutoMapper;
using GraphQlDemoPostgresQl.ApiModels;
using GraphQlDemoPostgresQl.Database.DALs;
using GraphQlDemoPostgresQl.DatabaseModels.Customers;
using HotChocolate.Subscriptions;

namespace GraphQlDemoPostgresQl.Mutations;

[ExtendObjectType<Mutation>]
public class CustomerMutation(ITopicEventSender eventSender)
{
    private readonly ITopicEventSender _eventSender = eventSender;

    public async Task<CustomerSchema> CreateCustomer([Service] CustomerDAL customerDAL, [Service] IMapper mapper, CreateCustomerSchema customerSchema, CancellationToken cancellationToken = default)
    {
        var customerEntity = mapper.Map<CustomerEntity>(customerSchema);
        var addResult = await customerDAL.AddAsync(customerEntity, cancellationToken);
        if (addResult.IsSuccess && addResult.Data != null)
        {
            var customer = mapper.Map<CustomerSchema>(addResult.Data);
            // Publish event to the "newCustomer" topic
            await _eventSender.SendAsync("onCustomerCreated", customer, cancellationToken);
            return customer;
        }
        throw addResult.Exception ?? new Exception("Unable to add customer;");
    }
}
