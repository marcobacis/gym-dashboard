using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Repositories;
using SoftFluent.EntityFrameworkCore.DataEncryption;
using SoftFluent.EntityFrameworkCore.DataEncryption.Providers;

namespace Persistence;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistenceModule(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<AppDbContextOptions>().BindConfiguration(AppDbContextOptions.SectionName);
        services.AddOptionsWithValidateOnStart<EncryptionOptions>().BindConfiguration(EncryptionOptions.SectionName);

        services.AddSingleton<IEncryptionProvider>((serviceProvider) =>
        {
            var config = serviceProvider.GetRequiredService<IOptions<EncryptionOptions>>().Value;
            return new AesProvider(config.KeyBytes, config.InitializationVectorBytes);
        });
        
        services.AddPooledDbContextFactory<ApplicationDbContext>((serviceProvider, options) =>
        {
            var dbOptions = serviceProvider.GetRequiredService<IOptions<AppDbContextOptions>>().Value;
            options.UseNpgsql(dbOptions.Database);
        });
        
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        
        return services;
    }
}