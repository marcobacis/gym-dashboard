namespace Domain.Ports;

public interface IUnitOfWork
{
    IGymAvailabilityRepository Availability { get; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}