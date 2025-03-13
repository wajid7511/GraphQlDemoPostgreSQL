using System.Reflection;
using GraphQlDemoPostgresQl.Abstractions;
using GraphQlDemoPostgresQl.Mappers;
using GraphQlDemoPostgresQl.Mutations;
using GraphQlDemoPostgresQl.Queries;
using GraphQlDemoPostgresQl.Subscriptions;

namespace GraphQlDemoPostgresQl.Extensions;

public static class GraphQlDemoPostgresQlExtension
{
    public static IServiceCollection AddGraphQlDemoPostgresQlMapper(this IServiceCollection service)
    {
        service.AddAutoMapper(typeof(GraphQlDemoPostgresQlMapperProfile));
        return service;
    }
    public static IServiceCollection AddGraphQlDemoPostgresQl(this IServiceCollection service)
    {
        service
            .AddGraphQLServer()
            .AddQueryType<Query>()
            .AddTypeExtension<CustomerQuery>()
            .AddMutationType<Mutation>()
            .AddTypeExtension<CustomerMutation>()
            .AddSubscriptionType<CustomerSubscription>()
            .AddInMemorySubscriptions()
            .AddProjections()
            .AddFiltering()
            .AddSorting();
        return service;
    }

    public static void RegisterGraphQlDemoPostgresQlIServicesRegisterModules(
           this IServiceCollection services,
           ConfigurationManager configuration
       )
    {
        try
        {
            var assemblies = Directory
                .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Select(Assembly.LoadFrom)
                .ToArray();

            var moduleTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(
                    t =>
                        typeof(IServiceRegistrationModule).IsAssignableFrom(t)
                        && !t.IsInterface
                        && !t.IsAbstract
                );

            foreach (var moduleType in moduleTypes)
            {
                var moduleInstance = Activator.CreateInstance(moduleType);
                if (moduleInstance is not null)
                {
                    var module = (IServiceRegistrationModule)moduleInstance;
                    module?.RegisterServices(services, configuration);
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            Console.WriteLine("ReflectionTypeLoadException caught:");
            foreach (var loaderException in ex.LoaderExceptions)
            {
                Console.WriteLine("Error:" + loaderException?.ToString() ?? "No Exception");
            }
        }
    }

}
