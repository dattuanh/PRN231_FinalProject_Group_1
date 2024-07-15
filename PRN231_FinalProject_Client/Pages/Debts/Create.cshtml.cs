using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Debts
{
    public class CreateModel : PageModel
    {
        private readonly PRN231_FinalProject_Client.Models.PRN221_ProjectContext _context;

        public CreateModel(PRN231_FinalProject_Client.Models.PRN221_ProjectContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public DebtsLoan DebtsLoan { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DebtsLoans.Add(DebtsLoan);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
