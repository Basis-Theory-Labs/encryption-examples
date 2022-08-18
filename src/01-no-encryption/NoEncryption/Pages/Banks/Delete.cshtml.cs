using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoEncryption.Data.Entities;

namespace NoEncryption.Pages.Banks;

public class DeleteModel : PageModel
{
    private readonly Data.BankDbContext _context;

    [BindProperty]
    public Bank Bank { get; set; } = default!;

    public DeleteModel(Data.BankDbContext context) => _context = context;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank = await _context.Banks.FirstOrDefaultAsync(m => m.Id == id);
        if (bank == null) return NotFound();

        Bank = bank;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank = await _context.Banks.FindAsync(id);
        if (bank == null) return RedirectToPage("./Index");

        Bank = bank;
        _context.Banks.Remove(Bank);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
