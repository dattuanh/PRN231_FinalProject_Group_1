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
    public class BudgetsController : ControllerBase
    {
        private readonly PRN221_ProjectContext _context;

        public BudgetsController(PRN221_ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Budgets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets()
        {
          if (_context.Budgets == null)
          {
              return NotFound();
          }
            return await _context.Budgets.ToListAsync();
        }

        // GET: api/Budgets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Budget>> GetBudget(int id)
        {
          if (_context.Budgets == null)
          {
              return NotFound();
          }
            var budget = await _context.Budgets.FindAsync(id);

            if (budget == null)
            {
                return NotFound();
            }

            return budget;
        }
        [HttpGet("User/{uid}")]
        public async Task<ActionResult<IEnumerable<Budget>>> GetUserBudget(int uid)
        {
            if (_context.Budgets == null)
            {
                return NotFound();
            }
            var budget = await _context.Budgets.Where(b=>b.UserId==uid).ToListAsync();

            if (budget == null)
            {
                return NotFound();
            }

            return budget;
        }

        // PUT: api/Budgets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudget(int id, Budget budget)
        {
            if (id != budget.BudgetId)
            {
                return BadRequest();
            }

            _context.Entry(budget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
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

        public class BudgetUpdateModel
        {
            public decimal TotalBudget { get; set; }
        }

        [HttpPut("User/{uid}")]
        public async Task<ActionResult<Budget>> UpdateUserBudget(int uid, [FromBody] BudgetUpdateModel model)
        {
            var budget = _context.Budgets.FirstOrDefault(b => b.UserId == uid);
            if (budget == null)
            {
                return NotFound();
            }
            budget.TotalBudget = model.TotalBudget;
            await _context.SaveChangesAsync();
            return Ok(budget);
        }

        // POST: api/Budgets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Budget>> PostBudget(Budget budget)
        {
          if (_context.Budgets == null)
          {
              return Problem("Entity set 'PRN221_ProjectContext.Budgets'  is null.");
          }
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudget", new { id = budget.BudgetId }, budget);
        }

        // DELETE: api/Budgets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            if (_context.Budgets == null)
            {
                return NotFound();
            }
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetExists(int id)
        {
            return (_context.Budgets?.Any(e => e.BudgetId == id)).GetValueOrDefault();
        }
    }
}
