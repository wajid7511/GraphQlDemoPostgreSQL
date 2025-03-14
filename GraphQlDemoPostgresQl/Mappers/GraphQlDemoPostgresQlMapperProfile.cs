using AutoMapper;
using GraphQlDemoPostgresQl.ApiModels;
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
