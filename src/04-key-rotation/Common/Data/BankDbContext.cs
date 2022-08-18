using Common.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Common.Data;

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
