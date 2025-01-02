namespace domis.api.Endpoints.Helpers;

public record ProductFilter(decimal? MinPrice, decimal? MaxPrice, decimal? MinWidth, decimal? MaxWidth, decimal? MinHeight, decimal? MaxHeight);
public record PageOptions(int? PageNumber, int? PageSize, int? Sort);