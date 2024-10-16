namespace domis.api.Repositories.Queries;

public static class CartQueries
{
    public const string CheckIfCartExists = @"
        SELECT EXISTS 
        (SELECT 1 FROM domis.cart WHERE id = @CartId);"
;
    public const string GetAllCartStatuses = @"
               SELECT id AS Id, 
               status_name AS StatusName 
               FROM domis.cart_status;";
    
    public const string CreateCart = @"
                INSERT INTO domis.cart (user_id, status_id, created_at)
                VALUES (@UserId, @StatusId, @CreatedAt)
                RETURNING id;";
    
    public const string UpdateCartStatus = @"
                UPDATE domis.cart
                SET status_id = @StatusId
                WHERE id = @CartId;";
    
    public const string CreateCartItem = @"
                INSERT INTO domis.cart_item (cart_id, product_id, quantity, created_at, modified_at)
                VALUES (@CartId, @ProductId, @Quantity, @CreatedAt, @ModifiedAt)
                RETURNING id;";
    
    public const string UpdateCartItemQuantity = @"
                UPDATE domis.cart_item
                SET quantity = @Quantity, modified_at = @ModifiedAt
                WHERE id = @CartItemId;";
    
    public const string DeleteCartItem = @"
                DELETE FROM domis.cart_item
                WHERE id = @CartItemId;";
    
    
    public const string DeleteCartItemsQuery = @"
                    DELETE FROM domis.cart_item
                    WHERE cart_id = @CartId;";

    public const string DeleteCartQuery = @"
                    DELETE FROM domis.cart
                    WHERE id = @CartId;";
    
    public const string GetCart = @"
           SELECT 
                c.id AS CartId,
                c.user_id AS UserId,
                c.status_id AS StatusId,
                c.created_at AS CreatedAt,
                ci.id AS CartItemId,
                ci.product_id AS ProductId,
                ci.quantity AS Quantity,
                ci.created_at AS CartItemCreatedAt,
                ci.modified_at AS CartItemModifiedAt,
                p.product_name AS Name,
                p.sku AS Sku,
                p.price AS Price,  
                p.quantity_type_id AS QuantityType,
                i.blob_url AS Url,
				s.status_name as Status
            FROM 
                domis.cart c
			LEFT JOIN 
                domis.cart_status s ON s.id = c.status_id
            LEFT JOIN 
                domis.cart_item ci ON c.id = ci.cart_id
            LEFT JOIN
                domis.product p ON ci.product_id = p.id
            LEFT JOIN
	            domis.product_image pi ON p.id = pi.product_id
            LEFT JOIN 
                domis.image i ON pi.image_id = i.id
            WHERE 
                (pi.image_type_id is null or pi.image_type_id = 1) AND c.id = @CartId;";

    public const string GetCartByUser = @"
        SELECT 
            c.id AS CartId,
            c.user_id AS UserId,
            c.status_id AS StatusId,
            c.created_at AS CreatedAt,
            ci.id AS CartItemId,
            ci.product_id AS ProductId,
            ci.quantity AS Quantity,
            ci.created_at AS CartItemCreatedAt,
            ci.modified_at AS CartItemModifiedAt,
            p.product_name AS Name,
            p.sku AS Sku,
            p.price AS Price,  
            p.quantity_type_id AS QuantityType,
            i.blob_url AS Url,
            s.status_name AS Status
        FROM 
            domis.cart c
        LEFT JOIN 
            domis.cart_status s ON s.id = c.status_id
        LEFT JOIN 
            domis.cart_item ci ON c.id = ci.cart_id
        LEFT JOIN
            domis.product p ON ci.product_id = p.id
        LEFT JOIN
            domis.product_image pi ON p.id = pi.product_id
        LEFT JOIN 
            domis.image i ON pi.image_id = i.id
        WHERE 
            (pi.image_type_id IS NULL OR pi.image_type_id = 1) AND c.user_id = @UserId;";


    public const string CheckIfProductExistsInCart = @"SELECT COUNT(1) 
    FROM domis.cart_item 
    WHERE cart_id = @CartId AND product_id = @ProductId;";

    public const string UpdateQuantityBasedOnCartAndProduct = @"UPDATE domis.cart_item 
    SET quantity = @Quantity, modified_at = @ModifiedAt
    WHERE cart_id = @CartId AND product_id = @ProductId
    RETURNING Id;";

    public const string GetCartItemsWithProductPriceByCartId = @"
        SELECT 
            ci.id AS CartItemId, 
            ci.cart_id AS CartId, 
            ci.product_id AS ProductId,
            p.id AS ProductId,
            p.product_name AS ProductName,
            p.price AS ProductPrice
        FROM 
            domis.cart_item ci
        JOIN 
            domis.product p ON ci.product_id = p.id
        WHERE 
            ci.cart_id = @CartId;";
}