namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    public class IntToDecimalValueConverter : ValueConverterBase<int, decimal>
    {
        #region Methods
        protected override object Convert(int value, Type targetType, object parameter)
        {
            return System.Convert.ToDecimal(value);
        }

        protected override object ConvertBack(decimal value, Type targetType, object parameter)
        {
            return System.Convert.ToInt32(value);
        }
        #endregion
    }

    public class DecimalToIntValueConverter : ValueConverterBase<decimal, int>
    {
        #region Methods
        protected override object Convert(decimal value, Type targetType, object parameter)
        {
            return System.Convert.ToInt32(value);
        }

        protected override object ConvertBack(int value, Type targetType, object parameter)
        {
            return System.Convert.ToDecimal(value);
        }
        #endregion
    }
}
