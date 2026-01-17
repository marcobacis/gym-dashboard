using System.Diagnostics.CodeAnalysis;
using Domain.Ports;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    [field: AllowNull, MaybeNull]
    public IGymAvailabilityRepository Availability => field ??= new GymAvailabilityRepository(context);
    
    [field: AllowNull, MaybeNull]
    public IGymRepository Gyms => field ??= new GymRepository(context);
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}