using Microsoft.EntityFrameworkCore;
using SoftFluent.EntityFrameworkCore.DataEncryption;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IEncryptionProvider _encryptionProvider;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IEncryptionProvider encryptionProvider)
        : base(options)
    {
        _encryptionProvider = encryptionProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.UseEncryption(_encryptionProvider);
    }
}