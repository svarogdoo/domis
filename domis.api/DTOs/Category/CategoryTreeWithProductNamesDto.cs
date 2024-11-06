namespace domis.api.DTOs.Category;

public class CategoryTreeWithProductNamesDto
{
    public required int Id { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? Name { get; set; }
    public List<CategoryTreeWithProductNamesDto>? Subcategories { get; set; }
    public List<CategoryProductBasicInfoDto>? Products { get; set; }
}

public class CategoryProductBasicInfoDto
{
    public required int Id { get; set; }
    public string? ProductName { get; set; }
    public int CategoryId { get; set; }
}