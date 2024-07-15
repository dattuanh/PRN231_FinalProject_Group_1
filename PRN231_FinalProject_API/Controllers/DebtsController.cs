using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231_FinalProject_API.DTOs.Debts;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebtsLoan>>> GetDebtsLoans()
        {
          if (_context.DebtsLoans == null)
          {
              return NotFound();
          }
            return await _context.DebtsLoans.ToListAsync();
        }

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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDebts(int id, [FromBody] DebtsLoan debtsLoan)
        {
            if (debtsLoan == null)
            {
                return BadRequest();
            }

            var existingDebts = await _context.DebtsLoans.FindAsync(id);
            if (existingDebts == null)
            {
                return NotFound();
            }

            existingDebts.UserId = debtsLoan.UserId;
            existingDebts.Type = debtsLoan.Type;
            existingDebts.Amount = debtsLoan.Amount;
            existingDebts.InterestRate = debtsLoan.InterestRate;
            existingDebts.Description = debtsLoan.Description;

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

        [HttpPost]
        public async Task<ActionResult<DebtsLoan>> PostDebtsLoan(DebtsDTO dto)
        {
          if (_context.DebtsLoans == null)
          {
              return Problem("Entity set 'PRN221_ProjectContext.DebtsLoans'  is null.");
          }

            DebtsLoan debtsLoan = new DebtsLoan
            {
                UserId = dto.UserId,
                Type = dto.Type,
                Amount = dto.Amount,
                InterestRate = dto.InterestRate,
                Description = dto.Description
            };

            _context.DebtsLoans.Add(debtsLoan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDebtsLoan", new { id = debtsLoan.DebtLoanId }, debtsLoan);
        }

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
