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

    public async Task<AvailabilityItem?> GetLatestAvailabilityItem(Guid gymId, CancellationToken cancellationToken)
    {
        return await context.Set<AvailabilityItem>()
            .Where(i => i.GymId == gymId)
            .OrderByDescending(item => item.Time)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<AvailabilityItem>> GetAvailabilityItems(Guid gymId, CancellationToken cancellationToken)
    {
        return await context.Set<AvailabilityItem>()
            .Where(i => i.GymId == gymId)
            .ToListAsync(cancellationToken);
    }
}