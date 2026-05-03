namespace Orc.Controls.Converters;

using System;
using Catel.MVVM.Converters;

internal class Hour24ToHour12Converter : ValueConverterBase
{
    private int _prev;

    public bool IsEnabled { get; set; }
    private string? AmPm { get; set; }

    protected override object? Convert(object? value, Type targetType, object? parameter)
    {
        if (!IsEnabled || value is null)
        {
            return value;
        }

        if (!int.TryParse(value.ToString(), out var hour24))
        {
            return value;
        }

        var newValue = hour24 % 12 > 0 ? hour24 % 12 : 12;

        // Set current AM/PM designator.
        AmPm = hour24 >= 12 ? "PM" : "AM";

        return newValue;
    }

    protected override object? ConvertBack(object? value, Type targetType, object? parameter)
    {
        if (!IsEnabled || value is null || !string.Equals(AmPm, "AM") && !string.Equals(AmPm, "PM"))
        {
            return value;
        }

        if (!int.TryParse(value.ToString(), out var hour12))
        {
            return value;
        }

        AmPm = GetAmPm(hour12);

        // Remember hour set previously by user.
        _prev = hour12;

        var isPm = string.Equals(AmPm, "PM");
        return isPm switch
        {
            true when hour12 == 12 => 12,
            false when hour12 == 12 => 0,
            _ => hour12 % 12 + (isPm ? 12 : 0)
        };
    }

    private string? GetAmPm(int hour12)
    {
        return _prev switch
        {
            12 when hour12 == 11 && string.Equals(AmPm, "PM") => "AM",
            12 when hour12 == 11 && string.Equals(AmPm, "AM") => "PM",
            11 when hour12 == 12 && string.Equals(AmPm, "PM") => "AM",
            11 when hour12 == 12 && string.Equals(AmPm, "AM") => "PM",
            _ => AmPm
        };
    }
}
