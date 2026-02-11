namespace ProductService.Services
{
    public interface IBlobService
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
