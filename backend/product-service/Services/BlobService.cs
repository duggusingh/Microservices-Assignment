using Azure.Storage.Blobs;

namespace ProductService.Services
{
    public interface IBlobService { Task<string> UploadAsync(IFormFile file); }

    public class BlobService(IConfiguration config) : IBlobService
    {
        public async Task<string> UploadAsync(IFormFile file)
        {
            var connectionString = config["AzureBlob:ConnectionString"];
            var containerName = config["AzureBlob:ContainerName"];
            
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobName = $"{Guid.NewGuid()}_{file.FileName}";
            var blobClient = containerClient.GetBlobClient(blobName);

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }
    }
}
