// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanToTotalConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    internal class Hour24ToHour12Converter : ValueConverterBase
    {
        #region Fields
        private int _prev = 0;
        #endregion

        #region Constructors
        public Hour24ToHour12Converter()
        {
            
        }
        #endregion

        #region Properties
        public bool IsEnabled { get; set; }
        private string AmPm { get; set; }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (IsEnabled && value != null)
            {
                int hour24 = 0;
                if (int.TryParse(value.ToString(), out hour24))
                {
                    int newValue = 0;
                    if (hour24 % 12 > 0) newValue = hour24 % 12;
                    else newValue = 12;

                    // Set current AM/PM designator.
                    if (hour24 >= 12) AmPm = "PM";
                    else AmPm = "AM";

                    return newValue;
                }
            }
            return value;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            if (IsEnabled && value != null && (string.Equals(AmPm, "AM") || string.Equals(AmPm, "PM")))
            {
                int hour12 = 0;
                if (int.TryParse(value.ToString(), out hour12))
                {
                    // Here I try to smartly increment hour.
                    // For example: when hour is 12 PM and user press down key then hour will change to 11 AM, not 11 PM.
                    // This is done by changing AM/PM designator used to convert 12-hour to 24-hour.
                    if (_prev == 12 && hour12 == 11 && string.Equals(AmPm, "PM")) AmPm = "AM";
                    else if (_prev == 12 && hour12 == 11 && string.Equals(AmPm, "AM")) AmPm = "PM";
                    else if (_prev == 11 && hour12 == 12 && string.Equals(AmPm, "PM")) AmPm = "AM";
                    else if (_prev == 11 && hour12 == 12 && string.Equals(AmPm, "AM")) AmPm = "PM";

                    // Remember hour set previously by user.
                    _prev = hour12;

                    bool isPm = string.Equals(AmPm, "PM");
                    int newValue = 0;

                    if (isPm && hour12 == 12) newValue = 12;
                    else if (!isPm && hour12 == 12) newValue = 0;
                    else newValue = (hour12 % 12) + (isPm ? 12 : 0);

                    return newValue;
                }
            }
            return value;
        }
        #endregion
    }
}
