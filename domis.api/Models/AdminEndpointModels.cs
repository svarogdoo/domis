using domis.api.DTOs.Product;

namespace domis.api.Models;

public record RoleRequest(string Role);

public record RoleDiscountRequest(string RoleName, decimal Discount);

public record ProductSaleRequest(List<int> ProductIds, decimal? SalePrice, decimal? SalePercentage, DateTime StartDate, DateTime? EndDate);

public record CategorySaleRequest(int CategoryId, decimal? SalePercentage, DateTime StartDate, DateTime EndDate);

public record AssignProductToCategoryRequest(int ProductId, int CategoryId, bool OverwriteExisting);

public record CreateProductRequest(
    int CategoryId,
    string Name, string Description, int Sku, bool IsActive, int QuantityType,
    Attributes Attributes,
    ProductPriceUpdateDto? Price,
    Size? Size
);

public record CreateProductInitialRequest(int CategoryId, string Name, int Sku);

public record AddGalleryImagesRequest(List<string> DataUrls);