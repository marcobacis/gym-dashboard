using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DomainModule
{
    public static IServiceCollection AddDomainModule(this IServiceCollection services)
    {
        services.RegisterApplicationServices();
        return services;
    }

    private static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.RegisterAllServices<IApplicationService>();
    }

    private static void RegisterAllServices<T>(this IServiceCollection services)
    {
        var assembly = typeof(T).Assembly;
        var serviceType = typeof(T);
        foreach (var type in assembly.GetTypes())
        {
            if (serviceType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
            {
                services.AddScoped(type);
            }
        }
    }
}