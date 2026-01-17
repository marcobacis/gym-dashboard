using Domain.Entities;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class GymRepository(ApplicationDbContext context) : IGymRepository
{
    public Task<Gym?> GetGymById(Guid gymId, CancellationToken cancellationToken)
    {
        return context.Set<Gym>().FirstOrDefaultAsync(g => g.Id == gymId, cancellationToken);
    }

    public async Task<List<Gym>> GetGyms(CancellationToken cancellationToken)
    {
        return await context.Set<Gym>().ToListAsync(cancellationToken); 
    }
}