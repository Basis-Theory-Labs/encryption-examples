using Common.Data;
using Common.Data.Entities;
using Common.Encryption;
using CustomerPortal.Areas.Banks.Models;
using Encryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerPortal.Areas.Banks.Pages;

public class CreateModel : PageModel
{
    private readonly BankDbContext _context;
    private readonly IEncryptionService _encryptionService;

    [BindProperty]
    public CreateBankModel Bank { get; set; } = default!;

    public CreateModel(BankDbContext context, IEncryptionService encryptionService)
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
            RoutingNumber = (await _encryptionService.EncryptAsync(Bank.RoutingNumber, "default", cancellationToken)).ToCompactSerializationFormat(),
            AccountNumber = (await _encryptionService.EncryptAsync(Bank.AccountNumber, "default", cancellationToken)).ToCompactSerializationFormat(),
        };

        _context.Banks.Add(bank);
        await _context.SaveChangesAsync(cancellationToken);

        return RedirectToPage("./Index");
    }
}
