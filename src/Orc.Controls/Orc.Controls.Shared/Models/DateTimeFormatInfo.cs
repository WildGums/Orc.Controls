// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeFormatInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.Logging;
    using System;

    public class DateTimeFormatInfo
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

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
        public bool IsDateOnly { get { return HourPosition == null && MinutePosition == null && SecondPosition == null && AmPmPosition == null; } }
        public bool IsYearShortFormat { get; set; }
        public bool? IsHour12Format { get; set; }
        public bool? IsAmPmShortFormat { get; set; }
        public int MaxPosition { get; set; }
        #endregion

        #region Methods
        public string GetSeparator(int position)
        {
            Argument.IsNotOutOfRange(() => position, 0, 7);

            switch (position)
            {
                case 0: return Separator0;
                case 1: return Separator1;
                case 2: return Separator2;
                case 3: return Separator3;
                case 4: return Separator4;
                case 5: return Separator5;
                case 6: return Separator6;
                case 7: return Separator7;
                default: return null;
            }
        }
        #endregion
    }
}
