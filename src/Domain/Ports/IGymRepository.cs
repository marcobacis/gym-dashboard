using Domain.Entities;

namespace Domain.Ports;

public interface IGymRepository
{
    public Task<List<Gym>> GetGyms(CancellationToken cancellationToken);
}