using CustomerPortal.Areas.Banks.Models;
using CustomerPortal.Data;
using CustomerPortal.Data.Entities;
using CustomerPortal.Encryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerPortal.Areas.Banks.Pages;

public class CreateModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly EncryptionService _encryptionService;

    [BindProperty]
    public CreateBankModel Bank { get; set; } = default!;

    public CreateModel(BankDbContext context, EncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var bank = new Bank
        {
            Id = Guid.NewGuid(),
            Status = ProcessStatus.PENDING,
            RoutingNumber = await _encryptionService.Encrypt(Bank.RoutingNumber),
            AccountNumber = await _encryptionService.Encrypt(Bank.AccountNumber)
        };

        _context.Banks.Add(bank);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
