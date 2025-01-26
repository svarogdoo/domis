using domis.api.DTOs.Product;

namespace domis.api.DTOs.Category;

public class CategoryWithProductsDto
{
    public CategoryBasicInfoDto? Category { get; set; }
    public List<ProductPreviewDto>? Products { get; set; }
    public MaxFilterValues? MaxFilters { get; set; }
}

public class CategoryBasicInfoDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<CategoryPath>? Paths { get; set; }
}

public class CategoryPath
{
    public int? Id { get; set; }
    public string? Name { get; set; }
}

public class MaxFilterValues
{
    public decimal? MaxPrice { get; set; }
    public decimal? MaxWidth { get; set; }
    public decimal? MaxLength { get; set; }
    public decimal? MaxHeight { get; set; }
    public decimal? MaxDepth { get; set; }
    public decimal? MaxThickness { get; set; }
}