using Azure.Storage.Blobs;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace domis.api.Services;

public interface IAzureBlobService
{
    Task<List<string>> UploadBase64Images(string productName, List<string> dataUrls);
    Task DeleteImage(string imageUrl);
}

public class AzureBlobService(BlobContainerClient containerClient) : IAzureBlobService
{
    public async Task<List<string>> UploadBase64Images(string productName, List<string> dataUrls)
    {
        var imageUrls = new List<string>();

        foreach (var dataUrl in dataUrls.Where(dataUrl => !string.IsNullOrEmpty(dataUrl)))
        {
            try
            {
                var base64Data = dataUrl.Split(',')[1];
                var imageBytes = Convert.FromBase64String(base64Data);

                using var image = Image.Load(imageBytes);
                using var outputStream = new MemoryStream();
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(1200, 1200),
                    Mode = ResizeMode.Max
                }));

                var webpEncoder = new WebpEncoder
                {
                    Quality = 75,
                    Method = WebpEncodingMethod.BestQuality
                };

                await image.SaveAsync(outputStream, webpEncoder);

                outputStream.Position = 0;

                var blobName = $"{SanitizeProductName(productName)}_{Guid.NewGuid()}.webp";
                var blobClient = containerClient.GetBlobClient(blobName);

                await blobClient.UploadAsync(outputStream, overwrite: true);

                imageUrls.Add(blobClient.Uri.ToString());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to process or upload image for product: {ProductName}", productName);
            }
        }

        return imageUrls;
    }
    
    public async Task DeleteImage(string imageUrl)
    {
        try
        {
            var blobName = GetBlobNameFromUrl(imageUrl);
            var blobClient = containerClient.GetBlobClient(blobName);
            
            var blobExists = await blobClient.ExistsAsync();
            if (!blobExists.Value)
            {
                Log.Warning("Blob does not exist: {BlobName}", blobName);
                return;
            }

            await blobClient.DeleteAsync();
            Log.Information("Successfully deleted image: {BlobName}", blobName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to delete image: {ImageUrl}", imageUrl);
        }
    }

    private static string GetBlobNameFromUrl(string imageUrl)
    {
        Uri uri = new(imageUrl);
        return uri.Segments.Last();
    }


    private static string SanitizeProductName(string productName)
    {
        return productName
            .Replace(" ", "_") // Replace spaces with underscores
            .Replace("/", "-") // Replace slashes with dashes
            .Replace("\\", "-") // Replace backslashes with dashes
            .Replace(":", "-") // Replace colons with dashes
            .Replace("*", "-") // Replace asterisks with dashes
            .Replace("?", "-") // Replace question marks with dashes
            .Replace("\"", "-") // Replace double quotes with dashes
            .Replace("<", "-") // Replace less-than signs with dashes
            .Replace(">", "-") // Replace greater-than signs with dashes
            .Replace("|", "-"); // Replace pipes with dashes
    }
}