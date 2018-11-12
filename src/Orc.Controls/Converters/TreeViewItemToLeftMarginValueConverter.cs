// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewItemToLeftMarginValueConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
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
        #region Properties
        public double Length { get; set; }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            var item = value as TreeViewItem;
            return item == null ? new Thickness(0) : new Thickness(Length * item.GetDepth(), 0, 0, 0);
        }
        #endregion
    }
}
