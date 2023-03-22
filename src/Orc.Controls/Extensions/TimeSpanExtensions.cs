namespace Orc.Controls;

using System;

internal static class TimeSpanExtensions
{
    public static TimeSpan RoundTimeSpan(this TimeSpan timeSpan)
    {
        var totalSeconds = Math.Round(timeSpan.TotalSeconds);
        return TimeSpan.FromSeconds(totalSeconds);
    }
}