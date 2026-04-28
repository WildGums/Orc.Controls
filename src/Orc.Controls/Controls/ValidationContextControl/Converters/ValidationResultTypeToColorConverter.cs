namespace Orc.Controls;

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Catel.Data;

public class ValidationResultTypeToColorMultiValueConverter : IMultiValueConverter
{
    public SolidColorBrush? DefaultBrush { get; set; } = new(Colors.Black);
    public SolidColorBrush? ErrorBrush { get; set; }
    public SolidColorBrush? WarningBrush { get; set; }

    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2)
        {
            return DefaultBrush;
        }

        if (values[0] is not SolidColorBrush defaultColorBrush)
        {
            return DefaultBrush;
        }

        if (values[1] is not ValidationResultType validationResultType)
        {
            return defaultColorBrush;
        }

        var colorBrush = validationResultType == ValidationResultType.Error ? ErrorBrush : WarningBrush;
        return colorBrush;
    }

    public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return null;
    }
}
