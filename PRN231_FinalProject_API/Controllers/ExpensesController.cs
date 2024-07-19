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
    public class ExpensesController : ControllerBase
    {
        private readonly PRN221_ProjectContext _context;

        public ExpensesController(PRN221_ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
          if (_context.Expenses == null)
          {
              return NotFound();
          }
            return await _context.Expenses.ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
          if (_context.Expenses == null)
          {
              return NotFound();
          }
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }
<<<<<<< HEAD

=======
>>>>>>> 5036270e7a62d2dc9064e3987a59c2e124fe726a
        [HttpGet("User/{uid}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetUserExpense(int uid)
        {
            if (_context.Expenses == null)
            {
                return NotFound();
            }
<<<<<<< HEAD
            var expenses = await _context.Expenses.Where(i => i.UserId == uid).ToListAsync();

            if (expenses == null)
=======
            var expense = await _context.Expenses.Where(e=>e.UserId==uid).ToListAsync();

            if (expense == null)
>>>>>>> 5036270e7a62d2dc9064e3987a59c2e124fe726a
            {
                return NotFound();
            }

<<<<<<< HEAD
            return expenses;
=======
            return expense;
>>>>>>> 5036270e7a62d2dc9064e3987a59c2e124fe726a
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
<<<<<<< HEAD
        public async Task<IActionResult> PutExpense(int id, [FromBody]Expense expense)
=======
        public async Task<IActionResult> PutExpense(int id, Expense expense)
>>>>>>> 5036270e7a62d2dc9064e3987a59c2e124fe726a
        {
            if (id != expense.ExpenseId)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
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

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
          if (_context.Expenses == null)
          {
              return Problem("Entity set 'PRN221_ProjectContext.Expenses'  is null.");
          }
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpense", new { id = expense.ExpenseId }, expense);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            if (_context.Expenses == null)
            {
                return NotFound();
            }
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseExists(int id)
        {
            return (_context.Expenses?.Any(e => e.ExpenseId == id)).GetValueOrDefault();
        }
<<<<<<< HEAD
=======

        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetTotalExpense(int id)
        {
            
            

            var totalExpense = await _context.Expenses
                .Where(e => e.UserId == id)
                .SumAsync(e => e.Amount);

            return Ok(totalExpense);
        }

        [HttpGet("recent/{userId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetRecentExpenses(int userId)
        {
            var recentExpenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.ExpenseDate)
                .Take(5)
                .Select(e => new
                {
                    e.ExpenseId,
                    e.ExpenseDate,
                    e.Amount,
                    e.Category,
                    e.Description
                })
                .ToListAsync();

            if (recentExpenses == null || !recentExpenses.Any())
            {
                return NotFound("No recent expenses found for this user.");
            }

            return Ok(recentExpenses);
        }
>>>>>>> 5036270e7a62d2dc9064e3987a59c2e124fe726a
    }
}
