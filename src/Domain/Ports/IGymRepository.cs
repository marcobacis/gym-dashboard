using Domain.Entities;

namespace Domain.Ports;

public interface IGymRepository
{
    public Task<Gym?> GetGymById(Guid gymId, CancellationToken cancellationToken);
    
    public Task<List<Gym>> GetGyms(CancellationToken cancellationToken);
}