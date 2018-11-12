// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateRange.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Data;

    public class DateRange : ModelBase
    {
        #region Properties
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration => End.Subtract(Start);
        public bool IsTemporary { get; internal set; } = false;
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{Name} ({Start} => {End})";
        }
        #endregion
    }
}
