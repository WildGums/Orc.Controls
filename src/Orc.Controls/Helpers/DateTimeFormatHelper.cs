namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Catel.Logging;

    public static class DateTimeFormatHelper
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly HashSet<char> TimeFormatChars = new()
        {
            'h',
            'm',
            's',
            't',
            'H'
        };
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
                    if (formatCharacters.Contains(c) && prev.HasValue && formatCharacters.Contains(prev.Value)
                        || formatCharacters.Contains(c) && prev.HasValue && !formatCharacters.Contains(prev.Value)
                        || !formatCharacters.Contains(c) && prev.HasValue && formatCharacters.Contains(prev.Value))
                    {
                        if (part is not null)
                        {
                            parts.Add(part);
                        }

                        part = string.Empty;
                    }
                }

                part += c;
                prev = c;
            }

            if (part is not null)
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
            var parts = Split(format, new[] {'y', 'M', 'd', 'H', 'h', 'm', 's', 't'});

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

            if (!result.IsCorrect(true, !isDateOnly, out var errorMessage))
            {
                Log.Warning(errorMessage);

                throw new FormatException(errorMessage);
            }

            if (!isDateOnly)
            {
                result.IsAmPmShortFormat = result.AmPmFormat is not null && result.AmPmFormat.Length < 2;
            }

            result.MaxPosition = current - 1;

            return result;
        }

        public static string FindMatchedLongTimePattern(CultureInfo cultureInfo, string timePattern)
        {
            var timeChars = new HashSet<char>(timePattern.Where(x => TimeFormatChars.Contains(x)));

            var patterns = cultureInfo.DateTimeFormat.GetAllDateTimePatterns()
                .Select(ExtractTimePatternFromFormat).Distinct()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new {Pattern = x, MatchesCount = x.Count(y => timeChars.Contains(y))})
                .OrderByDescending(x => x.MatchesCount)
                .Select(x => x.Pattern)
                .ToList();

            var result = patterns.FirstOrDefault() ?? string.Empty;

            return result;
        }

        public static string ExtractTimePatternFromFormat(string format)
        {
            var timeCharPositions = format.Select((x, i) => new {Char = x, Position = i})
                .Where(x => TimeFormatChars.Contains(x.Char)).GroupBy(x => x.Char)
                .ToDictionary(x => x.Key, x => x.Min(y => y.Position));

            if (timeCharPositions.Any())
            {
                var timePosition = timeCharPositions.Values.Min();
                return format.Substring(timePosition).Trim();
            }

            return string.Empty;
        }

        public static string ExtractDatePatternFromFormat(string format)
        {
            var timeCharPositions = format.Select((x, i) => new { Char = x, Position = i })
                .Where(x => TimeFormatChars.Contains(x.Char)).GroupBy(x => x.Char)
                .ToDictionary(x => x.Key, x => x.Min(y => y.Position));

            if (timeCharPositions.Any())
            {
                var timePosition = timeCharPositions.Values.Min();
                return format.Substring(0, timePosition).Trim();
            }

            return format;
        }
        #endregion
    }
}
