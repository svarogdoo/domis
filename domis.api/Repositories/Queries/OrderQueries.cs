namespace domis.api.Repositories.Queries;

public static class OrderQueries
{
    public const string GetPaymentStatuses = @"
            SELECT 
                id AS Id, 
                status_name AS StatusName 
            FROM 
                domis.payment_status;";
    
    public const string GetOrderStatuses = @"
            SELECT 
                id AS Id, 
                status_name AS StatusName 
            FROM 
                domis.order_status;";
    
    public const string GetPaymentVendors = @"
            SELECT 
                id AS Id, 
                payment_vendor_type_name AS PaymentVendorTypeName 
            FROM 
                domis.payment_vendor_type;";
    
    public const string CreateOrderShipping = @"
            INSERT INTO domis.order_shipping (
                first_name,
                last_name,
                company_name,
                country_id,
                city,
                address,
                apartment,
                county,
                postal_code,
                phone_number,
                email
            ) 
            VALUES (
                @FirstName,
                @LastName,
                @CompanyName,
                @CountryId,
                @City,
                @Address,
                @Apartment,
                @County,
                @PostalCode,
                @PhoneNumber,
                @Email
            )
            RETURNING id;";
    
    public const string UpdateOrderShipping = @"
            UPDATE domis.order_shipping
            SET
                first_name = @FirstName,
                last_name = @LastName,
                company_name = @CompanyName,
                country_id = @CountryId,
                city = @City,
                address = @Address,
                apartment = @Apartment,
                county = @County,
                postal_code = @PostalCode,
                phone_number = @PhoneNumber,
                email = @Email
            WHERE 
                id = @Id;";
    
    public const string GetOrderShipping = @"
            SELECT 
                os.id AS Id,
                os.first_name AS FirstName,
                os.last_name AS LastName,
                os.company_name AS CompanyName,
                os.country_id AS CountryId,
                os.city AS City,
                os.address AS Address,
                os.apartment AS Apartment,
                os.county AS County,
                os.postal_code AS PostalCode,
                os.phone_number AS PhoneNumber,
                os.email AS Email,
                c.country_name AS CountryName
            FROM 
                domis.order_shipping os
            INNER JOIN 
                domis.country c ON os.country_id = c.id
            WHERE 
                os.id = @Id;";
    
    public const string DeleteOrderShipping= @"
            DELETE FROM domis.order_shipping
            WHERE id = @Id;";
    
    public const string CreateOrder = @"
                INSERT INTO domis.order (user_id, status_id, order_shipping_id, payment_status_id, payment_vendor_type_id, payment_amount, comment, created_at)
                SELECT 
                    c.user_id,
                    1, -- assuming 1 is the status ID for a new order
                    @OrderShippingId,
                    @PaymentStatusId,
                    @PaymentVendorTypeId,
                    @PaymentAmount,
                    @Comment,
                    @CreatedAt
                FROM domis.cart c
                WHERE c.id = @CartId
                RETURNING id;";
    
    public const string CreateOrderItems = @"
                INSERT INTO domis.order_item (order_id, product_id, quantity, created_at)
                SELECT 
                    @OrderId,
                    ci.product_id,
                    ci.quantity,
                    @CreatedAt
                FROM domis.cart_item ci
                WHERE ci.cart_id = @CartId;";
    
    public const string UpdateOrderStatus = @"
            UPDATE domis.order
            SET status_id = @StatusId
            WHERE id = @OrderId;";
    
      public const string  GetOrderDetails = @"
            SELECT
                o.id AS OrderId,
                o.user_id AS UserId,
                o.payment_amount AS Amount,
                o.comment AS Comment,
                o.created_at AS OrderCreatedAt,
                o.status_id AS OrderStatusId,
                os.status_name AS OrderStatusName,
                o.order_shipping_id AS OrderShippingId,
                osh.first_name AS ShippingFirstName,
                osh.last_name AS ShippingLastName,
                osh.company_name AS ShippingCompanyName,
                osh.country_id AS ShippingCountryId,
                c.country_name AS ShippingCountryName,
                osh.city AS ShippingCity,
                osh.address AS ShippingAddress,
                osh.apartment AS ShippingApartment,
                osh.county AS ShippingCounty,
                osh.postal_code AS ShippingPostalCode,
                osh.phone_number AS ShippingPhoneNumber,
                osh.email AS ShippingEmail,
                o.payment_status_id AS PaymentStatusId,
                ps.status_name AS PaymentStatusName,
                o.payment_vendor_type_id AS PaymentVendorTypeId,
                pvt.payment_vendor_type_name AS PaymentVendorTypeName,
                oi.id AS OrderItemId,
                oi.product_id AS ProductId,
                oi.quantity AS OrderItemQuantity,
                oi.created_at AS OrderItemCreatedAt,
                oi.modified_at AS OrderItemModifiedAt,
                p.product_name AS ProductName,
                p.product_description AS ProductDescription,
               i.blob_url AS Url
            FROM
                domis.order o
                LEFT JOIN domis.order_status os ON o.status_id = os.id
                LEFT JOIN domis.order_shipping osh ON o.order_shipping_id = osh.id
                LEFT JOIN domis.country c ON osh.country_id = c.id
                LEFT JOIN domis.payment_status ps ON o.payment_status_id = ps.id
                LEFT JOIN domis.payment_vendor_type pvt ON o.payment_vendor_type_id = pvt.id
                LEFT JOIN domis.order_item oi ON o.id = oi.order_id
                LEFT JOIN domis.product p ON oi.product_id = p.id
                LEFT JOIN domis.product_image pi ON p.id = pi.product_id
                LEFT JOIN domis.image i ON pi.image_id = i.id
            WHERE
                o.id = @OrderId AND (pi.image_type_id is null or pi.image_type_id = 1);";

}