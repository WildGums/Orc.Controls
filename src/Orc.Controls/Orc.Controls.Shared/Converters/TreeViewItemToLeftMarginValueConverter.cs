// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewItemToLeftMarginValueConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM.Converters;

    public class TreeViewItemToLeftMarginValueConverter : ValueConverterBase
    {
        public double Length { get; set; }

        protected override object Convert(object value, Type targetType, object parameter)
        {
            var item = value as TreeViewItem;
            if (item == null)
                return new Thickness(0);

            return new Thickness(Length * item.GetDepth(), 0, 0, 0);
        }
    }
}