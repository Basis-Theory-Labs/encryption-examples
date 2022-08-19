using Common.Constants;
using Common.Data;
using Common.Data.Entities;
using CustomerPortal.Areas.Banks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenKMS.Abstractions;
using OpenKMS.Extensions;

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
            RoutingNumber = (await _encryptionService.EncryptAsync(Bank.RoutingNumber, EncryptionSchemes.BankRoutingNumber, cancellationToken)).ToCompactSerializationFormat(),
            AccountNumber = (await _encryptionService.EncryptAsync(Bank.AccountNumber, EncryptionSchemes.BankAccountNumber, cancellationToken)).ToCompactSerializationFormat(),
        };

        _context.Banks.Add(bank);
        await _context.SaveChangesAsync(cancellationToken);

        return RedirectToPage("./Index");
    }
}
