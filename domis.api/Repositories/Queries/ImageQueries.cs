namespace domis.api.Repositories.Queries;

public static class ImageQueries
{
    public const string GetProductImages = @"
            SELECT
                i.Id as Id,
                i.blob_url AS Url,
                it.image_type_name AS Type
            FROM domis.product_image pi
            JOIN domis.image i ON pi.image_id = i.id
            JOIN domis.image_type it ON pi.image_type_id = it.id
            WHERE pi.product_id = @ProductId;";

    public const string Temp = @"
        SELECT 
            pi.id AS ImageId,
            pi.image_id AS ImageUrl,
            pi.image_type_id AS ImageTypeId
        FROM domis.product_image pi
        WHERE pi.product_id = @ProductId AND pi.id = @ImageId
    ";
    
    public const string AddProductImage = @"
        INSERT INTO domis.product_image (product_id, image_id, image_type_id)
        VALUES (@ProductId, @ImageUrl, @ImageTypeId)
        RETURNING id
    ";

    public const string DeleteFeaturedImage = @"
        DELETE FROM domis.product_image
        WHERE product_id = @ProductId AND image_type_id = 1
    ";
    
    public const string DeleteImage = @"
        DELETE FROM domis.image WHERE id = @ImageId
    ";
    
    public const string DeleteProductImage = @"
        DELETE FROM domis.product_image WHERE image_id = @ImageId
    ";
}
