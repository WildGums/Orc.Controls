namespace Orc.Controls.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM.Converters;

    public class TreeViewItemToLeftMarginValueConverter : ValueConverterBase
    {
        public double Length { get; set; }

        protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            var item = value as TreeViewItem;
            return item is null ? new Thickness(0) : new Thickness(Length * item.GetDepth(), 0, 0, 0);
        }
    }
}
