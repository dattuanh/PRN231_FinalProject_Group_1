﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN231_FinalProject_Client.Models;

namespace PRN231_FinalProject_Client.Pages.Debts
{
    public class DeleteModel : PageModel
    {
        //private readonly PRN231_FinalProject_Client.Models.PRN221_ProjectContext _context;

        //public DeleteModel(PRN231_FinalProject_Client.Models.PRN221_ProjectContext context)
        //{
        //    _context = context;
        //}

        [BindProperty]
        public DebtsLoan DebtsLoan { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //DebtsLoan = await _context.DebtsLoans
            //    .Include(d => d.User).FirstOrDefaultAsync(m => m.DebtLoanId == id);

            //if (DebtsLoan == null)
            //{
            //    return NotFound();
            //}
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //DebtsLoan = await _context.DebtsLoans.FindAsync(id);

            //if (DebtsLoan != null)
            //{
            //    _context.DebtsLoans.Remove(DebtsLoan);
            //    await _context.SaveChangesAsync();
            //}

            return RedirectToPage("./Index");
        }
    }
}
