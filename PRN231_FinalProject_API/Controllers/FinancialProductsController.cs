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
    public class FinancialProductsController : ControllerBase
    {
        private readonly PRN221_ProjectContext _context;

        public FinancialProductsController(PRN221_ProjectContext context)
        {
            _context = context;
        }

        // GET: api/FinancialProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialProduct>>> GetFinancialProducts()
        {
          if (_context.FinancialProducts == null)
          {
              return NotFound();
          }
            return await _context.FinancialProducts.ToListAsync();
        }

        // GET: api/FinancialProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialProduct>> GetFinancialProduct(int id)
        {
          if (_context.FinancialProducts == null)
          {
              return NotFound();
          }
            var financialProduct = await _context.FinancialProducts.FindAsync(id);

            if (financialProduct == null)
            {
                return NotFound();
            }

            return financialProduct;
        }

        // PUT: api/FinancialProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinancialProduct(int id, FinancialProduct financialProduct)
        {
            if (id != financialProduct.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(financialProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancialProductExists(id))
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

        // POST: api/FinancialProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FinancialProduct>> PostFinancialProduct(FinancialProduct financialProduct)
        {
          if (_context.FinancialProducts == null)
          {
              return Problem("Entity set 'PRN221_ProjectContext.FinancialProducts'  is null.");
          }
            _context.FinancialProducts.Add(financialProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinancialProduct", new { id = financialProduct.ProductId }, financialProduct);
        }

        // DELETE: api/FinancialProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancialProduct(int id)
        {
            if (_context.FinancialProducts == null)
            {
                return NotFound();
            }
            var financialProduct = await _context.FinancialProducts.FindAsync(id);
            if (financialProduct == null)
            {
                return NotFound();
            }

            _context.FinancialProducts.Remove(financialProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinancialProductExists(int id)
        {
            return (_context.FinancialProducts?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
