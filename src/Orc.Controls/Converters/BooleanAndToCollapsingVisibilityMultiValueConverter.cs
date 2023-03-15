﻿namespace Orc.Controls.Converters;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

public class BooleanAndToCollapsingVisibilityMultiValueConverter : MarkupExtension, IMultiValueConverter
{
    public object? Convert(object?[]? values, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (values is null)
        {
            return Visibility.Collapsed;
        }

        foreach (var obj in values)
        {
            if (obj is not bool b)
            {
                return Visibility.Collapsed;
            }

            if (!b)
            {
                return Visibility.Collapsed;
            }
        }

        return Visibility.Visible;
    }

    public object?[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
    {
        // Not supported (and IMultiValueConverter must return null if no conversion is supported)
        return null;
    }

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
