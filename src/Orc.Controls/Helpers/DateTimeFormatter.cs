namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public static class DateTimeFormatter
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(DateTimeFormatter));

    public static string Format(DateTime dateTime, string format, bool isDateOnly = false)
    {
        return Format(dateTime, DateTimeFormatHelper.GetDateTimeFormatInfo(format, isDateOnly));
    }

    internal static string Format(DateTime dateTime, DateTimeFormatInfo formatInfo)
    {
        ArgumentNullException.ThrowIfNull(formatInfo);

        var parts = new List<KeyValuePair<int, string>>
        {
            new (formatInfo.DayPosition, dateTime.Day.ToString(NumberFormatHelper.GetFormat(formatInfo.DayFormat?.Length ?? 0))),
            new (formatInfo.MonthPosition, dateTime.Month.ToString(NumberFormatHelper.GetFormat(formatInfo.MonthFormat?.Length ?? 0)))
        };

        if (formatInfo.IsYearShortFormat)
        {
            var yearShort = dateTime.Year - (dateTime.Year - dateTime.Year % 100);
            parts.Add(new KeyValuePair<int, string>(formatInfo.YearPosition, yearShort.ToString(NumberFormatHelper.GetFormat(formatInfo.YearFormat?.Length ?? 0))));
        }
        else
        {
            parts.Add(new KeyValuePair<int, string>(formatInfo.YearPosition, dateTime.Year.ToString(NumberFormatHelper.GetFormat(formatInfo.YearFormat?.Length ?? 0))));
        }

        if (!formatInfo.IsDateOnly)
        {
            var hourPosition = formatInfo.HourPosition;
            if (hourPosition is not null)
            {
                if (formatInfo.IsHour12Format == true)
                {
                    var hour12 = dateTime.Hour % 12 > 0 ? dateTime.Hour % 12 : 12;
                    parts.Add(new KeyValuePair<int, string>(hourPosition.Value, hour12.ToString(NumberFormatHelper.GetFormat(formatInfo.HourFormat?.Length ?? 0))));
                }
                else
                {
                    parts.Add(new KeyValuePair<int, string>(hourPosition.Value, dateTime.Hour.ToString(NumberFormatHelper.GetFormat(formatInfo.HourFormat?.Length ?? 0))));
                }
            }

            var minutePosition = formatInfo.MinutePosition;
            if (minutePosition is not null)
            {
                parts.Add(new KeyValuePair<int, string>(minutePosition.Value, dateTime.Minute.ToString(NumberFormatHelper.GetFormat(formatInfo.MinuteFormat?.Length ?? 0))));
            }

            var secondPosition = formatInfo.SecondPosition;
            if (secondPosition is not null)
            {
                parts.Add(new KeyValuePair<int, string>(secondPosition.Value, dateTime.Second.ToString(NumberFormatHelper.GetFormat(formatInfo.SecondFormat?.Length ?? 0))));
            }

            var amPmPosition = formatInfo.AmPmPosition;
            if (amPmPosition is not null)
            {
                parts.Add(new KeyValuePair<int, string>(amPmPosition.Value, dateTime.Hour >= 12 ? Meridiems.GetPmForFormat(formatInfo) : Meridiems.GetAmForFormat(formatInfo)));
            }
        }

        parts = parts.OrderBy(x => x.Key).ToList();

        return BuildFormatString(formatInfo, parts);
    }

    private static string BuildFormatString(DateTimeFormatInfo formatInfo, List<KeyValuePair<int, string>> parts)
    {
        // Always contain year, month, day part.
        var builder = new StringBuilder();

        builder.Append(formatInfo.Separator0);
        for (var i = 0; i < parts.Count; i++)
        {
            builder.Append(parts[i].Value);
            builder.Append(formatInfo.GetSeparator(i + 1));
        }

        return builder.ToString();
    }
}
