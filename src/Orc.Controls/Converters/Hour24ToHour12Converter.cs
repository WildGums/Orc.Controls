// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hour24ToHour12Converter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    internal class Hour24ToHour12Converter : ValueConverterBase
    {
        #region Fields
        private int _prev;
        #endregion

        #region Properties
        public bool IsEnabled { get; set; }
        private string AmPm { get; set; }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (!IsEnabled || value is null)
            {
                return value;
            }

            if (!int.TryParse(value.ToString(), out var hour24))
            {
                return value;
            }

            var newValue = hour24 % 12 > 0 ? hour24 % 12 : 12;

            // Set current AM/PM designator.
            AmPm = hour24 >= 12 ? "PM" : "AM";

            return newValue;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            if (!IsEnabled || value is null || !string.Equals(AmPm, "AM") && !string.Equals(AmPm, "PM"))
            {
                return value;
            }

            if (!int.TryParse(value.ToString(), out var hour12))
            {
                return value;
            }

            AmPm = GetAmPm(hour12);

            // Remember hour set previously by user.
            _prev = hour12;

            var isPm = string.Equals(AmPm, "PM");
            if (isPm && hour12 == 12)
            {
                return 12;
            }

            if (!isPm && hour12 == 12)
            {
                return 0;
            }

            return hour12 % 12 + (isPm ? 12 : 0);
        }

        private string GetAmPm(int hour12)
        {
            switch (_prev)
            {
                case 12 when hour12 == 11 && string.Equals(AmPm, "PM"):
                    return "AM";

                case 12 when hour12 == 11 && string.Equals(AmPm, "AM"):
                    return "PM";

                case 11 when hour12 == 12 && string.Equals(AmPm, "PM"):
                    return "AM";

                case 11 when hour12 == 12 && string.Equals(AmPm, "AM"):
                    return "PM";
            }

            return AmPm;
        }
        #endregion
    }
}
