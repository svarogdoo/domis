using domis.api.Models.Enums;

namespace domis.api.Extensions;

public class DapperExtensions
{
    public static ProductQuantityType MapQuantityType(string quantityType)
    {
        return Enum.TryParse(quantityType, true, out ProductQuantityType result)
            ? result
            : ProductQuantityType.None; // Default value for unknown strings
    }
}
