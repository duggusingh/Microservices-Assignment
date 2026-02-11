[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase {
    // Standard CRUD: GET, POST, PUT, DELETE

    [HttpPost("{id}/image")] 
    // Multipart/form-data upload
    public async Task<IActionResult> UploadImage(int id, IFormFile file, [FromServices] IBlobService blobService) {
        var imageUrl = await blobService.UploadAsync(file); // Upload to Azure
        var product = await _context.Products.FindAsync(id);
        product.ImageUrl = imageUrl; // Persist URL in MSSQL
        await _context.SaveChangesAsync();
        return Ok(new { ImageUrl = imageUrl }); // Return URL in response
    }
}
