namespace Orc.Controls;

using System;

public static class TimeSpanPartExtensions
{
    public static TimeSpan CreateTimeSpan(this TimeSpanPart timeSpanPart, double value)
    {
        return timeSpanPart switch
        {
            TimeSpanPart.Days => TimeSpan.FromDays(value),
            TimeSpanPart.Hours => TimeSpan.FromHours(value),
            TimeSpanPart.Minutes => TimeSpan.FromMinutes(value),
            TimeSpanPart.Seconds => TimeSpan.FromSeconds(value),
            _ => throw new InvalidOperationException()
        };
    }

    public static double GetTimeSpanPartValue(this TimeSpan value, TimeSpanPart timeSpanPart)
    {
        return timeSpanPart switch
        {
            TimeSpanPart.Days => value.TotalDays,
            TimeSpanPart.Hours => value.TotalHours,
            TimeSpanPart.Minutes => value.TotalMinutes,
            TimeSpanPart.Seconds => value.TotalSeconds,
            _ => throw new InvalidOperationException()
        };
    }

    public static string GetTimeSpanPartName(this TimeSpanPart timeSpanPart)
    {
        return timeSpanPart switch
        {
            TimeSpanPart.Days => "days",
            TimeSpanPart.Hours => "hours",
            TimeSpanPart.Minutes => "minutes",
            TimeSpanPart.Seconds => "seconds",
            _ => throw new InvalidOperationException()
        };
    }
}
