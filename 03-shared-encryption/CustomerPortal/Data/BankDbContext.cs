using CustomerPortal.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data;

public class BankDbContext : DbContext
{
    public DbSet<Bank> Banks { get; set; }

    public BankDbContext (DbContextOptions<BankDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bank>(Bank.Configure);
    }
}
