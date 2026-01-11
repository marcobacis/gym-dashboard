using Domain.Entities;

namespace Domain.Ports;

public interface IGymAvailabilityRepository
{
    public void AddAvailabilityItem(AvailabilityItem item);
}