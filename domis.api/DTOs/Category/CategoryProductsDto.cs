using domis.api.DTOs.Product;

namespace domis.api.DTOs.Category;

public class CategoryProductsDto
{
    public CategoryBasicInfoDto? Category { get; set; }
    public List<ProductPreviewDto>? Products { get; set; }
}

public class CategoryBasicInfoDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}