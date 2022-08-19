using Common.Data;
using CustomerPortal.Areas.Banks.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Areas.Banks.Pages;

public class IndexModel : PageModel
{
    private readonly BankDbContext _context;

    public IList<BankModel> Banks { get;set; } = default!;

    public IndexModel(BankDbContext context) => _context = context;

    public async Task OnGetAsync()
    {
        var banks = await _context.Banks.ToListAsync();

        Banks = banks.Select(bank => new BankModel
        {
            Id = bank.Id,
            RoutingNumber = bank.RoutingNumber,
            AccountNumber = bank.AccountNumber,
            Status = bank.Status
        }).ToList();
    }
}
