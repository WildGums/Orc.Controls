// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecord.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.Models
{
    using System;
    using Catel.Logging;

    public class LogRecord
    {
        #region Properties
        public DateTime DateTime { get; set; }

        public LogEvent LogEvent { get; set; }

        public string Message { get; set; }
        #endregion
    }
}