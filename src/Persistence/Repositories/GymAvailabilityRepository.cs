using Domain.Entities;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class GymAvailabilityRepository(ApplicationDbContext context) : IGymAvailabilityRepository
{
    public void AddAvailabilityItem(AvailabilityItem item)
    {
        context.Set<AvailabilityItem>().Add(item);
    }

    public async Task<AvailabilityItem?> GetLatestAvailabilityItem(CancellationToken cancellationToken)
    {
        return await context.Set<AvailabilityItem>()
            .OrderByDescending(item => item.Time)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<AvailabilityItem>> GetAvailabilityItems(CancellationToken cancellationToken)
    {
        return await context.Set<AvailabilityItem>().ToListAsync(cancellationToken);
    }
}