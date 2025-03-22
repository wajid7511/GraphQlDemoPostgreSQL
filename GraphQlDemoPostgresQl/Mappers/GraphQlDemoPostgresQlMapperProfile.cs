using AutoMapper;
using GraphQlDemoPostgresQl.ApiModels.Inputs.Customers;
using GraphQlDemoPostgresQl.ApiModels.Outputs.Customers;
using GraphQlDemoPostgresQl.DatabaseModels.Customers;

namespace GraphQlDemoPostgresQl.Mappers;

public class GraphQlDemoPostgresQlMapperProfile : Profile
{
    public GraphQlDemoPostgresQlMapperProfile()
    {
        CreateMap<CustomerEntity, CustomerSchema>();
        CreateMap<CreateCustomerSchema, CustomerEntity>();
    }
}
