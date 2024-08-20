namespace domis.api.Models;

public class Category
{
    public int CategoryId { get; set; }
    public int? ParentCategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryDescription { get; set; }
    public List<Category> Subcategories { get; set; } = new List<Category>();
}