// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateRange.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.Data;
    using System;

    public class DateRange : ModelBase
    {
        #region Properties
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get { return End.Subtract(Start); } }
        public bool IsTemporary { get; internal set; } = false;
        #endregion
    }
}
