namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class SuggestionListService : ISuggestionListService
    {
        #region ISuggestionListService Members
        public List<KeyValuePair<string, string>> GetSuggestionList(DateTime dateTime, DateTimePart editablePart, Orc.Controls.DateTimeFormatInfo dateTimeFormatInfo)
        {
            switch (editablePart)
            {
                case DateTimePart.Day:
                    return CreateDays(dateTime);

                case DateTimePart.Month:
                    return CreateMonths(dateTime);

                case DateTimePart.Hour:
                    return CreateHours();

                case DateTimePart.Hour12:
                    return CreateHours12();

                case DateTimePart.Minute:
                    return CreateMinutes();

                case DateTimePart.Second:
                    return CreateSeconds();

                case DateTimePart.AmPmDesignator:
                    var meridiems = new List<KeyValuePair<string, string>>
                    {
                        CreateItem(Meridiems.LongAM, Meridiems.GetAmForFormat(dateTimeFormatInfo)),
                        CreateItem(Meridiems.LongPM, Meridiems.GetPmForFormat(dateTimeFormatInfo))
                    };
                    return meridiems;

                default:
                    throw new InvalidOperationException();
            }
        }
        #endregion

        #region Methods
        private static List<KeyValuePair<string, string>> CreateDays(DateTime dateTime)
        {
            var days = new List<KeyValuePair<string, string>>();
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            for (var i = 1; i <= daysInMonth; i++)
            {
                var day = new DateTime(dateTime.Year, dateTime.Month, i).ToString("d ddd", CultureInfo.InvariantCulture);
                days.Add(CreateItem(i.ToString(), day));
            }

            return days;
        }

        private static List<KeyValuePair<string, string>> CreateMonths(DateTime dateTime)
        {
            var months = new List<KeyValuePair<string, string>>();
            for (var i = 1; i <= 12; i++)
            {
                var month = new DateTime(dateTime.Year, i, 1).ToString("M MMM", CultureInfo.InvariantCulture);
                months.Add(CreateItem(i.ToString(), month));
            }

            return months;
        }

        private static List<KeyValuePair<string, string>> CreateHours()
        {
            var hours = new List<KeyValuePair<string, string>>();
            for (var i = 0; i < 24; i++)
            {
                hours.Add(i < 10 ? CreateItem(i.ToString(), "  " + i) : CreateItem(i.ToString(), i.ToString()));
            }

            return hours;
        }

        private static List<KeyValuePair<string, string>> CreateHours12()
        {
            var hours12 = new List<KeyValuePair<string, string>>();
            for (var i = 1; i <= 12; i++)
            {
                hours12.Add(i < 10 ? CreateItem(i.ToString(), "  " + i) : CreateItem(i.ToString(), i.ToString()));
            }

            return hours12;
        }

        private static List<KeyValuePair<string, string>> CreateMinutes()
        {
            var minutes = new List<KeyValuePair<string, string>>();
            for (var i = 0; i < 60; i++)
            {
                minutes.Add(CreateItem(i.ToString("00"), i.ToString("00")));
            }

            return minutes;
        }

        private static List<KeyValuePair<string, string>> CreateSeconds()
        {
            var seconds = new List<KeyValuePair<string, string>>();
            for (var i = 0; i < 60; i++)
            {
                seconds.Add(CreateItem(i.ToString("00"), i.ToString("00")));
            }

            return seconds;
        }

        private static KeyValuePair<string, string> CreateItem(string itemIndex, string item)
        {
            return new KeyValuePair<string, string>(itemIndex, item);
        }
        #endregion
    }
}
