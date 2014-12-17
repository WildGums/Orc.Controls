// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanPartHelpers.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Helpers
{
    using System;

    public static class TimeSpanPartHelper
    {
        public static TimeSpan CreateTimeSpan(TimeSpanPart timeSpanPart, double value)
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

        public static double GetTimeSpanPartValue(TimeSpan value, TimeSpanPart timeSpanPart)
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

        public static string GetTimeSpanPartName(TimeSpanPart timeSpanPart)
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
    }
}