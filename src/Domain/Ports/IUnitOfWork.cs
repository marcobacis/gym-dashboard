namespace Domain.Ports;

public interface IUnitOfWork
{
    IGymAvailabilityRepository Availability { get; }
    
    IGymRepository Gyms { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}