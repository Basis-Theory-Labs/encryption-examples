using System.Text;
using Common.Data;
using CustomerPortal.Areas.Banks.Models;
using Encryption;
using Encryption.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Areas.Banks.Pages;

public class IndexModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly IEncryptionService _encryptionService;
    public IList<BankModel> Banks { get;set; } = default!;

    public IndexModel(BankDbContext context, IEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public async Task OnGetAsync()
    {
        var banks = await _context.Banks.ToListAsync();

        Banks = await Task.WhenAll(banks.Select(async bank => new BankModel
        {
            Id = bank.Id,
            RoutingNumber = Encoding.UTF8.GetString(await _encryptionService.DecryptAsync(JsonWebEncryption.FromCompactSerializationFormat(bank.RoutingNumber))),
            AccountNumber = Encoding.UTF8.GetString(await _encryptionService.DecryptAsync(JsonWebEncryption.FromCompactSerializationFormat(bank.AccountNumber))),
            Status = bank.Status
        }));
    }
}
