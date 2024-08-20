using CsvHelper.Configuration.Attributes;
using domis.api.Extensions;

namespace domis.api.Models;

public class NivelacijaRecord(int sku, decimal price, decimal stock)
{
    public int Sku { get; set; } = sku;
    [TypeConverter(typeof(CustomDecimalConverter))]
    public decimal Price { get; set; } = price;
    [TypeConverter(typeof(CustomDecimalConverter))]
    public decimal Stock { get; set; } = stock;
}