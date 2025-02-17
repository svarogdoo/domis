using domis.api.Common;
using domis.api.DTOs.Image;
using domis.api.Models;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IImageService
{
    Task<List<ImageGetDto>> GetImages(int productId);
    Task<bool> AddProductImage(int productId, IFormFile image, int imageTypeId);
    Task<bool> DeleteProductImageAsync(int productId, int imageId);
    
    Task<bool> AddGalleryImages(int productId, AddGalleryImagesRequest request);
}

public class ImageService(
    IImageRepository repository, 
    IProductRepository productRepo,
    IAzureBlobService azureBlobService
    ) : IImageService
{
    public async Task<List<ImageGetDto>> GetImages(int productId)
        => await repository.GetImages(productId);

    public async Task<bool> AddProductImage(int productId, IFormFile image, int imageTypeId)
    {
        if (!await productRepo.ProductExists(productId))
            return false;

        //var imageUrl = await azureBlobService.UploadImage(image);

        if (imageTypeId == 1) 
        {
            await repository.DeleteFeaturedImage(productId);
        }

        var imageId = await repository.AddProductImage(productId, "imageUrl", imageTypeId);

        return true;
    }

    public async Task<bool> DeleteProductImageAsync(int productId, int imageId)
    {
        var image = await repository.GetProductImageById(productId, imageId);
        if (image == null)
            throw new NotFoundException("Slika nije pronađena.");

        if (image.ImageTypeId == 1)
            throw new ArgumentException("Slika je naslovna slika. Nije dozvoljeno brisanje, samo promena.");
 
        await repository.DeleteProductImage(imageId);

        //await azureBlobService.DeleteImage(image.ImageUrl);

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
        
        return await repository.AddImages(productId, productName ?? string.Empty, uploadedImageUrls, 2);
    }
}