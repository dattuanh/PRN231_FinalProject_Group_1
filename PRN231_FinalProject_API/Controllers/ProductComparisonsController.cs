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
    public class ProductComparisonsController : ControllerBase
    {
        private readonly PRN221_ProjectContext _context;

        public ProductComparisonsController(PRN221_ProjectContext context)
        {
            _context = context;
        }

        // GET: api/ProductComparisons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductComparison>>> GetProductComparisons()
        {
          if (_context.ProductComparisons == null)
          {
              return NotFound();
          }
            return await _context.ProductComparisons.ToListAsync();
        }

        // GET: api/ProductComparisons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductComparison>> GetProductComparison(int id)
        {
          if (_context.ProductComparisons == null)
          {
              return NotFound();
          }
            var productComparison = await _context.ProductComparisons.FindAsync(id);

            if (productComparison == null)
            {
                return NotFound();
            }

            return productComparison;
        }

        // PUT: api/ProductComparisons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductComparison(int id, ProductComparison productComparison)
        {
            if (id != productComparison.ComparisonId)
            {
                return BadRequest();
            }

            _context.Entry(productComparison).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductComparisonExists(id))
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

        // POST: api/ProductComparisons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductComparison>> PostProductComparison(ProductComparison productComparison)
        {
          if (_context.ProductComparisons == null)
          {
              return Problem("Entity set 'PRN221_ProjectContext.ProductComparisons'  is null.");
          }
            _context.ProductComparisons.Add(productComparison);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductComparison", new { id = productComparison.ComparisonId }, productComparison);
        }

        // DELETE: api/ProductComparisons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductComparison(int id)
        {
            if (_context.ProductComparisons == null)
            {
                return NotFound();
            }
            var productComparison = await _context.ProductComparisons.FindAsync(id);
            if (productComparison == null)
            {
                return NotFound();
            }

            _context.ProductComparisons.Remove(productComparison);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductComparisonExists(int id)
        {
            return (_context.ProductComparisons?.Any(e => e.ComparisonId == id)).GetValueOrDefault();
        }
    }
}
