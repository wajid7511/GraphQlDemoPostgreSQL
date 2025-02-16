using GraphQlDemoPostgresQl.DatabaseModels.Customers;
using Microsoft.EntityFrameworkCore;

namespace GraphQlDemoPostgresQl.Database.DbContexts;

public class PostgresQlDbContext : DbContext
{
    public PostgresQlDbContext(DbContextOptions<PostgresQlDbContext> options)
                : base(options)
    {

    }
    public DbSet<CustomerEntity> Customers { get; set; }
}
