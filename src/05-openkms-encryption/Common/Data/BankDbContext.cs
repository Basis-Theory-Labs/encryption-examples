using Common.Data.Entities;
using Microsoft.EntityFrameworkCore;
using OpenKMS.Abstractions;
using OpenKMS.EntityFrameworkCore;

namespace Common.Data;

public class BankDbContext : DbContext
{
    private readonly IEncryptionService _encryptionService;

    public DbSet<Bank> Banks { get; set; } = default!;

    public BankDbContext (DbContextOptions<BankDbContext> options, IEncryptionService encryptionService) : base(options)
    {
        _encryptionService = encryptionService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseEncryption(_encryptionService);

        modelBuilder.Entity<Bank>(Bank.Configure);
    }
}
