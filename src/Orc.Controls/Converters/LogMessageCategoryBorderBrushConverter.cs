namespace Orc.Controls.Converters;

using System;
using System.Collections.Generic;
using System.Windows.Media;
using Catel.MVVM.Converters;

internal class LogMessageCategoryBorderBrushConverter : ValueConverterBase<string>
{
    public static readonly Dictionary<string, SolidColorBrush> BrushCache = new(StringComparer.OrdinalIgnoreCase);

    static LogMessageCategoryBorderBrushConverter()
    {
        BrushCache["Debug"] = new SolidColorBrush(Colors.DarkGray);
        BrushCache["Information"] = new SolidColorBrush(Colors.RoyalBlue);
        BrushCache["Warning"] = new SolidColorBrush(Colors.DarkOrange);
        BrushCache["Error"] = new SolidColorBrush(Colors.Red);
        BrushCache["Critical"] = new SolidColorBrush(Colors.Red);
        BrushCache["Clock"] = new SolidColorBrush(Colors.Gray);
    }

    protected override object? Convert(string? value, Type targetType, object? parameter)
    {
        return BrushCache.TryGetValue(value ?? string.Empty, out var brush) ? brush : Brushes.Black;
    }
}
