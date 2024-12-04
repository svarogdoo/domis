using domis.api.Models.Enums;

namespace domis.api.Common;

public static class StaticHelper
{
    private static readonly Dictionary<SortProductEnum, string> SortMappings = new()
    {
        { SortProductEnum.None, "Name ASC" }, //default
        { SortProductEnum.PriceAsc, "Price ASC" },
        { SortProductEnum.PriceDesc, "Price DESC" },
        { SortProductEnum.NameAsc, "Name ASC" },
        { SortProductEnum.NameDesc, "Name DESC" }
    };

    public static string GetOrderByClause(SortProductEnum sort)
    {
        return SortMappings.TryGetValue(sort, out var orderBy) ? orderBy : "Name ASC";
    }
}

public static class DateTimeHelper
{
    public static TimeZoneInfo BelgradeTimeZone { get; } =
        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    public static DateTime BelgradeNow { get; } =
        TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BelgradeTimeZone);
}