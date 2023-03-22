namespace Orc.Controls;

using System;

public static class NumberExtensions
{
    public static T? ConvertTo<T>(this Number number)
    {
        return (T?)ConvertTo(number, typeof(T));
    }

    public static object? ConvertTo(this Number number, Type convertToType)
    {
        ArgumentNullException.ThrowIfNull(convertToType);

        var value = number.Value;
        if (value is null)
        {
            return default;
        }

        if (number.Type == convertToType)
        {
            return value;
        }

        var range = convertToType.TryGetNumberRange();
        return range is not null 
            ? convertToType.ChangeTypeSafe(number.DValue) 
            : default;
    }
}
