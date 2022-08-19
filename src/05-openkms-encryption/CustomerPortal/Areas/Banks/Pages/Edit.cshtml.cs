using Common.Data;
using Common.Data.Entities;
using CustomerPortal.Areas.Banks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Areas.Banks.Pages;

public class EditModel : PageModel
{
    private readonly BankDbContext _context;

    [BindProperty]
    public EditBankModel Bank { get; set; } = default!;

    public EditModel(BankDbContext context) => _context = context;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank =  await _context.Banks.FirstOrDefaultAsync(m => m.Id == id);
        if (bank == null) return NotFound();

        Bank = new EditBankModel
        {
            Id = bank.Id,
            RoutingNumber = bank.RoutingNumber,
            AccountNumber = bank.AccountNumber,
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var bank =  await _context.Banks.FirstOrDefaultAsync(m => m.Id == Bank.Id);
        if (bank == null) return NotFound();

        bank.RoutingNumber = Bank.RoutingNumber;
        bank.AccountNumber = Bank.AccountNumber;
        bank.Status = ProcessStatus.PENDING;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private bool BankExists(Guid id) => _context.Banks.Any(e => e.Id == id);
}
