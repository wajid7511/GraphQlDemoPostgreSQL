using System.Text.Json;
using AutoMapper;
using GraphQlDemoPostgresQl.Abstractions.RabbitMq;
using GraphQlDemoPostgresQl.ApiModels.Inputs.Customers;
using GraphQlDemoPostgresQl.ApiModels.Outputs.Customers;
using GraphQlDemoPostgresQl.Database.DALs;
using GraphQlDemoPostgresQl.DatabaseModels.Customers;

namespace GraphQlDemoPostgresQl.Mutations;

[ExtendObjectType<Mutation>]
public class CustomerMutation(IMessageQueueService messageQueueService)
{
    private readonly IMessageQueueService _messageQueueService = messageQueueService ?? throw new ArgumentNullException(nameof(messageQueueService));
    public async Task<CustomerSchema> CreateCustomer([Service] CustomerDAL customerDAL, [Service] IMapper mapper, CreateCustomerSchema customerSchema, CancellationToken cancellationToken = default)
    {
        var customerEntity = mapper.Map<CustomerEntity>(customerSchema);
        var addResult = await customerDAL.AddAsync(customerEntity, cancellationToken);
        if (addResult.IsSuccess && addResult.Data != null)
        {
            var customer = mapper.Map<CustomerSchema>(addResult.Data);
            _messageQueueService.PublishMessage(JsonSerializer.Serialize(customer));
            return customer;
        }
        throw addResult.Exception ?? new Exception("Unable to add customer;");
    }
}
