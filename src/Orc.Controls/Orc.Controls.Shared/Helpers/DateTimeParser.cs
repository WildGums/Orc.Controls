// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeParser.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class DateTimeParser
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        public static DateTime Parse(string input, string format, bool isDateOnly = false)
        {
            return Parse(input, DateTimeFormatHelper.GetDateTimeFormatInfo(format, isDateOnly));
        }

        internal static DateTime Parse(string input, DateTimeFormatInfo formatInfo)
        {
            Argument.IsNotNull(() => input);

            return (DateTime)Parse(input, formatInfo, true);
        }

        public static bool TryParse(string input, string format, out DateTime dateTime, bool isDateOnly = false)
        {
            return TryParse(input, DateTimeFormatHelper.GetDateTimeFormatInfo(format, isDateOnly), out dateTime);
        }

        internal static bool TryParse(string input, DateTimeFormatInfo formatInfo, out DateTime dateTime)
        {
            Argument.IsNotNull(() => input);

            dateTime = DateTime.MinValue;

            var result = Parse(input, formatInfo, false);
            if (result == null)
            {
                return false;
            }
            else
            {
                dateTime = (DateTime)result;
                return true;
            }
        }

        private static DateTime? Parse(string input, DateTimeFormatInfo formatInfo, bool throwOnError = true)
        {
            Argument.IsNotNull(() => formatInfo);

            var year = 0;
            var month = 0;
            var day = 0;
            var hour = 0;
            var minute = 0;
            var second = 0;
            var amPm = default(string);
            var separator = default(string);

            var i = 0;
            var partValue = default(string);
            for (i = 0; i < 7; i++)
            {
                separator = formatInfo.GetSeparator(i);
                if (separator != null && separator.Length > 0)
                {
                    if (!input.StartsWith(separator))
                    {
                        return ThrowOnError<FormatException>("Invalid value. Value does not match to format");
                    }
                    input = input.Substring(separator.Length);
                }

                if (i == formatInfo.YearPosition)
                {
                    partValue = new string(input.TakeWhile(x => char.IsDigit(x)).ToArray());

                    if (formatInfo.YearFormat.Length == 1 && (partValue.Length < 1 || partValue.Length > 2)) // 'y'
                    {
                        return ThrowOnError<FormatException>("Invalid year value. Year must contain 1 or 2 digits");
                    }
                    else if (formatInfo.YearFormat.Length == 2 && partValue.Length != 2) // 'yy'
                    {
                        return ThrowOnError<FormatException>("Invalid year value. Year must contain 2 digits");
                    }
                    else if (formatInfo.YearFormat.Length == 3 && (partValue.Length < 3 || partValue.Length > 5)) // 'yyy'
                    {
                        return ThrowOnError<FormatException>("Invalid year value. Year must contain 3 or 4 or 5 digits");
                    }
                    else if (formatInfo.YearFormat.Length == 4 && (partValue.Length < 4 || partValue.Length > 5)) // 'yyyy'
                    {
                        return ThrowOnError<FormatException>("Invalid year value. Year must contain 4 or 5 digits");
                    }
                    else if (formatInfo.YearFormat.Length == 5 && partValue.Length != 5) // 'yyyyy'
                    {
                        return ThrowOnError<FormatException>("Invalid year value. Year must contain 5 digits");
                    }

                    year = int.Parse(partValue); // There is no reason to fail.

                    year += formatInfo.IsYearShortFormat == true ? 2000 : 0;

                    input = input.Substring(partValue.Length);
                }
                else if (i == formatInfo.MonthPosition)
                {
                    partValue = new string(input.TakeWhile(x => char.IsDigit(x)).ToArray());

                    if (formatInfo.MonthFormat.Length == 1 && (partValue.Length < 1 || partValue.Length > 2)) // 'M'
                    {
                        return ThrowOnError<FormatException>("Invalid month value. Month must contain 1 or 2 digits");
                    }
                    else if (formatInfo.MonthFormat.Length == 2 && partValue.Length != 2) // 'MM'
                    {
                        return ThrowOnError<FormatException>("Invalid month value. Month must contain 2 digits");
                    }

                    month = int.Parse(partValue); // There is no reason to fail.

                    input = input.Substring(partValue.Length);
                }
                else if (i == formatInfo.DayPosition)
                {
                    partValue = new string(input.TakeWhile(x => char.IsDigit(x)).ToArray());

                    if (formatInfo.DayFormat.Length == 1 && (partValue.Length < 1 || partValue.Length > 2)) // 'd'
                    {
                        return ThrowOnError<FormatException>("Invalid day value. Day must contain 1 or 2 digits");
                    }
                    else if (formatInfo.DayFormat.Length == 2 && partValue.Length != 2) // 'dd'
                    {
                        return ThrowOnError<FormatException>("Invalid day value. Day must contain 2 digits");
                    }

                    day = int.Parse(partValue); // There is no reason to fail.

                    input = input.Substring(partValue.Length);
                }
                else if (i == formatInfo.HourPosition)
                {
                    partValue = new string(input.TakeWhile(x => char.IsDigit(x)).ToArray());

                    if (formatInfo.HourFormat.Length == 1 && (partValue.Length < 1 || partValue.Length > 2)) // 'h' or 'H'
                    {
                        return ThrowOnError<FormatException>("Invalid hour value. Hour must contain 1 or 2 digits");
                    }
                    else if (formatInfo.HourFormat.Length == 2 && partValue.Length != 2) // 'hh' or 'HH'
                    {
                        return ThrowOnError<FormatException>("Invalid hour value. Hour must contain 2 digits");
                    }

                    hour = int.Parse(partValue); // There is no reason to fail.

                    if (formatInfo.IsHour12Format == true && (hour > 12 || hour < 1))
                    {
                        return ThrowOnError<FormatException>("Invalid hour value. Hour must be in range <1,12> for short format");
                    }

                    input = input.Substring(partValue.Length);
                }
                else if (i == formatInfo.MinutePosition)
                {
                    partValue = new string(input.TakeWhile(x => char.IsDigit(x)).ToArray());

                    if (formatInfo.MinuteFormat.Length == 1 && (partValue.Length < 1 || partValue.Length > 2)) // 'm'
                    {
                        return ThrowOnError<FormatException>("Invalid minute value. Minute must contain 1 or 2 digits");
                    }
                    else if (formatInfo.MinuteFormat.Length == 2 && partValue.Length != 2) // 'mm'
                    {
                        return ThrowOnError<FormatException>("Invalid minute value. Minute must contain 2 digits");
                    }

                    minute = int.Parse(partValue); // There is no reason to fail.

                    input = input.Substring(partValue.Length);
                }
                else if (i == formatInfo.SecondPosition)
                {
                    partValue = new string(input.TakeWhile(x => char.IsDigit(x)).ToArray());

                    if (formatInfo.SecondFormat.Length == 1 && (partValue.Length < 1 || partValue.Length > 2))
                    {
                        return ThrowOnError<FormatException>("Invalid second value. Second must contain 1 or 2 digits"); // 's'
                    }
                    else if (formatInfo.SecondFormat.Length == 2 && partValue.Length != 2) // 'ss'
                    {
                        return ThrowOnError<FormatException>("Invalid second value. Second must contain 2 digits");
                    }

                    second = int.Parse(partValue); // There is no reason to fail.

                    input = input.Substring(partValue.Length);
                }
                else if (i == formatInfo.AmPmPosition)
                {
                    partValue = new string(input.Take(formatInfo.AmPmFormat.Length).ToArray());

                    if (formatInfo.IsAmPmShortFormat == true && !(Meridiems.IsShortAm(partValue) || Meridiems.IsShortPm(partValue)))
                    {
                        return ThrowOnError<FormatException>("Invalid AM/PM designator value");
                    }
                    else if (formatInfo.IsAmPmShortFormat == false && !(Meridiems.IsLongAm(partValue) || Meridiems.IsLongPm(partValue)))
                    {
                        return ThrowOnError<FormatException>("Invalid AM/PM designator value");
                    }

                    amPm = partValue;

                    hour += Meridiems.IsPm(amPm) ? 12 : 0;

                    input = input.Substring(partValue.Length);
                }
            }

            separator = formatInfo.GetSeparator(i);
            if (separator != null && separator.Length > 0)
            {
                if (!input.StartsWith(separator))
                {
                    return ThrowOnError<FormatException>("Invalid value. Value does not match to format");
                }
            }

            try
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception exc)
            {
                return ThrowOnError<FormatException>("Invalid value. It's not possible to build new DateTime object", exc);
            }
        }

        private static DateTime? ThrowOnError<TException>(string exceptionMessage, Exception innerException = null, bool throwOnError = true)
            where TException : Exception
        {
            if (throwOnError)
            {
                throw innerException == null ?
                    (TException)Activator.CreateInstance(typeof(TException), new object[] { exceptionMessage }) :
                    (TException)Activator.CreateInstance(typeof(TException), new object[] { exceptionMessage, innerException });
            }

            return null;
        }
        #endregion
    }
}
