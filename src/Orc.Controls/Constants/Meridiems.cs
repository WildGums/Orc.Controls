// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Meridiems.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    internal static class Meridiems
    {
        #region Constants
        public const string LongAM = "AM";
        public const string ShortAM = "A";
        public const string LongPM = "PM";
        public const string ShortPM = "P";
        #endregion

        #region Methods
        public static string GetAmForFormat(DateTimeFormatInfo formatInfo)
        {
            return formatInfo.IsAmPmShortFormat == true ? ShortAM : LongAM;
        }

        public static string GetPmForFormat(DateTimeFormatInfo formatInfo)
        {
            return formatInfo.IsAmPmShortFormat == true ? ShortPM : LongPM;
        }

        public static bool IsValid(string meridiem)
        {
            return (IsAm(meridiem) || IsPm(meridiem));
        }

        public static bool IsAm(string meridiem)
        {
            return (IsShortAm(meridiem) || IsLongAm(meridiem));
        }

        public static bool IsShortAm(string meridiem)
        {
            return string.Equals(ShortAM, meridiem, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsLongAm(string meridiem)
        {
            return string.Equals(LongAM, meridiem, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsPm(string meridiem)
        {
            return (IsShortPm(meridiem) || IsLongPm(meridiem));
        }

        public static bool IsShortPm(string meridiem)
        {
            return string.Equals(ShortPM, meridiem, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsLongPm(string meridiem)
        {
            return string.Equals(LongPM, meridiem, StringComparison.CurrentCultureIgnoreCase);
        }
        #endregion
    }
}
