using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Repositories;

namespace Persistence;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistenceModule(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<AppDbContextOptions>().BindConfiguration(AppDbContextOptions.SectionName);
        
        services.AddDbContextPool<ApplicationDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<AppDbContextOptions>>().Value;
            options.UseNpgsql(dbOptions.Database);
        });
        
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IGymAvailabilityRepository, GymAvailabilityRepository>();

        return services;
    }
}