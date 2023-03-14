namespace Orc.Controls;

using System;
using Catel;

public static class PredefinedDateRanges
{
    public static DateRange Today
    {
        get
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var today = now.Date;

            return new DateRange
            {
                Name = LanguageHelper.GetRequiredString("Controls_DateRangePicker_Today"),
                Start = today,
                End = now
            };
        }
    }

    public static DateRange Yesterday
    {
        get
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var today = now.Date;
            var yesterday = today.AddDays(-1);

            return new DateRange
            {
                Name = LanguageHelper.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_Yesterday)),
                Start = yesterday,
                End = today
            };
        }
    }

    public static DateRange ThisWeek
    {
        get
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var today = now.Date;

            var diff = today.DayOfWeek - DayOfWeek.Monday;
            diff += (diff < 0 ? 7 : 0);

            var fdow = today.AddDays(-diff);

            return new DateRange
            {
                Name = LanguageHelper.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_ThisWeek)),
                Start = fdow,
                End = now
            };
        }
    }

    public static DateRange ThisMonth
    {
        get
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var fdom = new DateTime(now.Year, now.Month, 1, 0, 0, 0);

            return new DateRange
            {
                Name = LanguageHelper.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_ThisMonth)),
                Start = fdom,
                End = now
            };
        }
    }

    public static DateRange LastWeek
    {
        get
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var today = now.Date;

            var diff = today.DayOfWeek - DayOfWeek.Monday;
            diff += (diff < 0 ? 7 : 0);

            var fdow = today.AddDays(-diff).Date;
            var fdolw = fdow.AddDays(-7);

            return new DateRange
            {
                Name = LanguageHelper.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_LastWeek)),
                Start = fdolw,
                End = fdow
            };
        }
    }

    public static DateRange LastMonth
    {
        get
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var fdom = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            var fdolm = fdom.AddMonths(-1);

            return new DateRange
            {
                Name = LanguageHelper.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_LastMonth)),
                Start = fdolm,
                End = fdom
            };
        }
    }
}
