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
                        days.Add(CreateItem(i.ToString(), day));
                    }
                    return days;

                case DateTimePart.Month:
                    var months = new List<KeyValuePair<string, string>>();
                    for (var i = 1; i <= 12; i++)
                    {
                        var month = new DateTime(dateTime.Year, i, 1).ToString("M MMM", CultureInfo.InvariantCulture);
                        months.Add(CreateItem(i.ToString(), month));
                    }
                    return months;

                case DateTimePart.Hour:
                    var hours = new List<KeyValuePair<string, string>>();
                    for (var i = 0; i < 24; i++)
                    {
                        hours.Add(i < 10 ? CreateItem(i.ToString(), ("  " + i)) : CreateItem(i.ToString(), i.ToString()));
                    }
                    return hours;

                case DateTimePart.Minute:
                    var minutes = new List<KeyValuePair<string, string>>();
                    for (var i = 0; i < 60; i++)
                    {
                        minutes.Add(CreateItem(i.ToString("00"), i.ToString("00")));
                    }
                    return minutes;

                case DateTimePart.Second:
                    var seconds = new List<KeyValuePair<string, string>>();
                    for (var i = 0; i < 60; i++)
                    {
                        seconds.Add(CreateItem(i.ToString("00"), i.ToString("00")));
                    }
                    return seconds;

                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion

        #region Methods
        private static KeyValuePair<string, string> CreateItem(string itemIndex, string item)
        {
            return new KeyValuePair<string, string>(itemIndex, item);
        }
        #endregion
    }
}