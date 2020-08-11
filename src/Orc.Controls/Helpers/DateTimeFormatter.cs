// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Catel;
    using Catel.Logging;

    public static class DateTimeFormatter
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        public static string Format(DateTime dateTime, string format, bool isDateOnly = false)
        {
            return Format(dateTime, DateTimeFormatHelper.GetDateTimeFormatInfo(format, true, isDateOnly));
        }

        internal static string Format(DateTime dateTime, DateTimeFormatInfo formatInfo)
        {
            Argument.IsNotNull(() => formatInfo);

            var parts = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(formatInfo.DayPosition, dateTime.Day.ToString(NumberFormatHelper.GetFormat(formatInfo.DayFormat.Length))),
                new KeyValuePair<int, string>(formatInfo.MonthPosition, dateTime.Month.ToString(NumberFormatHelper.GetFormat(formatInfo.MonthFormat.Length)))
            };

            if (formatInfo.IsYearShortFormat)
            {
                var yearShort = dateTime.Year - (dateTime.Year - dateTime.Year % 100);
                parts.Add(new KeyValuePair<int, string>(formatInfo.YearPosition, yearShort.ToString(NumberFormatHelper.GetFormat(formatInfo.YearFormat.Length))));
            }
            else
            {
                parts.Add(new KeyValuePair<int, string>(formatInfo.YearPosition, dateTime.Year.ToString(NumberFormatHelper.GetFormat(formatInfo.YearFormat.Length))));
            }

            if (!formatInfo.IsDateOnly)
            {
                if (formatInfo.IsHour12Format == true)
                {
                    var hour12 = dateTime.Hour % 12 > 0 ? dateTime.Hour % 12 : 12;
                    parts.Add(new KeyValuePair<int, string>(formatInfo.HourPosition.Value, hour12.ToString(NumberFormatHelper.GetFormat(formatInfo.HourFormat.Length))));
                }
                else
                {
                    parts.Add(new KeyValuePair<int, string>(formatInfo.HourPosition.Value, dateTime.Hour.ToString(NumberFormatHelper.GetFormat(formatInfo.HourFormat.Length))));
                }

                parts.Add(new KeyValuePair<int, string>(formatInfo.MinutePosition.Value, dateTime.Minute.ToString(NumberFormatHelper.GetFormat(formatInfo.MinuteFormat.Length))));

                if (formatInfo.SecondPosition.HasValue)
                {
                    parts.Add(new KeyValuePair<int, string>(formatInfo.SecondPosition.Value, dateTime.Second.ToString(NumberFormatHelper.GetFormat(formatInfo.SecondFormat.Length))));
                }

                if (formatInfo.AmPmPosition.HasValue)
                {
                    parts.Add(new KeyValuePair<int, string>(formatInfo.AmPmPosition.Value, dateTime.Hour >= 12 ? Meridiems.GetPmForFormat(formatInfo) : Meridiems.GetAmForFormat(formatInfo)));
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
            for (int i = 0; i < parts.Count; i++)
            {
                builder.Append(parts[i].Value);
                builder.Append(formatInfo.GetSeparator(i + 1));
            }

            return builder.ToString();
        }
        #endregion
    }
}
