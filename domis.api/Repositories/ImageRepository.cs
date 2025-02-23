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
    Task DeleteImage(int imageId);
    Task<bool> AddGalleryImages(int productId, string productName, List<string> imageUrls, int imageTypeId);
    Task<int> UpdateFeaturedImage(int productId, string productName, string dataUrl);
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
        return await connection.QueryFirstOrDefaultAsync<ProductImageDto>(ImageQueries.GetImageDetailsByProductIdAndImageId, new
        {
            ProductId = productId,
            ImageId = imageId
        });
    }

    public async Task DeleteFeaturedImage(int productId) 
        => await connection.ExecuteAsync(ImageQueries.DeleteFeaturedImage, new { ProductId = productId });

    public async Task DeleteProductImage(int imageId)
    {
        await connection.ExecuteAsync(ImageQueries.DeleteProductImage, new { ImageId = imageId });
    }

    public async Task DeleteImage(int imageId)
    {
        await connection.ExecuteAsync(ImageQueries.DeleteImage, new { ImageId = imageId });
    }


    public async Task<bool> AddGalleryImages(int productId, string productName, List<string> imageUrls, int imageTypeId)
    {
        var imageIds = await InsertIntoImage(productName, imageUrls);
        await InsertIntoProductImage(productId, imageIds, imageTypeId);

        return true;
    }

    public async Task<int> UpdateFeaturedImage(int productId, string productName, string dataUrl)
    {
        var imageId = (await InsertIntoImage(productName, [dataUrl]))
            .FirstOrDefault();

        return await UpdateFeaturedProductImage(productId, imageId);
    }

    private async Task<List<int>> InsertIntoImage(string productName, List<string> imageUrls)
    {
        var query = """
                INSERT INTO domis.image (image_name, blob_url)
                VALUES (@ImageName, @BlobUrl)
                RETURNING id;
        """;

        var insertedIds = new List<int>();
        
        var findMaxIndexQuery = """
            SELECT COALESCE(MAX(
                CAST(SPLIT_PART(image_name, '_', 2) AS INTEGER)
            ), 0)
            FROM domis.image
            WHERE image_name LIKE @ProductNamePattern;
        """;
        
        var index = await connection.ExecuteScalarAsync<int>(findMaxIndexQuery, new { ProductNamePattern = productName + "_%" });
        
        foreach (var imageUrl in imageUrls)
        {
            var parameters = new
            {
                ImageName = $"{productName}_{++index}",
                BlobUrl = imageUrl
            };

            var insertedId = await connection.ExecuteScalarAsync<int>(query, parameters);
            insertedIds.Add(insertedId);
        }

        return insertedIds;
    }

    private async Task InsertIntoProductImage(int productId, List<int> imageIds, int imageTypeId)
    {
        var query = """
                INSERT INTO domis.product_image (product_id, image_id, image_type_id)
                VALUES (@ProductId, @ImageId, @ImageTypeId);
        """;
        
        foreach (var imageId in imageIds)
        {
            var parameters = new
            {
                ProductId = productId,
                ImageId = imageId,
                ImageTypeId = imageTypeId
            };

            await connection.ExecuteScalarAsync<int>(query, parameters);
        }
    }

    private async Task<int> UpdateFeaturedProductImage(int productId, int imageId)
    {
        const string getExistingFeaturedImg = @"
            SELECT image_id FROM domis.product_image 
            WHERE product_id = @ProductId AND image_type_id = 1;";
        
        const string updateFeaturedImg = @"
            UPDATE domis.product_image
            SET image_id = @ImageId
            WHERE product_id = @ProductId AND image_type_id = 1;";

        const string insertFeaturedImg = @"
            INSERT INTO domis.product_image (product_id, image_id, image_type_id)
            SELECT @ProductId, @ImageId, 1
            WHERE NOT EXISTS (
                SELECT 1 FROM domis.product_image WHERE product_id = @ProductId AND image_type_id = 1
            );";
        
        var existingFeaturedImgId = await connection.ExecuteScalarAsync<int?>(getExistingFeaturedImg, new { ProductId = productId });

        if (existingFeaturedImgId.HasValue)
        {
            await connection.ExecuteAsync(updateFeaturedImg, new { ProductId = productId, ImageId = imageId });
            await DeleteImage(existingFeaturedImgId.Value);
            return existingFeaturedImgId.Value;
        }

        await connection.ExecuteAsync(insertFeaturedImg, new { ProductId = productId, ImageId = imageId });
        return 0;
    }
}

//TODO: move and change
public class ProductImageDto
{
    public int ImageId { get; set; }
    public int ImageTypeId { get; set; } // 1 = Featured, 2 = Regular
    public string BlobUrl { get; set; }
}