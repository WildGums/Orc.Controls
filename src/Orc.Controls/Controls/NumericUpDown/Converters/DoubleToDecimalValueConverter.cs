﻿namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    public class DoubleToDecimalValueConverter : ValueConverterBase<double, decimal>
    {
        #region Methods
        protected override object Convert(double value, Type targetType, object parameter)
        {
            return System.Convert.ToDecimal(value);
        }

        protected override object ConvertBack(decimal value, Type targetType, object parameter)
        {
            return System.Convert.ToDouble(value);
        }
        #endregion
    }
}
