using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Data.Entities;

public class Bank
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required]
    public string RoutingNumber { get; set; }

    [Required]
    public string AccountNumber { get; set; }

    public ProcessStatus Status { get; set; }

    internal static void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.Property(b => b.Status).HasConversion<string>();
    }
}

public enum ProcessStatus
{
    PENDING,

    PROCESSED
}
