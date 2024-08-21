namespace domis.api.DTOs;

public class ProductImageDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? Url { get; set; }
    public string? Type { get; set; }
}