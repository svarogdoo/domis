using System.Runtime.Serialization;

namespace domis.api.Models.Enums;

public enum ProductQuantityType
{
    [EnumMember(Value = "None")]
    None = 1,
    [EnumMember(Value = "MeterSquared")]
    MeterSquared = 2,
    [EnumMember(Value = "Meter")]
    Meter = 3,
    [EnumMember(Value = "Piece")]
    Piece = 4

}