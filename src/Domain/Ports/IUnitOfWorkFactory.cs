namespace Domain.Ports;

public interface IUnitOfWorkFactory
{
    public IUnitOfWork Create();
}