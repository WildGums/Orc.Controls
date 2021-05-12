// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YearLongToYearShortConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    public class YearLongToYearShortConverter : ValueConverterBase
    {
        #region Fields
        private int _yearBase = 2000;
        #endregion

        #region Constructors
        public YearLongToYearShortConverter()
        {
        }
        #endregion

        #region Properties
        public bool IsEnabled { get; set; }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (!IsEnabled || value is null)
            {
                return value;
            }

            if (!int.TryParse(value.ToString(), out var yearLong))
            {
                return value;
            }

            _yearBase = yearLong - yearLong % 100;

            return yearLong - _yearBase;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            if (!IsEnabled || value is null)
            {
                return value;
            }

            if (int.TryParse(value.ToString(), out var yearShort))
            {
                return _yearBase + yearShort;
            }

            return value;
        }
        #endregion
    }
}
