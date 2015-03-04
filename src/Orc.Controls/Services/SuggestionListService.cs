// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestionListService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Interfaces;

    public class SuggestionListService : ISuggestionListService
    {
        #region ISuggestionListService Members
        public List<KeyValuePair<string, string>> GetSuggestionList(DateTime dateTime, DateTimePart editablePart)
        {
            switch (editablePart)
            {
                case DateTimePart.Day:
                    var days = new List<KeyValuePair<string, string>>();
                    var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                    for (var i = 1; i <= daysInMonth; i++)
                    {
                        var day = new DateTime(dateTime.Year, dateTime.Month, i).ToString("d ddd", CultureInfo.InvariantCulture);
                        days.Add(AlignItem(i, day));
                    }
                    return days;

                case DateTimePart.Month:
                    var months = new List<KeyValuePair<string, string>>();
                    for (var i = 1; i <= 12; i++)
                    {
                        var month = new DateTime(dateTime.Year, i, 1).ToString("M MMM", CultureInfo.InvariantCulture);
                        months.Add(AlignItem(i, month));
                    }
                    return months;

                case DateTimePart.Hour:
                    var hours = new List<KeyValuePair<string, string>>();
                    for (var i = 0; i < 24; i++)
                    {
                        hours.Add(AlignItem(i, i.ToString()));
                    }
                    return hours;

                case DateTimePart.Minute:
                    var minutes = new List<KeyValuePair<string, string>>();
                    for (var i = 0; i < 60; i++)
                    {
                        minutes.Add(AlignItem(i, i.ToString()));
                    }
                    return minutes;

                case DateTimePart.Second:
                    var seconds = new List<KeyValuePair<string, string>>();
                    for (var i = 0; i < 60; i++)
                    {
                        seconds.Add(AlignItem(i, i.ToString()));
                    }
                    return seconds;

                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion

        #region Methods
        private static KeyValuePair<string, string> AlignItem(int itemIndex, string item)
        {
            if (itemIndex < 10)
            {
                return new KeyValuePair<string, string>(itemIndex.ToString(), "  " + item);
            }
            return new KeyValuePair<string, string>(itemIndex.ToString(), item);
        }
        #endregion
    }
}