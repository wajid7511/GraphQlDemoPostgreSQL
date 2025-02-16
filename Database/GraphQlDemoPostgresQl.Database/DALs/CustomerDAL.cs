using System.Linq.Expressions;
using GraphQlDemoPostgresQl.Common.Database;
using GraphQlDemoPostgresQl.Database.DALs.Base;
using GraphQlDemoPostgresQl.Database.DbContexts;
using GraphQlDemoPostgresQl.DatabaseModels.Customers;
using Microsoft.EntityFrameworkCore;

namespace GraphQlDemoPostgresQl.Database.DALs;

public class CustomerDAL(PostgresQlDbContext databaseContext) : BaseDAL(databaseContext)
{
    public virtual async ValueTask<DbGetResult<List<CustomerEntity>>> GetCustomersAsync(
             Expression<Func<CustomerEntity, bool>> predicate,
             int take = 10,
             int skip = 0
         )
    {
        ArgumentNullException.ThrowIfNull(predicate);
        try
        {
            var query = _databaseContext.Customers.Where(predicate);

            query = query.OrderBy(e => e.Id).Skip(skip).Take(take);

            var result = await query.ToListAsync();
            return new DbGetResult<List<CustomerEntity>>(result.Count != 0, result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new DbGetResult<List<CustomerEntity>>(false) { Exception = ex };
        }
    }

}
