// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.ObjectModel;

    public static class DateTimePartExtensions
    {
        public static ObservableCollection<string> GetPopupSource(this DateTimePart dateTimePart)
        {
            switch (dateTimePart)
            {
                case DateTimePart.Day:
                    return new ObservableCollection<string> { "1", "2" };

                case DateTimePart.Month:
                    return new ObservableCollection<string> { "3", "4" };

                case DateTimePart.Year:
                    return new ObservableCollection<string> { "5", "6" };

                case DateTimePart.Hour:
                    return new ObservableCollection<string> { "7", "8" };

                case DateTimePart.Minute:
                    return new ObservableCollection<string> { "9", "10" };

                case DateTimePart.Second:
                    return new ObservableCollection<string> { "11", "12" };

                default:
                    throw new InvalidOperationException();
            }
        }

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