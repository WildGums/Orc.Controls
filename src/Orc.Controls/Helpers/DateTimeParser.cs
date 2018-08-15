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
    using System.Linq;

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

            dateTime = (DateTime)result;
            return true;
        }

        private static DateTime? Parse(string input, DateTimeFormatInfo formatInfo, bool throwOnError)
        {
            Argument.IsNotNull(() => formatInfo);

            var year = 0;
            var month = 0;
            var day = 0;
            var hour = 0;
            var minute = 0;
            var second = 0;
            string separator;

            int i;
            for (i = 0; i < 7; i++)
            {
                separator = formatInfo.GetSeparator(i);

                if (!string.IsNullOrEmpty(separator) && input.Length > 0)
                {
                    if (!input.StartsWith(separator))
                    {
                        return ThrowOnError<FormatException>("Invalid value. Value does not match format", null, throwOnError);
                    }

                    input = input.Substring(separator.Length);
                }

                if (separator == null && i == formatInfo.MaxPosition + 1 && input.Length > 0)
                {
                    input = ParseAmPmFormat(input, formatInfo, i);
                }

                if (i == formatInfo.YearPosition)
                {
                    var partValue = new string(input.TakeWhile(char.IsDigit).ToArray());
                    if (!formatInfo.CheckYearFormat(partValue, out var errorMessage))
                    {
                        return ThrowOnError<FormatException>(errorMessage, null, throwOnError);
                    }

                    year = int.Parse(partValue); // There is no reason to fail.
                    year += formatInfo.IsYearShortFormat ? 2000 : 0;

                    input = input.Substring(partValue.Length);

                    continue;
                }

                if (i == formatInfo.MonthPosition)
                {
                    var partValue = new string(input.TakeWhile(char.IsDigit).ToArray());
                    if (!formatInfo.CheckMonthFormat(partValue, out var errorMessage))
                    {
                        return ThrowOnError<FormatException>(errorMessage, null, throwOnError);
                    }

                    month = int.Parse(partValue); // There is no reason to fail.

                    input = input.Substring(partValue.Length);

                    continue;
                }

                if (i == formatInfo.DayPosition)
                {
                    var partValue = new string(input.TakeWhile(char.IsDigit).ToArray());
                    if (!formatInfo.CheckDayFormat(partValue, out var errorMessage))
                    {
                        return ThrowOnError<FormatException>(errorMessage, null, throwOnError);
                    }

                    day = int.Parse(partValue); // There is no reason to fail.

                    input = input.Substring(partValue.Length);

                    continue;
                }

                if (i == formatInfo.HourPosition)
                {
                    var partValue = new string(input.TakeWhile(char.IsDigit).ToArray());
                    if (!formatInfo.CheckHourFormat(partValue, out var errorMessage))
                    {
                        return ThrowOnError<FormatException>(errorMessage, null, throwOnError);
                    }

                    hour = int.Parse(partValue); // There is no reason to fail.
                    if (hour < 0 && hour >= 24)
                    {
                        return ThrowOnError<FormatException>("Invalid hour value. Hour must be in range <0, 24> for short format", null, throwOnError);
                    }

                    input = input.Substring(partValue.Length);

                    continue;
                }

                if (i == formatInfo.MinutePosition)
                {
                    var partValue = new string(input.TakeWhile(char.IsDigit).ToArray());
                    if (!formatInfo.CheckMinuteFormat(partValue, out var errorMessage))
                    {
                        return ThrowOnError<FormatException>(errorMessage, null, throwOnError);
                    }

                    minute = int.Parse(partValue); // There is no reason to fail.

                    input = input.Substring(partValue.Length);

                    continue;
                }

                if (i == formatInfo.SecondPosition)
                {
                    var partValue = new string(input.TakeWhile(char.IsDigit).ToArray());
                    if (!formatInfo.CheckSecondFormat(partValue, out var errorMessage))
                    {
                        return ThrowOnError<FormatException>(errorMessage, null, throwOnError);
                    }

                    second = int.Parse(partValue); // There is no reason to fail.

                    input = input.Substring(partValue.Length);

                    continue;
                }

                if (i != formatInfo.AmPmPosition)
                {
                    continue;
                }

                var amPmPartValue = new string(input.Take(formatInfo.AmPmFormat.Length).ToArray());
                if (amPmPartValue.Length <= 0)
                {
                    // We allow the input string to not have AM/PM even if the format specifies them
                    continue;
                }

                if (!formatInfo.CheckIsAmPmShortFormat(amPmPartValue, out var amPmErrorMessage))
                {
                    return ThrowOnError<FormatException>(amPmErrorMessage, null, throwOnError);
                }

                var amPm = amPmPartValue;

                // We need to make sure hour is less than 12, because we could accept values like: 05/02/2017 16:41:24 PM
                hour += Meridiems.IsPm(amPm) && hour < 12 ? 12 : 0;

                input = input.Substring(amPmPartValue.Length);
            }

            separator = formatInfo.GetSeparator(i);
            if (!string.IsNullOrEmpty(separator) && !input.StartsWith(separator))
            {
                return ThrowOnError<FormatException>("Invalid value. Value does not match to format", null, throwOnError);
            }

            try
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (Exception exc)
            {
                return ThrowOnError<FormatException>("Invalid value. It's not possible to build new DateTime object", exc, throwOnError);
            }
        }

        private static string ParseAmPmFormat(string input, DateTimeFormatInfo formatInfo, int position)
        {
            // We have come to the end of format but there is still some characters left in the input.
            // They are probably AM/PM values even though the format is 24 hours. We will try and parse them correctly

            formatInfo.AmPmPosition = position;

            input = input.Trim();

            switch (input.Length)
            {
                case 2:
                    formatInfo.AmPmFormat = "tt";
                    break;

                case 1:
                    formatInfo.AmPmFormat = "t";
                    break;
            }

            return input;
        }

        private static DateTime? ThrowOnError<TException>(string exceptionMessage, Exception innerException, bool throwOnError)
            where TException : Exception
        {
            if (throwOnError)
            {
                throw innerException == null ?
                    Log.ErrorAndCreateException<TException>(exceptionMessage) :
                    Log.ErrorAndCreateException<TException>(innerException, exceptionMessage);
            }

            Log.Warning(exceptionMessage);

            return null;
        }
        #endregion
    }
}
