using Azure.Storage.Blobs;

namespace domis.api.Services;

public interface IAzureBlobService
{
    Task<string> UploadImage(IFormFile image);
    Task DeleteImage(string imageId);
}

public class AzureBlobService(BlobContainerClient containerClient) : IAzureBlobService
{
    //TODO: actually implement
    public async Task<string> UploadImage(IFormFile image)
    {
        var blobName = Guid.NewGuid().ToString();
        var blobClient = containerClient.GetBlobClient(blobName);

        await using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return blobClient.Uri.ToString();    }

    public Task DeleteImage(string imageId)
    {
        throw new NotImplementedException();
    }
}