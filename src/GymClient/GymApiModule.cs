using Domain.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace GymClient;

public static class GymApiModule
{
    public static IServiceCollection AddGymApiModule(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<GymClientOptions>().BindConfiguration(GymClientOptions.SectionName);
        services.AddSingleton<IGymClientFactory, GymClientFactory>();

        return services;
    }
}