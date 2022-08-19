using Common.Data;
using CustomerPortal.Areas.Banks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Areas.Banks.Pages;

public class DetailsModel : PageModel
{
    private readonly BankDbContext _context;

    public BankModel Bank { get; set; } = default!;

    public DetailsModel(BankDbContext context) => _context = context;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank = await _context.Banks.FirstOrDefaultAsync(m => m.Id == id);
        if (bank == null) return NotFound();

        Bank = new BankModel
        {
            Id = bank.Id,
            RoutingNumber = bank.RoutingNumber,
            AccountNumber = bank.AccountNumber,
            Status = bank.Status
        };

        return Page();
    }
}
