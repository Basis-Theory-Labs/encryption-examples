using CustomerPortal.Areas.Banks.Models;
using CustomerPortal.Data;
using CustomerPortal.Encryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Areas.Banks.Pages;

public class DetailsModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly EncryptionService _encryptionService;

    public BankModel Bank { get; set; } = default!;

    public DetailsModel(BankDbContext context, EncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank = await _context.Banks.FirstOrDefaultAsync(m => m.Id == id);
        if (bank == null) return NotFound();

        Bank = new BankModel
        {
            Id = bank.Id,
            RoutingNumber = await _encryptionService.Decrypt(bank.RoutingNumber),
            AccountNumber = await _encryptionService.Decrypt(bank.AccountNumber),
            Status = bank.Status
        };

        return Page();
    }
}
