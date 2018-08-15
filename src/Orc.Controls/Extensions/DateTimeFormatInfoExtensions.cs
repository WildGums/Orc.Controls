// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatInfoExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Linq;
    using Catel;
    using Catel.Logging;

    internal static class DateTimeFormatInfoExtensions
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        public static bool IsCorrect(this DateTimeFormatInfo formatInfo, bool isDateOnly, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);

            errorMessage = string.Empty;

            if (isDateOnly && (formatInfo.DayFormat == null || formatInfo.MonthFormat == null || formatInfo.YearFormat == null))
            {
                errorMessage = "Format string is incorrect. Day, month and year fields are mandatory";
                return false;
            }

            if (!isDateOnly && (formatInfo.DayFormat == null
                                || formatInfo.MonthFormat == null
                                || formatInfo.YearFormat == null
                                || formatInfo.HourFormat == null
                                || formatInfo.MinuteFormat == null
                                || formatInfo.SecondFormat == null))
            {
                errorMessage = "Format string is incorrect. Day, month, year, hour, minute and second fields are mandatory";
                return false;
            }

            return true;
        }

        public static void SetDayFormat(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => part);

            if (formatInfo.DayFormat != null)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Day field can not be specified more than once");
            }

            if (part.Length > 2)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Day field must be in one of formats: 'd' or 'dd'");
            }

            formatInfo.DayFormat = part;
            formatInfo.DayPosition = position;
        }

        public static void SetMonthFormat(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => part);

            if (formatInfo.MonthFormat != null)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Month field can not be specified more than once");
            }

            if (part.Length > 2)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Month field must be in one of formats: 'M' or 'MM'");
            }

            formatInfo.MonthFormat = part;
            formatInfo.MonthPosition = position;
        }

        public static void SetYearFormat(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => part);

            if (formatInfo.YearFormat != null)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Year field can not be specified more than once");
            }

            if (part.Length > 5)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Year field must be in one of formats: 'y' or 'yy' or 'yyy' or 'yyyy' or 'yyyyy'");
            }

            formatInfo.YearFormat = part;
            formatInfo.YearPosition = position;

            formatInfo.IsYearShortFormat = part.Length < 3;
        }

        public static void SetMinuteFormat(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => part);

            if (formatInfo.MinuteFormat != null)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Minute field can not be specified more than once");
            }

            if (part.Length > 2)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Minute field must be in one of formats: 'm' or 'mm'");
            }

            formatInfo.MinuteFormat = part;
            formatInfo.MinutePosition = position;
        }

        public static void SetHourFormat(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => part);

            if (formatInfo.HourFormat != null)
            {
                var errorMessage = part == formatInfo.HourFormat
                    ? "Format string is incorrect. Hour field can not be specified more than once"
                    : "Format string is incorrect. Hour field must be 12 hour or 24 hour format, but no both";

                throw Log.ErrorAndCreateException<FormatException>(errorMessage);
            }

            if (part.Length > 2)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Hour field must be in one of formats: 'h' or 'H' or 'hh' or 'HH'");
            }

            formatInfo.HourFormat = part;
            formatInfo.HourPosition = position;

            formatInfo.IsHour12Format = part.Contains('h');
        }

        public static void SetSecondFormat(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => part);

            if (formatInfo.SecondFormat != null)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Second field can not be specified more than once");
            }

            if (part.Length > 2)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Second field must be in one of formats: 's' or 'ss'");
            }

            formatInfo.SecondFormat = part;
            formatInfo.SecondPosition = position;
        }

        public static void SetAmPmFormat(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => part);

            if (formatInfo.AmPmFormat != null)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. AM/PM designator field can not be specified more than once");
            }

            if (part.Length > 2)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. AM/PM designator field must be in one of formats: 't' or 'tt'");
            }

            formatInfo.AmPmFormat = part;
            formatInfo.AmPmPosition = position;

            formatInfo.IsAmPmShortFormat = part.Length < 2;
        }

        public static void SetSeparator(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);

            switch (position)
            {
                case 0:
                    formatInfo.Separator0 = part;
                    break;
                case 1:
                    formatInfo.Separator1 = part;
                    break;
                case 2:
                    formatInfo.Separator2 = part;
                    break;
                case 3:
                    formatInfo.Separator3 = part;
                    break;
                case 4:
                    formatInfo.Separator4 = part;
                    break;
                case 5:
                    formatInfo.Separator5 = part;
                    break;
                case 6:
                    formatInfo.Separator6 = part;
                    break;
                case 7:
                    formatInfo.Separator7 = part;
                    break;
            }
        }
        #endregion
    }
}
