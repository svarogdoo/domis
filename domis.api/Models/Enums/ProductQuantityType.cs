using System.Runtime.Serialization;

namespace domis.api.Models.Enums;

public enum ProductQuantityType
{
    [EnumMember(Value = "None")]
    None,
    [EnumMember(Value = "MeterSquared")]
    MeterSquared,
    [EnumMember(Value = "Meter")]
    Meter,
    [EnumMember(Value = "Piece")]
    Piece
}