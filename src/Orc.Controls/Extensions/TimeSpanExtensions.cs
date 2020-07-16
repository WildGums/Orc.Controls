// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    internal static class TimeSpanExtensions
    {
        #region Methods
        public static TimeSpan RoundTimeSpan(this TimeSpan timeSpan)
        {
            var totalSeconds = Math.Round(timeSpan.TotalSeconds);
            return TimeSpan.FromSeconds(totalSeconds);
        }
        #endregion
    }
}
