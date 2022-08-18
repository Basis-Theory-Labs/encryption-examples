using System.ComponentModel.DataAnnotations;

namespace BasicEncryption.Areas.Banks.Models;

public class EditBankModel
{
    public Guid Id { get; set; }

    [Required]
    public string RoutingNumber { get; set; }

    [Required]
    public string AccountNumber { get; set; }
}
