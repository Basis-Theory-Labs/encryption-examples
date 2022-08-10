using System.Text;
using Common.Data;
using Common.Data.Entities;
using CustomerPortal.Areas.Banks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenKMS.Abstractions;
using OpenKMS.Models;

namespace CustomerPortal.Areas.Banks.Pages;

public class EditModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly IEncryptionService _encryptionService;

    [BindProperty]
    public EditBankModel Bank { get; set; } = default!;

    public EditModel(BankDbContext context, IEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank =  await _context.Banks.FirstOrDefaultAsync(m => m.Id == id);
        if (bank == null) return NotFound();

        Bank = new EditBankModel
        {
            Id = bank.Id,
            RoutingNumber = Encoding.UTF8.GetString(await _encryptionService.DecryptAsync(JsonWebEncryption.FromCompactSerializationFormat(bank.RoutingNumber))),
            AccountNumber = Encoding.UTF8.GetString(await _encryptionService.DecryptAsync(JsonWebEncryption.FromCompactSerializationFormat(bank.AccountNumber))),
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var bank =  await _context.Banks.FirstOrDefaultAsync(m => m.Id == Bank.Id);
        if (bank == null) return NotFound();

        // bank.RoutingNumber = await _encryptionService.Encrypt(Bank.RoutingNumber);
        // bank.AccountNumber = await _encryptionService.Encrypt(Bank.AccountNumber);
        bank.Status = ProcessStatus.PENDING;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private bool BankExists(Guid id) => _context.Banks.Any(e => e.Id == id);
}
