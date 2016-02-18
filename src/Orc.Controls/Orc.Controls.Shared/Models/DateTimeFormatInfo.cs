// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartHelper.cs" company="WildGums">
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
        public int DayPosition { get; internal set; }
        public int MonthPosition { get; internal set; }
        public int YearPosition { get; internal set; }
        public int? HourPosition { get; internal set; }
        public int? MinutePosition { get; internal set; }
        public int? SecondPosition { get; internal set; }
        public int? AmPmPosition { get; internal set; }
        public string DayFormat { get; internal set; }
        public string MonthFormat { get; internal set; }
        public string YearFormat { get; internal set; }
        public string HourFormat { get; internal set; }
        public string MinuteFormat { get; internal set; }
        public string SecondFormat { get; internal set; }
        public string AmPmFormat { get; internal set; }
        public string Separator0 { get; internal set; }
        public string Separator1 { get; internal set; }
        public string Separator2 { get; internal set; }
        public string Separator3 { get; internal set; }
        public string Separator4 { get; internal set; }
        public string Separator5 { get; internal set; }
        public string Separator6 { get; internal set; }
        public string Separator7 { get; internal set; }
        public bool IsYearShortFormat { get; internal set; }
        public bool? IsHour12Format { get; internal set; }
        public bool? IsAmPmShortFormat { get; internal set; }
        #endregion
    }
}
