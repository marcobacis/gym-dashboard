using Domain.Entities;
using Domain.Ports;

namespace Persistence.Repositories;

public class GymAvailabilityRepository(ApplicationDbContext context) : IGymAvailabilityRepository
{
    public void AddAvailabilityItem(AvailabilityItem item)
    {
        context.Set<AvailabilityItem>().Add(item);
    }
}