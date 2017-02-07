// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Meridiems.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
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
        #endregion
    }
}
