using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Debts
{
    public class EditModel : PageModel
    {
        //private readonly PRN231_FinalProject_Client.Models.PRN221_ProjectContext _context;

        //public EditModel(PRN231_FinalProject_Client.Models.PRN221_ProjectContext context)
        //{
        //    _context = context;
        //}

        [BindProperty]
        public DebtsLoan DebtsLoan { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
           // if (id == null)
           // {
           //     return NotFound();
           // }

           // DebtsLoan = await _context.DebtsLoans
           //     .Include(d => d.User).FirstOrDefaultAsync(m => m.DebtLoanId == id);

           // if (DebtsLoan == null)
           // {
           //     return NotFound();
           // }
           //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //_context.Attach(DebtsLoan).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!DebtsLoanExists(DebtsLoan.DebtLoanId))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Index");
        }
    }
}
