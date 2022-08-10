using Common.Data;
using Common.Encryption;
using CustomerPortal.Areas.Banks.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Areas.Banks.Pages;

public class IndexModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly EncryptionService _encryptionService;
    public IList<BankModel> Banks { get;set; } = default!;

    public IndexModel(BankDbContext context, EncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public async Task OnGetAsync()
    {
        var banks = await _context.Banks.ToListAsync();

        Banks = await Task.WhenAll(banks.Select(async x => new BankModel
        {
            Id = x.Id,
            RoutingNumber = await _encryptionService.Decrypt(x.RoutingNumber),
            AccountNumber = await _encryptionService.Decrypt(x.AccountNumber),
            Status = x.Status
        }));
    }
}
