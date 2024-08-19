using CsvHelper.Configuration.Attributes;
using domis.api.Extensions;

namespace domis.api.Models;

public class NivelacijaRecord
{
    public int Id { get; set; }
    [TypeConverter(typeof(CustomDecimalConverter))]
    public decimal Price { get; set; }
    [TypeConverter(typeof(CustomDecimalConverter))]
    public decimal Stock { get; set; }
}