// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanPartExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    public static class TimeSpanPartExtensions
    {
        #region Methods
        public static TimeSpan CreateTimeSpan(this TimeSpanPart timeSpanPart, double value)
        {
            switch (timeSpanPart)
            {
                case TimeSpanPart.Days:
                    return TimeSpan.FromDays(value);

                case TimeSpanPart.Hours:
                    return TimeSpan.FromHours(value);

                case TimeSpanPart.Minutes:
                    return TimeSpan.FromMinutes(value);

                case TimeSpanPart.Seconds:
                    return TimeSpan.FromSeconds(value);

                default:
                    throw new InvalidOperationException();
            }
        }

        public static double GetTimeSpanPartValue(this TimeSpan value, TimeSpanPart timeSpanPart)
        {
            switch (timeSpanPart)
            {
                case TimeSpanPart.Days:
                    return value.TotalDays;

                case TimeSpanPart.Hours:
                    return value.TotalHours;

                case TimeSpanPart.Minutes:
                    return value.TotalMinutes;

                case TimeSpanPart.Seconds:
                    return value.TotalSeconds;

                default:
                    throw new InvalidOperationException();
            }
        }

        public static string GetTimeSpanPartName(this TimeSpanPart timeSpanPart)
        {
            switch (timeSpanPart)
            {
                case TimeSpanPart.Days:
                    return "days";

                case TimeSpanPart.Hours:
                    return "hours";

                case TimeSpanPart.Minutes:
                    return "minutes";

                case TimeSpanPart.Seconds:
                    return "seconds";

                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion
    }
}
