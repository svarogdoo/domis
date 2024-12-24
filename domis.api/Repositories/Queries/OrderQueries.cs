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
            company_number,
            company_firstname,
            company_lastname,
            country_id,
            city,
            address,
            apartment,
            county,
            postal_code,
            phone_number,
            email,
            contact_phone,
            contact_person,
            address_type
        ) 
        VALUES (
            @FirstName,
            @LastName,
            @CompanyName,
            @CompanyNumber,
            @CompanyFirstName,
            @CompanyLastName,
            @CountryId,
            @City,
            @Address,
            @Apartment,
            @County,
            @PostalCode,
            @PhoneNumber,
            @Email,
            @ContactPhone,
            @ContactPerson,
            @AddressType
        )
        RETURNING id;";
    
    public const string UpdateOrderShipping = @"
        UPDATE domis.order_shipping
        SET
            first_name = COALESCE(@FirstName, first_name),
            last_name = COALESCE(@LastName, last_name),
            company_name = COALESCE(@CompanyName, company_name),
            country_id = COALESCE(@CountryId, country_id),
            city = COALESCE(@City, city),
            address = COALESCE(@Address, address),
            apartment = COALESCE(@Apartment, apartment),
            county = COALESCE(@County, county),
            postal_code = COALESCE(@PostalCode, postal_code),
            phone_number = COALESCE(@PhoneNumber, phone_number),
            email = COALESCE(@Email, email),
            company_number = COALESCE(@CompanyNumber, company_number),
            company_firstname = COALESCE(@CompanyFirstName, company_firstname),
            company_lastname = COALESCE(@CompanyLastName, company_lastname),
            contact_phone = COALESCE(@ContactPhone, contact_phone),
            contact_person = COALESCE(@ContactPerson, contact_person),
            address_type = COALESCE(@AddressType, address_type)
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
            os.company_number AS CompanyNumber,
            os.company_firstname AS CompanyFirstName,
            os.company_lastname AS CompanyLastName,
            os.contact_phone AS ContactPhone,
            os.contact_person AS ContactPerson,
            os.address_type AS AddressType,
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
        INSERT INTO domis.order 
        (user_id, status_id, payment_status_id, payment_vendor_type_id, payment_amount, comment, created_at, invoice_order_shipping_id, delivery_order_shipping_id)
        SELECT 
            c.user_id,
            1, -- assuming 1 is the status ID for a new order
            @PaymentStatusId,
            @PaymentVendorTypeId,
            @PaymentAmount,
            @Comment,
            @CreatedAt,
            @InvoiceOrderShippingId,
            @DeliveryOrderShippingId
        FROM domis.cart c
        WHERE c.id = @CartId
        RETURNING id;";


    public const string CreateOrderItems = @"
        INSERT INTO domis.order_item (order_id, product_id, quantity, order_item_amount, created_at)
        SELECT 
            @OrderId,
            ci.product_id,
            ci.quantity,
            ci.price,
            @CreatedAt
        FROM domis.cart_item ci
        WHERE ci.cart_id = @CartId
        RETURNING id;"; // Assuming order_item_id is the primary key


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
                oi.order_item_amount AS OrderItemAmount,
                oi.created_at AS OrderItemCreatedAt,
                oi.modified_at AS OrderItemModifiedAt,
                p.product_name AS ProductName,
                p.product_description AS ProductDescription,
                p.quantity_type_id AS QuantityType,
                p.sku AS Sku,
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

    public const string GetAllOrders = @"
        SELECT
            o.id AS OrderId,
            o.user_id AS UserId,
            o.payment_amount AS Amount,
            o.comment AS Comment,
            o.created_at AS OrderCreatedAt,
            o.status_id AS OrderStatusId,
            os.status_name AS OrderStatusName,
            o.invoice_order_shipping_id AS InvoiceOrderShippingId,
            o.delivery_order_shipping_id AS DeliveryOrderShippingId,
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
            oi.order_item_amount AS OrderItemAmount,
            oi.created_at AS OrderItemCreatedAt,
            oi.modified_at AS OrderItemModifiedAt,
            p.product_name AS ProductName,
            p.product_description AS ProductDescription,
            p.quantity_type_id AS QuantityType,
            p.sku AS Sku,
            i.blob_url AS Url
        FROM
            domis.order o
            LEFT JOIN domis.order_status os ON o.status_id = os.id
            LEFT JOIN domis.order_shipping osh ON o.delivery_order_shipping_id = osh.id
            LEFT JOIN domis.country c ON osh.country_id = c.id
            LEFT JOIN domis.payment_status ps ON o.payment_status_id = ps.id
            LEFT JOIN domis.payment_vendor_type pvt ON o.payment_vendor_type_id = pvt.id
            LEFT JOIN domis.order_item oi ON o.id = oi.order_id
            LEFT JOIN domis.product p ON oi.product_id = p.id
            LEFT JOIN domis.product_image pi ON p.id = pi.product_id
            LEFT JOIN domis.image i ON pi.image_id = i.id
        WHERE
            pi.image_type_id IS NULL OR pi.image_type_id = 1;"
    ; 

    public const string GetOrdersByUserId = @"
        SELECT 
            o.id AS Id,
            o.status_id AS StatusId,
            o.created_at AS Date,
            o.payment_vendor_type_id AS PaymentTypeId,
            o.payment_status_id AS PaymentStatusId,
            o.payment_amount AS PaymentAmount,
            TRIM(
                CONCAT_WS(', ', 
                    CASE WHEN os.address IS NOT NULL AND LENGTH(os.address) > 0 THEN os.address END,
                    CASE WHEN os.apartment IS NOT NULL AND LENGTH(os.apartment) > 0 THEN os.apartment END,
                    CASE WHEN os.postal_code IS NOT NULL AND LENGTH(os.postal_code) > 0 THEN os.postal_code END,
                    CASE WHEN os.city IS NOT NULL AND LENGTH(os.city) > 0 THEN os.city END
                )
            ) AS Address
        FROM domis.order o
        LEFT JOIN domis.order_shipping os ON o.invoice_order_shipping_id = os.id
        WHERE o.user_id = @UserId
        ORDER BY o.created_at DESC";

    public const string GetOrderItemsWithPrices = @"
        SELECT
            oi.id AS OrderItemId,
            oi.product_id AS ProductId,
            p.product_name AS ProductName,
            oi.order_item_amount as ProductPrice,
            oi.quantity AS Quantity         
        FROM
            domis.order_item oi
        JOIN 
            domis.product p ON p.id = oi.product_id
        WHERE
            oi.order_id = @OrderId;";

    public const string UpdateOrderItemPrice = @"
        UPDATE domis.order_item
        SET order_item_amount = @ProductPrice
        WHERE id = @OrderItemId;";

    public const string GetOrderItemsByOrderId = @"
        SELECT DISTINCT ON (oi.id) 
            oi.id AS Id,
            oi.quantity AS Quantity,
            oi.order_item_amount AS ItemPrice,
            p.product_name AS Name,
            p.quantity_type_id AS QuantityType,
            p.sku AS Sku,
            img.blob_url AS Image
        FROM domis.order_item oi
        LEFT JOIN domis.product p ON oi.product_id = p.id
        LEFT JOIN domis.product_image pi ON p.id = pi.product_id
        LEFT JOIN domis.image img ON pi.image_id = img.id AND pi.image_type_id = 1
        WHERE oi.order_id = @OrderId
        ORDER BY oi.id, oi.created_at DESC;";
}