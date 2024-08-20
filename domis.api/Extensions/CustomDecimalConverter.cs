using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace domis.api.Extensions;

public class CustomDecimalConverter : ITypeConverter
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        // Replace comma with dot if the culture uses dot as decimal separator
        text = text.Replace(',', '.');

        // Attempt to parse the value, including scientific notation
        if (decimal.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
        {
            return result;
        }

        throw new TypeConverterException(this, memberMapData, text, row.Context);
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return value?.ToString() ?? string.Empty;
    }
}