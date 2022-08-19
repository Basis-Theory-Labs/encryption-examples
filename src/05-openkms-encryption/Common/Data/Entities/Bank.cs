using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenKMS.EntityFrameworkCore;

namespace Common.Data.Entities;

public class Bank
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    [Required, Encrypted(EncryptionSchemes.BankRoutingNumber)]
    public string RoutingNumber { get; set; }

    [Required, Encrypted(EncryptionSchemes.BankAccountNumber)]
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
