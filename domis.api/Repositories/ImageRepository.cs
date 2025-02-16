using System.Data;
using Dapper;
using domis.api.DTOs.Image;
using domis.api.Repositories.Queries;

namespace domis.api.Repositories;

public interface IImageRepository
{
    Task<List<ImageGetDto>> GetImages(int productId);
    Task<int> AddProductImage(int productId, string imageUrl, int imageTypeId);
    Task DeleteFeaturedImage(int productId);
    Task<ProductImageDto?> GetProductImageById(int productId, int imageId);
    Task DeleteProductImage(int imageId);
    
    Task<bool> AddGalleryImages();
}

public class ImageRepository(IDbConnection connection) : IImageRepository
{
    public async Task<List<ImageGetDto>> GetImages(int productId)
    {
        var images = await connection.QueryAsync<ImageGetDto>(ImageQueries.GetProductImages,
            new { ProductId = productId });

        return images.ToList();    
    }

    public async Task<int> AddProductImage(int productId, string imageUrl, int imageTypeId)
    {
        return await connection.ExecuteScalarAsync<int>(ImageQueries.AddProductImage, new
        {
            ProductId = productId,
            ImageUrl = imageUrl,
            ImageTypeId = imageTypeId
        });
    }

    public async Task<ProductImageDto?> GetProductImageById(int productId, int imageId)
    {
        return await connection.QueryFirstOrDefaultAsync<ProductImageDto>(ImageQueries.Temp, new
        {
            ProductId = productId,
            ImageId = imageId
        });
    }

    public async Task DeleteFeaturedImage(int productId) 
        => await connection.ExecuteAsync(ImageQueries.DeleteFeaturedImage, new { ProductId = productId });

    public async Task DeleteProductImage(int imageId) 
        => await connection.ExecuteAsync(ImageQueries.DeleteProductImage, new { ImageId = imageId });

    public async Task<bool> AddGalleryImages()
    {
        throw new NotImplementedException();
    }
}

//TODO: move and change
public class ProductImageDto
{
    public int ImageId { get; set; }
    public string ImageUrl { get; set; } // You can adapt this to match your schema
    public int ImageTypeId { get; set; } // 1 = Featured, 2 = Regular
}