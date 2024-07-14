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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentRemindersController : ControllerBase
    {
        private readonly PRN221_ProjectContext _context;

        public PaymentRemindersController(PRN221_ProjectContext context)
        {
            _context = context;
        }

        // GET: api/PaymentReminders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentReminder>>> GetPaymentReminders()
        {
            if (_context.PaymentReminders == null)
            {
                return NotFound();
            }
            return await _context.PaymentReminders.ToListAsync();
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<PaymentReminder>>> GetPaymentRemindersByTimeAndUserId([FromQuery] DateTime today, [FromQuery] DateTime endOfDate, int userId)
        {
            var paymentReminders = await _context.PaymentReminders
                .Include(p => p.User)
                .Where(p => p.ReminderDate >= today && p.ReminderDate <= endOfDate && p.UserId == userId)
                .ToListAsync();
            if (paymentReminders == null)
            {
                return NotFound();
            }
            return paymentReminders;
        }

        // GET: api/PaymentReminders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentReminder>> GetPaymentReminder(int id)
        {
            if (_context.PaymentReminders == null)
            {
                return NotFound();
            }
            var paymentReminder = await _context.PaymentReminders.FindAsync(id);

            if (paymentReminder == null)
            {
                return NotFound();
            }

            return paymentReminder;
        }

        // PUT: api/PaymentReminders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentReminder(int id, PaymentReminder paymentReminder)
        {
            if (id != paymentReminder.ReminderId)
            {
                return BadRequest();
            }

            _context.Entry(paymentReminder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentReminderExists(id))
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

        // POST: api/PaymentReminders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentReminder>> PostPaymentReminder(PaymentReminder paymentReminder)
        {
            if (_context.PaymentReminders == null)
            {
                return Problem("Entity set 'PRN221_ProjectContext.PaymentReminders'  is null.");
            }
            _context.PaymentReminders.Add(paymentReminder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentReminder", new { id = paymentReminder.ReminderId }, paymentReminder);
        }

        // DELETE: api/PaymentReminders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentReminder(int id)
        {
            if (_context.PaymentReminders == null)
            {
                return NotFound();
            }
            var paymentReminder = await _context.PaymentReminders.FindAsync(id);
            if (paymentReminder == null)
            {
                return NotFound();
            }

            _context.PaymentReminders.Remove(paymentReminder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentReminderExists(int id)
        {
            return (_context.PaymentReminders?.Any(e => e.ReminderId == id)).GetValueOrDefault();
        }
    }
}
