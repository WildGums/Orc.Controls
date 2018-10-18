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
    using System.Globalization;
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
            if (string.IsNullOrWhiteSpace(format))
            {
                // Fallback to nothing
                format = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            }

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

                current = result.FormatPart(part, current, isDateOnly);
            }

            if (!result.IsCorrect(isDateOnly, out var errorMessage))
            {
                throw Log.ErrorAndCreateException<FormatException>(errorMessage);
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
