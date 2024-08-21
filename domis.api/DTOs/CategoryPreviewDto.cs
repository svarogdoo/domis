using domis.api.Models;

namespace domis.api.DTOs;

public class CategoryPreviewDto
{
    public int Id { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? Name { get; set; }
    public List<CategoryPreviewDto>? Subcategories { get; set; }
}
