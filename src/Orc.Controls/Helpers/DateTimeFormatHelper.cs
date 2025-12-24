namespace Orc.Controls;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public static class DateTimeFormatHelper
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(DateTimeFormatHelper));

    private const string CorrectDateFormatForSure = "dd\\MMMM\\yyyy";

    private static readonly HashSet<char> TimeFormatChars = new()
    {
        'h',
        'm',
        's',
        't',
        'H'
    };

    public static string[] Split(string format, char[] formatCharacters)
    {
        var parts = new List<string>();

        char? prev = null;
        string? part = null;
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
        
    public static DateTimeFormatInfo GetDateTimeFormatInfo(string format, bool isDateOnly = false, bool throwException = true)
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

            current = result.FormatPart(part, current, isDateOnly, throwException);
        }
            
        if (!result.IsCorrect(true, !isDateOnly, out var errorMessage))
        {
            result.SetFormatError(errorMessage, throwException);
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
            return format[..timePosition].Trim();
        }

        return format;
    }

    public static string FixFormat(CultureInfo? culture, string format, out bool hadAnyTimeFormat)
    {
        hadAnyTimeFormat = false;

        if (culture is null)
        {
            return format;
        }

        var formatInfo = GetDateTimeFormatInfo(format, false, false);

        var datePattern = ExtractDatePatternFromFormat(format);

        var isFullDateFormat = !(formatInfo.DayFormat is null || formatInfo.MonthFormat is null || formatInfo.YearFormat is null);
        if (!isFullDateFormat)
        {
            var shortDatePattern = ExtractDatePatternFromFormat(culture.DateTimeFormat.ShortDatePattern);
            if (string.IsNullOrWhiteSpace(datePattern) && GetDateTimeFormatInfo(shortDatePattern, true, false).IsCorrect)
            {
                datePattern = shortDatePattern;
            }
            else
            {
                var onlyDateFormat = GetDateTimeFormatInfo(datePattern, true, false);
                    
                var correctDateFormat = culture.DateTimeFormat.GetAllDateTimePatterns()
                    .Select(ExtractDatePatternFromFormat)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x =>
                    {
                        var correctFormatInfo = GetDateTimeFormatInfo(x, false, false);
                        var isCorrect = correctFormatInfo.IsCorrect;
                        if (isCorrect)
                        {
                            return new
                            {
                                FormatInfo = correctFormatInfo,
                                Format = x,
                                Matches = onlyDateFormat.GetCountOfMatches(correctFormatInfo)
                            };
                        }

                        return null;
                    })
                    .Where(x => x is not null)
                    .Distinct()
                    .MaxBy(x => x?.Matches ?? 0);
                    
                datePattern = correctDateFormat?.Format ?? CorrectDateFormatForSure;
            }
        }

        var timePattern = ExtractTimePatternFromFormat(format);

        hadAnyTimeFormat = !string.IsNullOrWhiteSpace(timePattern);

        var hasLongTimeFormat = !(formatInfo.HourFormat is null || formatInfo.MinuteFormat is null || formatInfo.SecondFormat is null);
        if (!hasLongTimeFormat)
        {
            //Find matching format from culture, if no culture just use specified format
            if (!string.IsNullOrEmpty(timePattern))
            {
                timePattern = FindMatchedLongTimePattern(culture, timePattern);
            }

            if (string.IsNullOrEmpty(timePattern))
            {
                timePattern = culture.DateTimeFormat.LongTimePattern;
            }
        }

        format = $"{datePattern} {timePattern}";

        return format;
    }
}
