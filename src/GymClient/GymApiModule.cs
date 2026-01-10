using Domain.Ports;
using GymDataFetcher.GymApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GymClient;

public static class GymApiModule
{
    public static IServiceCollection AddGymApiModule(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<GymClientOptions>().BindConfiguration(GymClientOptions.SectionName);

        services.AddSingleton<GymApiTokenService>();
        services.AddTransient<AuthHeaderHandler>();

        services.AddHttpClient<IGymClient, HttpGymClient>(
                (serviceProvider, httpClient) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<GymClientOptions>>();
                    httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
                })
            .AddHttpMessageHandler<AuthHeaderHandler>();

        return services;
    }
}