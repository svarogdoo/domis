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
    IAzureBlobService abs
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
            return false;

        if (image.ImageTypeId == 1)
            return false;

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

        // sada bi trebalo zvati azure servis i uploadovati svaki od novododatih urlova
        // kao response vratiti putanju do azure slike

        var imageUrl = await abs.UploadImage(request.DataUrls[0]);

        // nakon toga, pozvati metodu repozitorijuma da upise u tabele image i product_image
        // za image tabelu nam treba: image_name, image_url
        // za product_image tabelu nam treba: product_id, image_id(dobijamo iz prethodne metode)

        //todo: implement
        return await repository.AddGalleryImages();
    }
}