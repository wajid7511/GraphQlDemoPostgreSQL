using GraphQlDemoPostgresQl.Abstractions;
using GraphQlDemoPostgresQl.Database.DALs;
using GraphQlDemoPostgresQl.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQlDemoPostgresQl.Database.Extensions;

public class DatabaseRegisterServices : IServiceRegistrationModule
{
    public void RegisterServices(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<PostgresQlDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("Default"))
        );
        services.AddScoped<CustomerDAL>();
    }
}
