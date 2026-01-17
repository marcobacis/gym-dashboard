using Domain.Entities;

namespace Domain.Ports;

public interface IGymAvailabilityRepository
{
    public void AddAvailabilityItem(AvailabilityItem item);
    
    public Task<AvailabilityItem?> GetLatestAvailabilityItem(Guid gymId, CancellationToken cancellationToken);
    
    public Task<List<AvailabilityItem>> GetAvailabilityItems(Guid gymId, CancellationToken cancellationToken);
}