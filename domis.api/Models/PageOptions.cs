using domis.api.Models.Enums;

namespace domis.api.Models;

public class PageOptions
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 18;
    public SortProductEnum Sort { get; init; } = SortProductEnum.None;
}