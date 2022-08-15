using System.Text;
using Common.Data;
using Common.Encryption;
using CustomerPortal.Areas.Banks.Models;
using Encryption;
using Encryption.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CustomerPortal.Areas.Banks.Pages;

public class DeleteModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly IEncryptionService _encryptionService;

    [BindProperty]
    public BankModel Bank { get; set; } = default!;

    public DeleteModel(BankDbContext context, IEncryptionService encryptionService)
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

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank = await _context.Banks.FindAsync(id);
        if (bank == null) return RedirectToPage("./Index");

        _context.Banks.Remove(bank);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
