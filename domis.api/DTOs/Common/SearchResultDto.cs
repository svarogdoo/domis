namespace domis.api.DTOs.Common;

public class SearchResultDto
{
    public required int Id { get; set; }
    public int? Sku { get; set; }
    public string? Name { get; set; }
    public required string Type { get; set; }
}