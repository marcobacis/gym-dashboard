using Domain.Entities;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class GymRepository(ApplicationDbContext context) : IGymRepository
{
    public async Task<List<Gym>> GetGyms(CancellationToken cancellationToken)
    {
        return await context.Set<Gym>().ToListAsync(cancellationToken); 
    }
}