using System.Text;
using Common.Data;
using CustomerPortal.Areas.Banks.Models;
using Encryption;
using Encryption.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Areas.Banks.Pages;

public class DetailsModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly IEncryptionService _encryptionService;

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
            // RoutingNumber = await _encryptionService.DecryptAsync(bank.RoutingNumber),
            // AccountNumber = await _encryptionService.Decrypt(bank.AccountNumber),
            RoutingNumber = Encoding.UTF8.GetString(await _encryptionService.DecryptAsync(new JsonWebEncryption())),
            AccountNumber = Encoding.UTF8.GetString(await _encryptionService.DecryptAsync(new JsonWebEncryption())),
            Status = bank.Status
        };

        return Page();
    }
}
