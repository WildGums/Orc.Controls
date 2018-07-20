// --------------------------------------------------------------------------------------------------------------------
// <copyright file="logEntry.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
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
