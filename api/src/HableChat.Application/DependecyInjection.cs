using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HableChat.Application;

public static class DependecyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
        return services;
    }
        
}
