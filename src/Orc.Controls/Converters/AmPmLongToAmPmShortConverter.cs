// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmPmLongToAmPmShortConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    internal class AmPmLongToAmPmShortConverter : ValueConverterBase
    {
        #region Constructors
        public AmPmLongToAmPmShortConverter()
        {
        }
        #endregion

        #region Properties
        public bool IsEnabled { get; set; }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (!IsEnabled || value == null)
            {
                return value;
            }

            switch (value)
            {
                case Meridiems.LongAM:
                    return Meridiems.ShortAM;

                case Meridiems.LongPM:
                    return Meridiems.ShortPM;

                default:
                    return value;
            }
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            if (!IsEnabled || value == null)
            {
                return value;
            }

            switch (value)
            {
                case Meridiems.ShortAM:
                    return Meridiems.LongAM;

                case Meridiems.ShortPM:
                    return Meridiems.LongPM;

                default:
                    return value;
            }
        }
        #endregion
    }
}
