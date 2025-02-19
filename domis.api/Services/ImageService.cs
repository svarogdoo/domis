using domis.api.Common;
using domis.api.DTOs.Image;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IImageService
{
    Task<List<ImageGetDto>> GetImages(int productId);
    Task<bool> DeleteGalleryImage(int productId, int imageId);
    Task<bool> AddGalleryImages(int productId, AddGalleryImagesRequest request);
    Task<bool> UpdateFeaturedImage(int productId, string dataUrl);
}

public class ImageService(
    IImageRepository repository, 
    IProductRepository productRepo,
    IAzureBlobService azureBlobService
    ) : IImageService
{
    public async Task<List<ImageGetDto>> GetImages(int productId)
        => await repository.GetImages(productId);

    public async Task<bool> DeleteGalleryImage(int productId, int imageId)
    {
        var image = await repository.GetProductImageById(productId, imageId);
        if (image == null)
            throw new NotFoundException("Slika nije pronađena.");

        if (image.ImageTypeId == 1)
            throw new ArgumentException("Slika je naslovna slika. Nije dozvoljeno brisanje, samo promena.");
 
        await repository.DeleteProductImage(imageId);
        await repository.DeleteImage(imageId);

        await azureBlobService.DeleteImage(image.BlobUrl);

        return true;
    }

    public async Task<bool> AddGalleryImages(int productId, AddGalleryImagesRequest request)
    {
        var exists = await productRepo.ProductExists(productId);
        if (!exists)
            throw new NotFoundException("Product does not exist.");

        if (request.DataUrls.Count == 0)
            throw new ArgumentException("Data URLs has not been provided.");

        var productName = await productRepo.GetProductName(productId);
        
        var uploadedImageUrls = await azureBlobService.UploadBase64Images(productName ?? string.Empty, request.DataUrls);
        
        return await repository.AddGalleryImages(productId, productName ?? string.Empty, uploadedImageUrls, 2);
    }

    public async Task<bool> UpdateFeaturedImage(int productId, string dataUrl)
    {
        var exists = await productRepo.ProductExists(productId);
        if (!exists)
            throw new NotFoundException("Product does not exist.");   
        
        if (string.IsNullOrEmpty(dataUrl))
            throw new ArgumentException("Data URLs has not been provided.");
        
        var productName = await productRepo.GetProductName(productId);

        var uploadedImageUrl = await azureBlobService.UploadBase64Images(productName ?? string.Empty, [dataUrl]);
        
        var result =  await repository.UpdateFeaturedImage(productId, productName ?? string.Empty, uploadedImageUrl[0]);
        if (result > 0)
        {
            await azureBlobService.DeleteImage(dataUrl);
        }

        return true;
    }
}