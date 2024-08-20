using CsvHelper.Configuration.Attributes;
using domis.api.Extensions;

namespace domis.api.Models;

public class NivelacijaRecord(int id, decimal price, decimal stock)
{
    public int Id { get; set; } = id;
    [TypeConverter(typeof(CustomDecimalConverter))]
    public decimal Price { get; set; } = price;
    [TypeConverter(typeof(CustomDecimalConverter))]
    public decimal Stock { get; set; } = stock;
}