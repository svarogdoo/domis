namespace domis.api.Models;

public record RoleRequest(Roles Role);

public record RoleDiscountRequest(string RoleName, decimal Discount);

public record ProductSaleRequest(int ProductId, decimal? SalePrice, decimal? SalePercentage, DateTime StartDate, DateTime EndDate);

public record AssignProductToCategoryRequest(int ProductId, int CategoryId);