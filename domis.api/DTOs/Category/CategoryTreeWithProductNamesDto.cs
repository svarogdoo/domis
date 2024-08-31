namespace domis.api.DTOs.Category;

public class CategoryTreeWithProductNamesDto
{
    public required int Id { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? Name { get; set; }
    public List<CategoryTreeWithProductNamesDto>? Subcategories { get; set; }
    public List<ProductBasicInfoDto>? Products { get; set; }
}

public class ProductBasicInfoDto
{
    public required int Id { get; set; }
    public string? ProductName { get; set; }
    public int CategoryId { get; set; }
}