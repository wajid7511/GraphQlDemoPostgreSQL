using System.Linq.Expressions;
using GraphQlDemoPostgresQl.Common.Database;
using GraphQlDemoPostgresQl.Database.DALs.Base;
using GraphQlDemoPostgresQl.Database.DbContexts;
using GraphQlDemoPostgresQl.DatabaseModels.Customers;
using Microsoft.EntityFrameworkCore;

namespace GraphQlDemoPostgresQl.Database.DALs;

public class CustomerDAL(PostgresQlDbContext databaseContext) : BaseDAL(databaseContext)
{
    public virtual async Task<DbResult<CustomerEntity>> AddAsync(CustomerEntity customer, CancellationToken cancellationToken)
    {
        if (customer != null)
        {
            try
            {
                var saveChangeResult = await _databaseContext.Customers.AddAsync(customer, cancellationToken);
                await _databaseContext.SaveChangesAsync(cancellationToken);
                if (saveChangeResult != null && saveChangeResult.Entity != null)
                {
                    return new DbResult<CustomerEntity>(true, saveChangeResult.Entity!);
                }
                else
                {
                    return new DbResult<CustomerEntity>(false);
                }
            }
            catch (Exception ex)
            {
                return new DbResult<CustomerEntity>(false) { Exception = ex };
            }
        }
        throw new ArgumentNullException(nameof(customer));
    }
    public virtual async ValueTask<DbResult<List<CustomerEntity>>> GetCustomersAsync(
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
            return new DbResult<List<CustomerEntity>>(result.Count != 0, result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new DbResult<List<CustomerEntity>>(false) { Exception = ex };
        }
    }

}
