﻿namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    public class ShortToDecimalValueConverter : ValueConverterBase<short, decimal>
    {
        #region Methods
        protected override object Convert(short value, Type targetType, object parameter)
        {
            return System.Convert.ToDecimal(value);
        }

        protected override object ConvertBack(decimal value, Type targetType, object parameter)
        {
            return System.Convert.ToInt16(value);
        }
        #endregion
    }
}
