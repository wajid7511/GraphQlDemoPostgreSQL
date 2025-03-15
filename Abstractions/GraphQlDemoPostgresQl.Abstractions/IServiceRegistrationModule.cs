using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQlDemoPostgresQl.Abstractions;

public interface IServiceRegistrationModule
{
    void RegisterServices(
            IServiceCollection serviceRegistration,
            IConfiguration configuration
       );
}
