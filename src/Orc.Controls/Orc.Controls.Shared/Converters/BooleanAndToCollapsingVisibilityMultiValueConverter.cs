// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanAndToCollapsingVisibilityMultiValueConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Orc.Controls.Converters
{
    public class BooleanAndToCollapsingVisibilityMultiValueConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var obj in values)
            {
                if (!(obj is bool))
                {
                    return Visibility.Collapsed;
                }
                else if(!(bool)obj)
                {
                    return Visibility.Collapsed;
                }
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Not supported (and IMultiValueConverter must return null if no conversion is supported)
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
