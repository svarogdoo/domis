namespace domis.api.Repositories.Queries;

public static class ImageQueries
{
    public const string GetProductImages = @"
            SELECT
                i.blob_url AS Url,
                it.image_type_name AS Type
            FROM domis.product_image pi
            JOIN domis.image i ON pi.image_id = i.id
            JOIN domis.image_type it ON pi.image_type_id = it.id
            WHERE pi.product_id = @ProductId;";


}
