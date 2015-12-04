// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntryEventArgs.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using Catel.Logging;

    public class LogEntryEventArgs : EventArgs
    {
        public LogEntryEventArgs(IEnumerable<LogEntry> logEntries)
        {
            LogEntries = new List<LogEntry>(logEntries);
        }

        public List<LogEntry> LogEntries { get; private set; }
    }
}