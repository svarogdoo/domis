namespace domis.api.Repositories.Helpers;

public static class ProductQueries
{
    public const string GetSingleWithDetails = @"
            SELECT
                p.id AS Id,
                p.product_name AS Name,
                p.product_description AS Description,
                p.sku AS Sku,
                p.price AS Price,
                p.stock AS Stock,
                p.active AS IsActive,
                p.title AS Title,
                p.width AS Width,
                p.height AS Height,
                p.depth AS Depth,
                p.length AS Length,
                p.thickness AS Thickness,
                p.weight AS Weight,
                p.isItemType AS IsItemType,
                p.isSurfaceType AS IsSurfaceType,
                p.active AS IsActive,
                i.blob_url AS FeaturedImageUrl
            FROM domis.product p
            JOIN domis.product_image pi ON p.id = pi.product_id
            JOIN domis.image i ON pi.image_id = i.id
            JOIN domis.image_type it ON pi.image_type_id = it.id
            WHERE p.id = @ProductId AND it.image_type_name = 'Featured';"
    ;

    public const string GetAll = @"
                    SELECT
                        id AS Id,       
                        product_name AS Name,
                        sku AS Sku,
                        price AS Price,
                        stock AS Stock,
                        product_description AS Description
                    FROM domis.product"
    ;

    public const string GetAllByCategory = @"
            WITH RECURSIVE CategoryHierarchy AS (
                -- Anchor member: Start with the given category
                SELECT id
                FROM domis.category
                WHERE id = @CategoryId

                UNION ALL

                -- Recursive member: Join to get all subcategories
                SELECT c.id
                FROM domis.category c
                INNER JOIN CategoryHierarchy ch ON c.parent_category_id = ch.id
            ),
            ProductImages AS (
                -- Get Featured image for each product
                SELECT
                    pi.product_id AS ProductId,
                    i.blob_url AS FeaturedImageUrl
                FROM domis.product_image pi
                JOIN domis.image i ON pi.image_id = i.id
                JOIN domis.image_type it ON pi.image_type_id = it.id
                WHERE it.image_type_name = 'Featured'
            ),
            ProductsWithImages AS (
                -- Combine products with their Featured images
                SELECT
                    p.Id as Id,
                    p.product_name AS Name,
                    p.sku AS Sku,
                    p.price AS Price,
                    p.stock AS Stock,
                    p.active AS IsActive,
                    pi.FeaturedImageUrl
                FROM domis.product p
                INNER JOIN domis.product_category pc ON p.id = pc.product_id
                INNER JOIN CategoryHierarchy ch ON pc.category_id = ch.id
                LEFT JOIN ProductImages pi ON p.id = pi.ProductId
                WHERE p.active = true -- filter to include only active products
            )
            SELECT *
            FROM ProductsWithImages
            ORDER BY Name -- Ensure you have a column to order by
            OFFSET @Offset LIMIT @Limit;"
    ;

    public const string GetById = @"
            SELECT
                id AS Id,
                product_name AS Name,
                product_description AS Description,
                sku AS Sku,
                price AS Price,
                stock AS Stock,
                active AS IsActive
            FROM domis.product
            WHERE id = @ProductId;"
    ;

    public const string UpdateProductsByNivelacija = @"
            UPDATE domis.product
            SET price = CASE
                WHEN sku = @Sku THEN @Price
            END,
            stock = CASE
                WHEN sku = @Sku THEN @Stock
            END
            WHERE sku = @Sku"
    ;

    public const string CheckIfProductExists = @"
        SELECT EXISTS 
        (SELECT 1 FROM domis.product WHERE id = @ProductId);
    ";

    //TO-DO: decide what needs to be updated
    public const string UpdateProduct = @"
        UPDATE domis.product
        SET
            --product_name = COALESCE(@Name, product_name),
            product_description = COALESCE(@Description, product_description),
            --sku = COALESCE(@Sku, sku),
            --price = COALESCE(@Price, price),
            --stock = COALESCE(@Stock, stock),
            --active = COALESCE(@IsActive, active),
            title = COALESCE(@Title, title),
            width = COALESCE(@Width, width),
            height = COALESCE(@Height, height),
            depth = COALESCE(@Depth, depth),
            length = COALESCE(@Length, length),
            thickness = COALESCE(@Thickness, thickness),
            weight = COALESCE(@Weight, weight),
            isItemType = COALESCE(@IsItem, isItemType),
            isSurfaceType = COALESCE(@IsSurfaceType, isSurfaceType)
        WHERE id = @Id;"
    ;
}
