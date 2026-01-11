namespace Domain.Ports;

public interface IGymClient
{ 
    Task<int?> GetCurrentAvailability(CancellationToken cancellationToken = default);
}