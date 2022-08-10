using BasicEncryption.Areas.Banks.Models;
using BasicEncryption.Data;
using BasicEncryption.Data.Entities;
using BasicEncryption.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BasicEncryption.Areas.Banks.Pages;

public class EditModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly EncryptionService _encryptionService;

    [BindProperty]
    public EditBankModel Bank { get; set; } = default!;

    public EditModel(BankDbContext context, EncryptionService encryptionService)
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
            RoutingNumber = await _encryptionService.Decrypt(bank.RoutingNumber),
            AccountNumber = await _encryptionService.Decrypt(bank.AccountNumber)
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var bank =  await _context.Banks.FirstOrDefaultAsync(m => m.Id == Bank.Id);
        if (bank == null) return NotFound();

        bank.RoutingNumber = await _encryptionService.Encrypt(Bank.RoutingNumber);
        bank.AccountNumber = await _encryptionService.Encrypt(Bank.AccountNumber);
        bank.Status = ProcessStatus.PENDING;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private bool BankExists(Guid id) => _context.Banks.Any(e => e.Id == id);
}
