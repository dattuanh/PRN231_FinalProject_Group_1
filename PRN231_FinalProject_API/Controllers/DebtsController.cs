using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_FinalProject_API.Models;

namespace PRN231_FinalProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebtsController : ControllerBase
    {
        private readonly PRN221_ProjectContext _context;

        public DebtsController(PRN221_ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Debts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebtsLoan>>> GetDebtsLoans()
        {
          if (_context.DebtsLoans == null)
          {
              return NotFound();
          }
            return await _context.DebtsLoans.ToListAsync();
        }

        // GET: api/Debts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DebtsLoan>> GetDebtsLoan(int id)
        {
          if (_context.DebtsLoans == null)
          {
              return NotFound();
          }
            var debtsLoan = await _context.DebtsLoans.FindAsync(id);

            if (debtsLoan == null)
            {
                return NotFound();
            }

            return debtsLoan;
        }

        // PUT: api/Debts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDebtsLoan(int id, DebtsLoan debtsLoan)
        {
            if (id != debtsLoan.DebtLoanId)
            {
                return BadRequest();
            }

            _context.Entry(debtsLoan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DebtsLoanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Debts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DebtsLoan>> PostDebtsLoan(DebtsLoan debtsLoan)
        {
          if (_context.DebtsLoans == null)
          {
              return Problem("Entity set 'PRN221_ProjectContext.DebtsLoans'  is null.");
          }
            _context.DebtsLoans.Add(debtsLoan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDebtsLoan", new { id = debtsLoan.DebtLoanId }, debtsLoan);
        }

        // DELETE: api/Debts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDebtsLoan(int id)
        {
            if (_context.DebtsLoans == null)
            {
                return NotFound();
            }
            var debtsLoan = await _context.DebtsLoans.FindAsync(id);
            if (debtsLoan == null)
            {
                return NotFound();
            }

            _context.DebtsLoans.Remove(debtsLoan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DebtsLoanExists(int id)
        {
            return (_context.DebtsLoans?.Any(e => e.DebtLoanId == id)).GetValueOrDefault();
        }
    }
}
