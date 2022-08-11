using CustomerPortal.Data.Entities;

namespace CustomerPortal.Areas.Banks.Models;

public class BankModel
{
    public Guid Id { get; set; }
    public string RoutingNumber { get; set; }
    public string AccountNumber { get; set; }
    public ProcessStatus Status { get; set; }
}
