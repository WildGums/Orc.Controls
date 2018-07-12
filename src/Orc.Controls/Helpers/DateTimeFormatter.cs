// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatter.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class DateTimeFormatter
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods
        public static string Format(DateTime dateTime, string format, bool isDateOnly = false)
        {
            return Format(dateTime, DateTimeFormatHelper.GetDateTimeFormatInfo(format, isDateOnly));
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
                parts.Add(new KeyValuePair<int, string>(formatInfo.SecondPosition.Value, dateTime.Second.ToString(NumberFormatHelper.GetFormat(formatInfo.SecondFormat.Length))));
                if (formatInfo.AmPmPosition.HasValue)
                {
                    parts.Add(new KeyValuePair<int, string>(formatInfo.AmPmPosition.Value, dateTime.Hour >= 12 ? Meridiems.GetPmForFormat(formatInfo) : Meridiems.GetAmForFormat(formatInfo)));
                }
            }

            parts = parts.OrderBy(x => x.Key).ToList();

            // Always contain year, month, day part.
            var builder = new StringBuilder();
            builder.Append(formatInfo.Separator0);
            builder.Append(parts[0].Value);
            builder.Append(formatInfo.Separator1);
            builder.Append(parts[1].Value);
            builder.Append(formatInfo.Separator2);
            builder.Append(parts[2].Value);
            builder.Append(formatInfo.Separator3);

            if (parts.Count <= 3)
            {
                return builder.ToString();
            }

            builder.Append(parts[3].Value);
            builder.Append(formatInfo.Separator4);
            builder.Append(parts[4].Value);
            builder.Append(formatInfo.Separator5);
            builder.Append(parts[5].Value);
            builder.Append(formatInfo.Separator6);

            if (parts.Count <= 6)
            {
                return builder.ToString();
            }

            builder.Append(parts[6].Value);
            builder.Append(formatInfo.Separator7);

            return builder.ToString();
        }
        #endregion
    }
}
