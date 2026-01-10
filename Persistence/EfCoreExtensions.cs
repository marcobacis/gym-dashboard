using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class EfCoreExtensions
{
    public static async Task ApplyMigrations(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await appDbContext.Database.MigrateAsync(cancellationToken);
    }
}
