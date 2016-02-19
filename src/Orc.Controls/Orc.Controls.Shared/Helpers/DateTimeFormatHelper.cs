// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DateTimeFormatHelper
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        public static string[] Split(string format, char[] formatCharacters)
        {
            List<string> parts = new List<string>();

            char? prev = null;
            string part = null;
            foreach (var c in format)
            {
                if (c != prev)
                {
                    if ((formatCharacters.Contains(c) && (prev.HasValue && formatCharacters.Contains(prev.Value)))
                        || (formatCharacters.Contains(c) && (prev.HasValue && !formatCharacters.Contains(prev.Value)))
                        || (!formatCharacters.Contains(c) && (prev.HasValue && formatCharacters.Contains(prev.Value))))
                    {
                        if (part != null) parts.Add(part);
                        part = string.Empty;
                    }
                }
                part += c;
                prev = c;
            }
            if (part != null) parts.Add(part);

            return parts.ToArray();
        }

        public static DateTimeFormatInfo GetDateTimeFormatInfo(string format, bool isDateOnly = false)
        {
            Argument.IsNotNull(() => format);

            var result = new DateTimeFormatInfo();
            var parts = Split(format, new char[] { 'y', 'M', 'd', 'H', 'h', 'm', 's', 't' });

            var current = 0;
            string hourFormat = null, hour12Format = null;
            foreach (string part in parts)
            {
                if (part.Contains('d'))
                {
                    if (result.DayFormat != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Day field can not be specified more than once");
                    if (part.Length > 2) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Day field must be in one of formats: 'd' or 'dd'");

                    result.DayFormat = part;
                    result.DayPosition = current++;
                }
                else if (part.Contains('M'))
                {
                    if (result.MonthFormat != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Month field can not be specified more than once");
                    if (part.Length > 2) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Month field must be in one of formats: 'M' or 'MM'");

                    result.MonthFormat = part;
                    result.MonthPosition = current++;
                }
                else if (part.Contains('y'))
                {
                    if (result.YearFormat != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Year field can not be specified more than once");
                    if (part.Length > 5) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Year field must be in one of formats: 'y' or 'yy' or 'yyy' or 'yyyy' or 'yyyyy'");

                    result.YearFormat = part;
                    result.YearPosition = current++;
                }
                else if (part.Contains('h'))
                {
                    if (isDateOnly) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrent. Time fields are not expected");
                    if (hourFormat != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Hour field must be 12 hour or 24 hour format, but no both");
                    if (hour12Format != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Hour field can not be specified more than once");
                    if (part.Length > 2) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Hour field must be in one of formats: 'h' or 'H' or 'hh' or 'HH'");

                    hour12Format = part;
                    result.HourPosition = current++;
                }
                else if (part.Contains('H'))
                {
                    if (isDateOnly) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrent. Time fields are not expected");
                    if (hour12Format != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Hour field must be 12 hour or 24 hour format, but no both");
                    if (hourFormat != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Hour field can not be specified more than once");
                    if (part.Length > 2) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Hour field must be in one of formats: 'h' or 'H' or 'hh' or 'HH'");

                    hourFormat = part;
                    result.HourPosition = current++;
                }
                else if (part.Contains('m'))
                {
                    if (isDateOnly) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrent. Time fields are not expected");
                    if (result.MinuteFormat != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Minute field can not be specified more than once");
                    if (part.Length > 2) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Minute field must be in one of formats: 'm' or 'mm'");

                    result.MinuteFormat = part;
                    result.MinutePosition = current++;
                }
                else if (part.Contains('s'))
                {
                    if (isDateOnly) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrent. Time fields are not expected");
                    if (result.SecondFormat != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Second field can not be specified more than once");
                    if (part.Length > 2) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Second field must be in one of formats: 's' or 'ss'");

                    result.SecondFormat = part;
                    result.SecondPosition = current++;
                }
                else if (part.Contains('t'))
                {
                    if (isDateOnly) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrent. Time fields are not expected");
                    if (result.AmPmFormat != null) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. AM/PM designator field can not be specified more than once");
                    if (part.Length > 2) throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. AM/PM designator field must be in one of formats: 't' or 'tt'");

                    result.AmPmFormat = part;
                    result.AmPmPosition = current++;
                }
                else
                {
                    if (current == 0) result.Separator0 = part;
                    else if (current == 1) result.Separator1 = part;
                    else if (current == 2) result.Separator2 = part;
                    else if (current == 3) result.Separator3 = part;
                    else if (current == 4) result.Separator4 = part;
                    else if (current == 5) result.Separator5 = part;
                    else if (current == 6) result.Separator6 = part;
                    else if (current == 7) result.Separator7 = part;
                }
            }
            result.HourFormat = hour12Format == null ? hourFormat : hour12Format;

            if (isDateOnly && (result.DayFormat == null || result.MonthFormat == null || result.YearFormat == null))
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Day, month and year fields are mandatory");
            }
            if (!isDateOnly && (result.DayFormat == null || result.MonthFormat == null || result.YearFormat == null || result.HourFormat == null || result.MinuteFormat == null || result.SecondFormat == null))
            {
                throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Day, month, year, hour, minute and second fields are mandatory");
            }

            result.IsYearShortFormat = result.YearFormat.Length < 3;
            if (!isDateOnly)
            {
                result.IsHour12Format = result.HourFormat.Contains('h');
                result.IsAmPmShortFormat = result.AmPmFormat != null && result.AmPmFormat.Length < 2;
            }

            return result;
        }
        #endregion
    }
}
