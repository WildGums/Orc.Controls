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
            var parts = new List<string>();

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
                        if (part != null)
                        {
                            parts.Add(part);
                        }

                        part = string.Empty;
                    }
                }

                part += c;
                prev = c;
            }

            if (part != null)
            {
                parts.Add(part);
            }

            return parts.ToArray();
        }

        public static DateTimeFormatInfo GetDateTimeFormatInfo(string format, bool isDateOnly = false)
        {
            Argument.IsNotNull(() => format);

            var result = new DateTimeFormatInfo();
            var parts = Split(format, new[] { 'y', 'M', 'd', 'H', 'h', 'm', 's', 't' });

            var current = 0;
            var count = 0;

            foreach (var part in parts)
            {
                count++;

                if ((count == 1 || count == parts.Length) && !part.All(char.IsLetter))
                {
                    continue;
                }

                if (part.Contains('d'))
                {
                    result.SetDayFormat(part, current++);

                    continue;
                }

                if (part.Contains('M'))
                {
                    result.SetMonthFormat(part, current++);

                    continue;
                }

                if (part.Contains('y'))
                {
                    result.SetYearFormat(part, current++);

                    continue;
                }

                if (isDateOnly && part.IsDateTimeFormatPart())
                {
                    throw Log.ErrorAndCreateException<FormatException>("Format string is incorrect. Time fields are not expected");
                }

                if (part.Contains('h') || part.Contains('H'))
                {
                    result.SetHourFormat(part, current++);

                    continue;
                }

                if (part.Contains('m'))
                {
                    result.SetMinuteFormat(part, current++);

                    continue;
                }

                if (part.Contains('s'))
                {
                    result.SetSecondFormat(part, current++);

                    continue;
                }

                if (part.Contains('t'))
                {
                    result.SetAmPmFormat(part, current++);

                    continue;
                }

                result.SetSeparator(part, current);
            }

            if (!result.IsCorrect(isDateOnly, out var errowMessage))
            {
                throw Log.ErrorAndCreateException<FormatException>(errowMessage);
            }
            
            if (!isDateOnly)
            {
                result.IsAmPmShortFormat = result.AmPmFormat != null && result.AmPmFormat.Length < 2;
            }

            result.MaxPosition = current - 1;

            return result;
        }
        #endregion
    }
}
