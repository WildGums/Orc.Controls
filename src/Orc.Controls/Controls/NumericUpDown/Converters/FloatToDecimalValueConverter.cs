// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FloatToDecimalValueConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    public class FloatToDecimalValueConverter : ValueConverterBase<float, decimal>
    {
        #region Methods
        protected override object Convert(float value, Type targetType, object parameter)
        {
            return System.Convert.ToDecimal(value);
        }

        protected override object ConvertBack(decimal value, Type targetType, object parameter)
        {
            return System.Convert.ToSingle(value);
        }
        #endregion
    }
}
