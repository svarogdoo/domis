namespace domis.api.Models;

public record RoleRequest(Roles Role);

public record DiscountRequest(string RoleName, decimal Discount);