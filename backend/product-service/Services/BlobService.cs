using Azure.Storage.Blobs;

namespace ProductService.Services
{
    public class BlobService(BlobServiceClient blobClient, IConfiguration config) : IBlobService
    {
        public async Task<string> UploadAsync(IFormFile file)
        {
            var containerName = config["AzureBlob:ContainerName"]; 
            var container = blobClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();

            var blobName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var client = container.GetBlobClient(blobName);

            await using var stream = file.OpenReadStream();
            await client.UploadAsync(stream, true);

            return client.Uri.ToString(); // Returns the ImageUrl
        }
    }
}
