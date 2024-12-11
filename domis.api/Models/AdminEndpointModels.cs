namespace domis.api.Models;

public record RoleRequest(string Role);

public record RoleDiscountRequest(string RoleName, decimal Discount);

public record ProductSaleRequest(List<int> ProductIds, decimal? SalePrice, decimal? SalePercentage, DateTime StartDate, DateTime? EndDate);

public record CategorySaleRequest(int CategoryId, decimal? SalePercentage, DateTime StartDate, DateTime EndDate);

public record AssignProductToCategoryRequest(int ProductId, int CategoryId);