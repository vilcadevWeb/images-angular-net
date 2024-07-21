using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using productoapi.Enums;

namespace productoapi.Services
{
    public class AzureBlobStorageService: IAzureBlobStorageService
    {

        private readonly string azureStorageConnectionString;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            this.azureStorageConnectionString = configuration.GetValue<string>("AzureBlobStorageConnection");
        }

        public async Task DeleteAsync(ContainerEnum container, string blobFilename)
        {
            var containerName = Enum.GetName(typeof(ContainerEnum), container).ToLower();
            var blobContainerClient = new BlobContainerClient(this.azureStorageConnectionString, containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobFilename);

            try
            {
                await blobClient.DeleteAsync();
            }
            catch
            {
                Console.WriteLine("se produjo un error");
                throw;
            }
        }

        public async Task<string> UploadAsync(IFormFile file, ContainerEnum container, string blobName = null)
        {
            if (file.Length == 0) return null;
            var containerName = Enum.GetName(typeof(ContainerEnum), container).ToLower();
            var blobContainerClient = new BlobContainerClient(this.azureStorageConnectionString, containerName);

            if (string.IsNullOrEmpty(blobName))
            {
                blobName = Guid.NewGuid().ToString();
            }

            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var blobHttpHeader = new BlobHttpHeaders { ContentType = file.ContentType };

            await using (Stream stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
            }
            return blobName;
        }

    }

    public interface IAzureBlobStorageService
    {
        Task<string> UploadAsync(IFormFile file, ContainerEnum container, string blobName = null);
        Task DeleteAsync(ContainerEnum container, string blobFilename);
    }
}
