using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoEncryption.Data;
using NoEncryption.Data.Entities;

namespace NoEncryption.Pages.Banks;

public class EditModel : PageModel
{
    private readonly BankDbContext _context;

    [BindProperty]
    public Bank Bank { get; set; } = default!;

    public EditModel(BankDbContext context) => _context = context;

    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id == null) return NotFound();

        var bank =  await _context.Banks.FirstOrDefaultAsync(m => m.Id == id);
        if (bank == null) return NotFound();

        Bank = bank;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _context.Attach(Bank).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BankExists(Bank.Id)) return NotFound();

            throw;
        }

        return RedirectToPage("./Index");
    }

    private bool BankExists(Guid id) => _context.Banks.Any(e => e.Id == id);
}
