using domis.api.DTOs.Image;
using domis.api.Repositories;

namespace domis.api.Services;

public interface IImageService
{
    Task<List<ImageGetDto>> GetImages(int productId);
    Task<bool> AddProductImage(int productId, IFormFile image, int imageTypeId);
    Task<bool> DeleteProductImageAsync(int productId, int imageId);
}

public class ImageService(
    IImageRepository repository, 
    IProductRepository productRepo//, IAzureBlobService azureBlobService
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

}