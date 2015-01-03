// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
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

                case DateTimePart.Minute:
                    return "NumericTBMinute";

                case DateTimePart.Second:
                    return "NumericTBSecond";

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}