using GraphQlDemoPostgresQl.Database.DbContexts;

namespace GraphQlDemoPostgresQl.Database.DALs.Base;

public abstract class BaseDAL(PostgresQlDbContext databaseContext)
{
    protected readonly PostgresQlDbContext _databaseContext =
            databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
}

