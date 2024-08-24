namespace domis.api.DTOs;

public record ProductPreviewDto(int Id, string? Name, int? Sku, decimal? Price, decimal? Stock, string? FeaturedImageUrl);