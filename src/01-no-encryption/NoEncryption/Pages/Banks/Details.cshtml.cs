using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoEncryption.Data;
using NoEncryption.Data.Entities;

namespace NoEncryption.Pages.Banks;

public class DetailsModel : PageModel
{
    private readonly BankDbContext _context;

    public Bank Bank { get; set; } = default!;

    public DetailsModel(BankDbContext context) => _context = context;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank = await _context.Banks.FirstOrDefaultAsync(m => m.Id == id);
        if (bank == null) return NotFound();

        Bank = bank;
        return Page();
    }
}
