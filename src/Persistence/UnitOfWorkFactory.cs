using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class UnitOfWorkFactory(IDbContextFactory<ApplicationDbContext> factory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create()
    {
        return new UnitOfWork(factory.CreateDbContext());
    }
}