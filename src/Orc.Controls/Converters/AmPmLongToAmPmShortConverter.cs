namespace Orc.Controls.Converters;

using System;
using Catel.MVVM.Converters;

internal class AmPmLongToAmPmShortConverter : ValueConverterBase
{
    public bool IsEnabled { get; set; }

    protected override object? Convert(object? value, Type targetType, object? parameter)
    {
        if (!IsEnabled || value is null)
        {
            return value;
        }

        return value switch
        {
            Meridiems.LongAM => Meridiems.ShortAM,
            Meridiems.LongPM => Meridiems.ShortPM,
            _ => value
        };
    }

    protected override object? ConvertBack(object? value, Type targetType, object? parameter)
    {
        if (!IsEnabled || value is null)
        {
            return value;
        }

        return value switch
        {
            Meridiems.ShortAM => Meridiems.LongAM,
            Meridiems.ShortPM => Meridiems.LongPM,
            _ => value
        };
    }
}
