namespace domis.api.DTOs.Category;

public class CategoryMenuDto
{
    public int Id { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? Name { get; set; }
    public List<CategoryMenuDto>? Subcategories { get; set; }
}