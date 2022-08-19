using Common.Data;
using Common.Data.Entities;
using CustomerPortal.Areas.Banks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerPortal.Areas.Banks.Pages;

public class CreateModel : PageModel
{
    private readonly BankDbContext _context;

    [BindProperty]
    public CreateBankModel Bank { get; set; } = default!;

    public CreateModel(BankDbContext context) => _context = context;

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return Page();

        var bank = new Bank
        {
            Id = Guid.NewGuid(),
            Status = ProcessStatus.PENDING,
            RoutingNumber = Bank.RoutingNumber,
            AccountNumber = Bank.AccountNumber
        };

        _context.Banks.Add(bank);
        await _context.SaveChangesAsync(cancellationToken);

        return RedirectToPage("./Index");
    }
}
