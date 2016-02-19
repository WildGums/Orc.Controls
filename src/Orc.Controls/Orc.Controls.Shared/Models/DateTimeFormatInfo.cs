// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    public class DateTimeFormatInfo
    {
        #region Constructors
        public DateTimeFormatInfo()
        {

        }
        #endregion

        #region Properties
        public int DayPosition { get; set; }
        public int MonthPosition { get; set; }
        public int YearPosition { get; set; }
        public int? HourPosition { get; set; }
        public int? MinutePosition { get; set; }
        public int? SecondPosition { get; set; }
        public int? AmPmPosition { get; set; }
        public string DayFormat { get; set; }
        public string MonthFormat { get; set; }
        public string YearFormat { get; set; }
        public string HourFormat { get; set; }
        public string MinuteFormat { get; set; }
        public string SecondFormat { get; set; }
        public string AmPmFormat { get; set; }
        public string Separator0 { get; set; }
        public string Separator1 { get; set; }
        public string Separator2 { get; set; }
        public string Separator3 { get; set; }
        public string Separator4 { get; set; }
        public string Separator5 { get; set; }
        public string Separator6 { get; set; }
        public string Separator7 { get; set; }
        public bool IsYearShortFormat { get; set; }
        public bool? IsHour12Format { get; set; }
        public bool? IsAmPmShortFormat { get; set; }
        #endregion
    }
}
