namespace Orc.Controls.Converters;

using System;
using System.ComponentModel;
using System.Globalization;

public class NumberTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string)
               || sourceType.TryGetNumberRange() is not null;
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        return value switch
        {
            string str => !string.IsNullOrWhiteSpace(str) ? new Number(Convert.ToDouble(str)) : string.Empty,
            null => null,
            _ => new Number(value)
        };
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string)
               || destinationType?.TryGetNumberRange() is not null;
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is not Number number)
        {
            return null;
        }

        return destinationType == typeof(string)
            ? number.DValue.ToString(culture)
            : number.ConvertTo(destinationType);
    }
}
