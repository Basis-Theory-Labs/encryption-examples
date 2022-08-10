using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoEncryption.Data;
using NoEncryption.Data.Entities;

namespace NoEncryption.Pages.Banks;

public class IndexModel : PageModel
{
    private readonly BankDbContext _context;
    public IList<Bank> Bank { get;set; } = default!;

    public IndexModel(BankDbContext context) => _context = context;

    public async Task OnGetAsync()
    {
        Bank = await _context.Banks.ToListAsync();
    }
}
