﻿namespace domis.api.Models;

public class Category
{
    public int Id { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Category> Subcategories { get; set; } = [];
}