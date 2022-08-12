using Common.Data;
using Common.Data.Entities;
using Common.Encryption;
using CustomerPortal.Areas.Banks.Models;
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

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return Page();

        var bank = new Bank
        {
            Id = Guid.NewGuid(),
            Status = ProcessStatus.PENDING,
            RoutingNumber = await _encryptionService.Encrypt(Bank.RoutingNumber, cancellationToken),
            AccountNumber = await _encryptionService.Encrypt(Bank.AccountNumber, cancellationToken),
        };

        _context.Banks.Add(bank);
        await _context.SaveChangesAsync(cancellationToken);

        return RedirectToPage("./Index");
    }
}
