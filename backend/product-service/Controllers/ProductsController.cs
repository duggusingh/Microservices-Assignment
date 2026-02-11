using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.Services;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(ProductDbContext context, IBlobService blobService) : ControllerBase
    {
        // 1. READ ALL (GET)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await context.Products.ToListAsync();
        }

        // 2. READ ONE (GET /id)
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        // 3. CREATE (POST)
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // 4. UPDATE (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId) return BadRequest("ID mismatch");

            context.Entry(product).State = EntityState.Modified;
            
            try { await context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Products.Any(e => e.ProductId == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // 5. DELETE (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) return NotFound();

            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();
        }

        // 6. IMAGE UPLOAD (Special Requirement)
        [HttpPost("{id}/image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) return NotFound("Product not found");

            var imageUrl = await blobService.UploadAsync(file);
            product.ImageUrl = imageUrl;
            
            await context.SaveChangesAsync();
            return Ok(new { ImageUrl = imageUrl });
        }
    }
}
