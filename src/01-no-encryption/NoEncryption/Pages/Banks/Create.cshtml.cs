using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NoEncryption.Data.Entities;

namespace NoEncryption.Pages.Banks;

public class CreateModel : PageModel
{
    private readonly Data.BankDbContext _context;

    [BindProperty]
    public Bank Bank { get; set; } = default!;

    public CreateModel(Data.BankDbContext context) => _context = context;

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        Bank.Id = Guid.NewGuid();
        Bank.Status = ProcessStatus.PENDING;

        _context.Banks.Add(Bank);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
