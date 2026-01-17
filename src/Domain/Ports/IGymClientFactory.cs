namespace Domain.Ports;

public interface IGymClientFactory
{
    public Task<IGymClient> Create(string apiPrefix, string username, string password, CancellationToken cancellationToken);
}