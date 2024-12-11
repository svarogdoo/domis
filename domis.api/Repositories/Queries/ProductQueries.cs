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
                p.quantity_type_id AS QuantityType,
                i.blob_url AS FeaturedImageUrl
            FROM domis.product p
            JOIN domis.product_image pi ON p.id = pi.product_id
            JOIN domis.image i ON pi.image_id = i.id
            JOIN domis.image_type it ON pi.image_type_id = it.id
            WHERE p.id = @ProductId AND it.image_type_name = 'Featured';"
    ;

    public const string GetProductPricesForVPMultiple = @"
        SELECT
            pp.product_id AS ProductId,
            MAX(CASE WHEN pp.packaging_type = 'pal' THEN pp.price END) AS PalPrice,
            MAX(CASE WHEN pp.packaging_type = 'pak' THEN pp.price END) AS PakPrice
        FROM domis.product_pricing pp
        WHERE pp.product_id = ANY(@ProductIds)  -- Use ANY for array comparison
          AND pp.user_type = @Role
          AND pp.packaging_type IN ('pak', 'pal')
        GROUP BY pp.product_id;
    ";

    public const string GetSingleProductPricesForVP = @"
        SELECT
            pp.product_id AS ProductId,
            MAX(CASE WHEN pp.packaging_type = 'pal' THEN pp.price END) AS PalPrice,
            MAX(CASE WHEN pp.packaging_type = 'pak' THEN pp.price END) AS PakPrice
        FROM domis.product_pricing pp
        WHERE pp.product_id = @ProductId
          AND pp.user_type = @Role
          AND pp.packaging_type IN ('pak', 'pal')
        GROUP BY pp.product_id;
    ";
    
    public const string GetAll = @"
                    SELECT
                        p.id AS Id,       
                        p.product_name AS Name,
                        p.sku AS Sku,
                        p.price AS Price,
                        p.stock AS Stock,
                        p.product_description AS Description,
                        s.sale_price AS SalePrice,
                        s.start_date AS SaleStartDate,
                        s.end_date AS SaleEndDate,
                        CASE 
                            WHEN s.id IS NOT NULL AND s.is_active = TRUE AND s.start_date <= CURRENT_TIMESTAMP AND s.end_date >= CURRENT_TIMESTAMP 
                            THEN TRUE 
                            ELSE FALSE 
                        END AS IsOnSale
                    FROM domis.product p
                    LEFT JOIN
                        domis.sales s ON p.id = s.product_id"
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
        ActiveProductsInCategory AS (
            -- Select products in the specified category and its subcategories
            SELECT
                p.Id AS Id,
                p.sku AS Sku,
                p.product_name AS Name
            FROM domis.product p
            INNER JOIN domis.product_category pc ON p.id = pc.product_id
            INNER JOIN CategoryHierarchy ch ON pc.category_id = ch.id
            WHERE p.active = true -- filter to include only active products
        )
        SELECT Id, Sku, Name
        FROM ActiveProductsInCategory
        ORDER BY Name; -- Ensure you have a column to order by
    ";
    
    public const string GetProductsWithPricesByCategory = @"
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
        ActiveProductsInCategory AS (
            -- Select products in the specified category and its subcategories
            SELECT
                p.Id AS Id,
                p.price AS Price
            FROM domis.product p
            INNER JOIN domis.product_category pc ON p.id = pc.product_id
            INNER JOIN CategoryHierarchy ch ON pc.category_id = ch.id
            WHERE p.active = true -- filter to include only active products
        )
        SELECT Id, Price
        FROM ActiveProductsInCategory
    ";


    public const string GetAllFromCategory = @"
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
        LatestSale AS (
            -- Select only the most recent active sale per product
            SELECT
                s.product_id,
                s.sale_price,
                s.start_date,
                s.end_date,
                s.is_active
            FROM domis.sales s
            WHERE s.is_active = TRUE
              AND s.start_date = (
                  SELECT MAX(s2.start_date)
                  FROM domis.sales s2
                  WHERE s2.product_id = s.product_id AND s2.is_active = TRUE
              )
        ),
        ProductsWithImages AS (
            -- Combine products with their Featured images and latest sale
            SELECT DISTINCT ON (p.Id)
                p.Id AS Id,
                p.product_name AS Name,
                p.sku AS Sku,
                p.price AS Price,
                p.stock AS Stock,
                p.active AS IsActive,
                pi.FeaturedImageUrl,
                pqt.id AS QuantityType,
                -- Sale Info from LatestSale CTE
                ls.sale_price AS SalePrice,
                ls.start_date AS StartDate,
                ls.end_date AS EndDate,
                ls.is_active AS IsActive
            FROM domis.product p
            INNER JOIN domis.product_category pc ON p.id = pc.product_id
            INNER JOIN CategoryHierarchy ch ON pc.category_id = ch.id
            LEFT JOIN ProductImages pi ON p.id = pi.ProductId
            LEFT JOIN domis.product_quantity_type pqt ON p.quantity_type_id = pqt.id
            LEFT JOIN LatestSale ls ON p.id = ls.product_id -- Join to LatestSale instead of domis.sales
            WHERE p.active = TRUE -- filter to include only active products
        )
        SELECT *
        FROM ProductsWithImages";

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
        (SELECT 1 FROM domis.product WHERE id = @ProductId);"
    ;

    public const string UpdateProduct = @"
        UPDATE domis.product
        SET
            --product_name = COALESCE(@Name, product_name),
            product_description = COALESCE(@Description, product_description),
            --sku = COALESCE(@Sku, sku),
            --price = COALESCE(@Price, price),
            --stock = COALESCE(@Stock, stock),
            active = COALESCE(@IsActive, active),
            title = COALESCE(@Title, title),
            width = COALESCE(@Width, width),
            height = COALESCE(@Height, height),
            depth = COALESCE(@Depth, depth),
            length = COALESCE(@Length, length),
            thickness = COALESCE(@Thickness, thickness),
            weight = COALESCE(@Weight, weight),
            quantity_type_id = COALESCE(@QuantityType, quantity_type_id)
        WHERE id = @Id;"
    ;

    public const string GetAllQuantityTypes = @"
        SELECT
            id AS Id,
            name AS Name
        FROM domis.product_quantity_type;"
    ;

    public const string GetProductSizing = @"
        SELECT 
            pak,
            pal
        FROM 
            domis.product_packaging
        WHERE 
            product_id = @ProductId;"
    ;

    public const string GetProductPrice = @"
        SELECT price
        FROM domis.product
        WHERE id = @ProductId;
    ";
    
    public const string GetProductEffectivePrice = @"
        SELECT 
            COALESCE(
                (SELECT sale_price
                 FROM domis.sales
                 WHERE product_id = @ProductId 
                   AND is_active = TRUE
                   AND start_date <= CURRENT_DATE 
                   AND end_date >= CURRENT_DATE
                 LIMIT 1), 
                (SELECT price 
                 FROM domis.product
                 WHERE id = @ProductId)
            ) AS EffectivePrice;
    ";
    
    public const string InsertSale = @"
        INSERT INTO domis.sales (product_id, sale_price, start_date, end_date, is_active) 
        VALUES (@ProductId, @SalePrice, @StartDate, @EndDate, @IsActive)
    ";
    
    public const string GetActiveSale = @"
        SELECT 
            id AS Id,
            product_id AS ProductId,
            sale_price AS SalePrice,
            start_date AS StartDate,
            end_date AS EndDate,
            is_active AS IsActive
        FROM domis.sales
        WHERE product_id = @ProductId 
          AND is_active = TRUE
          AND start_date <= @CurrentDate 
          AND end_date >= @CurrentDate
        LIMIT 1";
    
    public static string GetProductCategoriesPaths = @"
        WITH RECURSIVE RecursiveCategoryHierarchy AS (
            -- Anchor member: Start with categories for the product
            SELECT
                pc.product_id AS ProductId,
                c.id AS CategoryId,
                c.parent_category_id AS ParentCategoryId,
                c.category_name AS CategoryName,
                ROW_NUMBER() OVER (ORDER BY c.id) AS PathId, -- Unique ID per distinct path
                1 AS Level -- Starting level
            FROM domis.product_category pc
            JOIN domis.category c ON pc.category_id = c.id
            WHERE pc.product_id = @ProductId AND active = true

            UNION ALL

            -- Recursive member: Join to find parent categories
            SELECT
                rch.ProductId,
                c.id AS CategoryId,
                c.parent_category_id AS ParentCategoryId,
                c.category_name AS CategoryName,
                rch.PathId, -- Propagate the PathId to keep categories within the same path
                rch.Level + 1 AS Level -- Increment level for each parent
            FROM domis.category c
            INNER JOIN RecursiveCategoryHierarchy rch
                ON c.id = rch.ParentCategoryId
        )

        -- Select each category in the hierarchy with its PathId, ordered by level
        SELECT
            PathId,
            CategoryId AS Id,
            CategoryName AS Name,
            Level
        FROM RecursiveCategoryHierarchy
        ORDER BY PathId, Level DESC;";
    
    public const string GetProductsOnSale = @"
        SELECT 
            p.id AS Id,
            p.product_name AS Name,
            p.sku AS Sku,
            p.price AS Price,
            p.stock AS Stock,
            NULL AS VpPrice, 
            NULL AS FeaturedImageUrl, 
            p.product_description AS Description,
            NULL AS QuantityType,
            s.is_active AS IsActive,
            s.sale_price AS SalePrice,
            s.start_date AS StartDate,
            s.end_date AS EndDate
        FROM domis.product p
        LEFT JOIN domis.sales s ON p.id = s.product_id
        WHERE p.active = TRUE
          AND s.is_active = TRUE
          AND (s.start_date IS NULL OR s.start_date <= @CurrentTime)
          AND (s.end_date IS NULL OR s.end_date >= @CurrentTime)
    ";

    public const string UpdateProductSizing = @"
        UPDATE domis.product_packaging
        SET
            pak = COALESCE(@Pak, pak),
            pal = COALESCE(@Pal, pal)
        WHERE product_id = @ProductId;
    ";
    
    public const string AssignProductToCategory = @"
        INSERT INTO domis.product_category (product_id, category_id)
        VALUES (@ProductId, @CategoryId)
        ON CONFLICT (product_id) 
        DO UPDATE SET category_id = @CategoryId;
    ";
    
    public const string SearchByName = @"
        SELECT id AS Id, product_name AS Name, sku AS Sku, 'Product' AS Type
        FROM domis.product
        WHERE product_name ILIKE @SearchTerm OR CAST(sku AS TEXT) ILIKE @SearchTerm
        UNION
        SELECT id AS Id, category_name AS Name, NULL AS Sku, 'Category' AS Type
        FROM domis.category
        WHERE category_name ILIKE @SearchTerm
        ORDER BY Type ASC
        LIMIT @PageSize OFFSET @Offset
    ";

    public const string DeactivateSale = @"
        UPDATE domis.sales
        SET is_active = FALSE
        WHERE product_id = @ProductId AND is_active = TRUE;
    ";
}
