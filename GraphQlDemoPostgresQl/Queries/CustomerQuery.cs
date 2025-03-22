using AutoMapper;
using GraphQlDemoPostgresQl.ApiModels.Outputs.Customers;
using GraphQlDemoPostgresQl.Database.DbContexts;

namespace GraphQlDemoPostgresQl.Queries;

[ExtendObjectType<Query>]
public class CustomerQuery
{
    [UseOffsetPaging(DefaultPageSize = 10, IncludeTotalCount = true, MaxPageSize = 100)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<CustomerSchema> GetCustomers(
        [Service] PostgresQlDbContext dbContext,
        [Service] IMapper mapper
    )
    {
        return mapper.ProjectTo<CustomerSchema>(dbContext.Customers);
    }
}
