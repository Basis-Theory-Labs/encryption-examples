using BasicEncryption.Data.Entities;

namespace BasicEncryption.Areas.Banks.Models;

public class BankModel
{
    public Guid Id { get; set; }
    public string RoutingNumber { get; set; }
    public string AccountNumber { get; set; }
    public ProcessStatus Status { get; set; }
}
