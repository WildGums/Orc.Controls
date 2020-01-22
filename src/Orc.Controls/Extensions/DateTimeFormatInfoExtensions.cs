// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatInfoExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.Logging;

    internal static class DateTimeFormatInfoExtensions
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        public static bool IsCorrect(this DateTimeFormatInfo formatInfo, bool isDateRequired, bool isTimeAllowed, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);

            errorMessage = string.Empty;
            var missingFields = new List<string>();

            if (isDateRequired && (formatInfo.DayFormat is null || formatInfo.MonthFormat is null || formatInfo.YearFormat is null))
            {
                missingFields.AddRange(new[]
                {
                    "day",
                    "month",
                    "year"
                });
            }

            if (!isTimeAllowed && !(formatInfo.HourFormat is null
                                   || formatInfo.MinuteFormat is null
                                   || formatInfo.SecondFormat is null))
            {
                missingFields.AddRange(new[]
                {
                    "hour",
                    "minute",
                    "second"
                });
            }

            if (missingFields.Any())
            {
                errorMessage = "Format string is incorrect. Missing required fields: " + string.Join(", ", missingFields);
                return false;
            }

            return true;
        }

        public static int FormatPart(this DateTimeFormatInfo result, string part, int current, bool isDateOnly)
        {
            if (part.Contains('d'))
            {
                result.SetDayFormat(part, current++);

                return current;
            }

            if (part.Contains('M'))
            {
                result.SetMonthFormat(part, current++);

                return current;
            }

            if (part.Contains('y'))
            {
                result.SetYearFormat(part, current++);

                return current;
            }

            if (isDateOnly && part.IsDateTimeFormatPart())
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Time fields are not expected");
            }

            if (part.Contains('h') || part.Contains('H'))
            {
                result.SetHourFormat(part, current++);

                return current;
            }

            if (part.Contains('m'))
            {
                result.SetMinuteFormat(part, current++);

                return current;
            }

            if (part.Contains('s'))
            {
                result.SetSecondFormat(part, current++);

                return current;
            }

            if (part.Contains('t'))
            {
                result.SetAmPmFormat(part, current++);

                return current;
            }

            result.SetSeparator(part, current);

            return current;
        }

        public static void SetDayFormat(this DateTimeFormatInfo formatInfo, string part, int position)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => part);

            if (formatInfo.DayFormat != null)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Day field can not be specified more than once");
            }

            if (part.Length > 4)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Day field must be in one of formats: 'd' or 'dd' or 'ddd' or 'dddd'");
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

            if (part.Length > 4)
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Month field must be in one of formats: 'M' or 'MM' or 'MMM' or 'MMMM'");
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

        public static bool CheckYearFormat(this DateTimeFormatInfo formatInfo, string partValue, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => partValue);

            errorMessage = string.Empty;
            switch (formatInfo.YearFormat.Length)
            {
                // 'y'
                case 1 when partValue.Length < 1 || partValue.Length > 2:
                    {
                        errorMessage = "Invalid year value. Year must contain 1 or 2 digits";
                        return false;
                    }
                // 'yy'
                case 2 when partValue.Length != 2:
                    {
                        errorMessage = "Invalid year value. Year must contain 2 digits";
                        return false;
                    }
                // 'yyy'
                case 3 when partValue.Length < 3 || partValue.Length > 5:
                    {
                        errorMessage = "Invalid year value. Year must contain 3 or 4 or 5 digits";
                        return false;
                    }
                // 'yyyy'
                case 4 when partValue.Length < 4 || partValue.Length > 5:
                    {
                        errorMessage = "Invalid year value. Year must contain 4 or 5 digits";
                        return false;
                    }
                // 'yyyyy'
                case 5 when partValue.Length != 5:
                    {
                        errorMessage = "Invalid year value. Year must contain 5 digits";
                        return false;
                    }
            }

            return true;
        }

        public static bool CheckMonthFormat(this DateTimeFormatInfo formatInfo, string partValue, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => partValue);

            errorMessage = string.Empty;
            switch (formatInfo.MonthFormat.Length)
            {
                // 'M'
                case 1 when partValue.Length < 1 || partValue.Length > 2:
                    {
                        errorMessage = "Invalid month value. Month must contain 1 or 2 digits";
                        return false;
                    }
                // 'MM'
                case 2 when partValue.Length != 2:
                    {
                        errorMessage = "Invalid month value. Month must contain 2 digits";
                        return false;
                    }
                // 'MMM'
                case 3 when partValue.Length != 3:
                    {
                        errorMessage = "Invalid month value. Month must contain 3 digits";
                        return false;
                    }
                // 'MMMM'
                case 4 when partValue.Length != 4:
                    {
                        errorMessage = "Invalid month value. Month must contain 4 digits";
                        return false;
                    }
            }

            return true;
        }

        public static bool CheckDayFormat(this DateTimeFormatInfo formatInfo, string partValue, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => partValue);

            errorMessage = string.Empty;
            switch (formatInfo.DayFormat.Length)
            {
                // 'd'
                case 1 when partValue.Length < 1 || partValue.Length > 2:
                    {
                        errorMessage = "Invalid day value. Day must contain 1 or 2 digits";
                        return false;
                    }

                // 'dd'
                case 2 when partValue.Length != 2:
                    {
                        errorMessage = "Invalid day value. Day must contain 2 digits";
                        return false;
                    }

                // 'ddd'
                case 3 when partValue.Length != 3:
                    {
                        errorMessage = "Invalid day value. Day must contain 3 digits";
                        return false;
                    }

                // 'dddd'
                case 4 when partValue.Length != 4:
                    {
                        errorMessage = "Invalid day value. Day must contain 4 digits";
                        return false;
                    }
            }

            return true;
        }

        public static bool CheckHourFormat(this DateTimeFormatInfo formatInfo, string partValue, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => partValue);

            errorMessage = string.Empty;
            switch (formatInfo.HourFormat.Length)
            {
                // 'h' or 'H'
                case 1 when partValue.Length < 1 || partValue.Length > 2:
                    {
                        errorMessage = "Invalid hour value. Hour must contain 1 or 2 digits";
                        return false;
                    }

                // 'hh' or 'HH'
                case 2 when partValue.Length != 2:
                    {
                        errorMessage = "Invalid hour value. Hour must contain 2 digits";
                        return false;
                    }
            }

            return true;
        }

        public static bool CheckMinuteFormat(this DateTimeFormatInfo formatInfo, string partValue, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => partValue);

            errorMessage = string.Empty;
            switch (formatInfo.MinuteFormat.Length)
            {
                // 'm'
                case 1 when partValue.Length < 1 || partValue.Length > 2:
                    {
                        errorMessage = "Invalid minute value. Minute must contain 1 or 2 digits";
                        return false;
                    }
                // 'mm'
                case 2 when partValue.Length != 2:
                    {
                        errorMessage = "Invalid minute value. Minute must contain 2 digits";
                        return false;
                    }
            }

            return true;
        }

        public static bool CheckSecondFormat(this DateTimeFormatInfo formatInfo, string partValue, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => partValue);

            errorMessage = string.Empty;
            switch (formatInfo.SecondFormat.Length)
            {
                case 1 when (partValue.Length < 1 || partValue.Length > 2):
                    {
                        errorMessage = "Invalid second value. Second must contain 1 or 2 digits";
                        return false;
                    }
                // 'ss'
                case 2 when partValue.Length != 2:
                    {
                        errorMessage = "Invalid second value. Second must contain 2 digits";
                        return false;
                    }
            }

            return true;
        }

        public static bool CheckIsAmPmShortFormat(this DateTimeFormatInfo formatInfo, string partValue, out string errorMessage)
        {
            Argument.IsNotNull(() => formatInfo);
            Argument.IsNotNull(() => partValue);

            errorMessage = string.Empty;
            switch (formatInfo.IsAmPmShortFormat)
            {
                case true when !(Meridiems.IsShortAm(partValue) || Meridiems.IsShortPm(partValue)):
                    {
                        errorMessage = "Invalid AM/PM designator value";
                        return false;
                    }

                case false when !(Meridiems.IsLongAm(partValue) || Meridiems.IsLongPm(partValue)):
                    {
                        errorMessage = "Invalid AM/PM designator value";
                        return false;
                    }
            }

            return true;
        }
        #endregion
    }
}
