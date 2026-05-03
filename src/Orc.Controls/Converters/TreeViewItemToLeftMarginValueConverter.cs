namespace Orc.Controls.Converters;

using System;
using System.Windows;
using System.Windows.Controls;
using Catel.MVVM.Converters;

public class TreeViewItemToLeftMarginValueConverter : ValueConverterBase
{
    private static readonly Thickness EmptyThickness = new(0);

    public double Length { get; set; }

    protected override object? Convert(object? value, Type targetType, object? parameter)
    {
        return value is TreeViewItem item 
            ? new Thickness(Length * item.GetDepth(), 0, 0, 0) 
            : EmptyThickness;
    }
}
