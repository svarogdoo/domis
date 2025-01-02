namespace domis.api.DTOs.Product;

public record AddProductDto(
    string Name, string Description, int Sku, Price Price,
    VpPrice? VpPrice, bool IsActive, Size Size, int CategoryId
);