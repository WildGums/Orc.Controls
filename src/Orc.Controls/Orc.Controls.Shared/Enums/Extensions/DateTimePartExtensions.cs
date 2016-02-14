// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    public static class DateTimePartExtensions
    {
        public static string GetDateTimePartName(this DateTimePart dateTimePart)
        {
            switch (dateTimePart)
            {
                case DateTimePart.Day:
                    return "NumericTBDay";

                case DateTimePart.Month:
                    return "NumericTBMonth";

                case DateTimePart.Year:
                    return "NumericTBYear";

                case DateTimePart.Hour:
                    return "NumericTBHour";

                case DateTimePart.Hour12:
                    return "NumericTBHour";

                case DateTimePart.Minute:
                    return "NumericTBMinute";

                case DateTimePart.Second:
                    return "NumericTBSecond";

                default:
                    throw new InvalidOperationException();
            }
        }

        public static string GetDateTimePartToggleButtonName(this DateTimePart dateTimePart)
        {
            switch (dateTimePart)
            {
                case DateTimePart.Day:
                    return "ToggleButtonD";

                case DateTimePart.Month:
                    return "ToggleButtonMo";

                case DateTimePart.Year:
                    return "ToggleButtonY";

                case DateTimePart.Hour:
                    return "ToggleButtonH";

                case DateTimePart.Hour12:
                    return "ToggleButtonH";

                case DateTimePart.Minute:
                    return "ToggleButtonM";

                case DateTimePart.Second:
                    return "ToggleButtonS";

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}