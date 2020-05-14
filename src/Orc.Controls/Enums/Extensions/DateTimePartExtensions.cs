// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    public static class DateTimePartExtensions
    {
        #region Methods
        public static string GetDateTimePartName(this DateTimePart dateTimePart)
        {
            switch (dateTimePart)
            {
                case DateTimePart.Day:
                    return "PART_DaysNumericTextBox";

                case DateTimePart.Month:
                    return "PART_MonthNumericTextBox";

                case DateTimePart.Year:
                    return "PART_YearNumericTextBox";

                case DateTimePart.Hour:
                    return "NumericTBHour";

                case DateTimePart.Hour12:
                    return "NumericTBHour";

                case DateTimePart.Minute:
                    return "NumericTBMinute";

                case DateTimePart.Second:
                    return "NumericTBSecond";

                case DateTimePart.AmPmDesignator:
                    return "ListTBAmPm";

                default:
                    throw new InvalidOperationException();
            }
        }

        public static string GetDateTimePartToggleButtonName(this DateTimePart dateTimePart)
        {
            switch (dateTimePart)
            {
                case DateTimePart.Day:
                    return "PART_DaysToggleButton";

                case DateTimePart.Month:
                    return "PART_MonthToggleButton";

                case DateTimePart.Year:
                    return "PART_YearToggleButton";

                case DateTimePart.Hour:
                    return "ToggleButtonH";

                case DateTimePart.Hour12:
                    return "ToggleButtonH";

                case DateTimePart.Minute:
                    return "ToggleButtonM";

                case DateTimePart.Second:
                    return "ToggleButtonS";

                case DateTimePart.AmPmDesignator:
                    return "ToggleButtonT";

                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion
    }
}
