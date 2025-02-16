using GraphQlDemoPostgresQl.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GraphQlDemoPostgresQl.Database;

public class PostgresQlDbContextFactory : IDesignTimeDbContextFactory<PostgresQlDbContext>
{
    public PostgresQlDbContext CreateDbContext(string[] args)
    {
        // Configuration for design-time services
        var optionsBuilder = new DbContextOptionsBuilder<PostgresQlDbContext>();

        // Use your connection string or configure the options here
        optionsBuilder.UseNpgsql(
            "Host=localhost;Database=GraphQlDemoPostgresQl;Username=postgres;Password=mypassword"
        );

        return new PostgresQlDbContext(optionsBuilder.Options);
    }
}
