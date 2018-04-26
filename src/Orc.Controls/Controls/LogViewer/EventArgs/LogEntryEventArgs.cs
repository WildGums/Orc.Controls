// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntryEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using Catel.Logging;

    public class LogEntryEventArgs : EventArgs
    {
        public LogEntryEventArgs(List<LogEntry> logEntries)
        {
            LogEntries = logEntries;
        }

        public List<LogEntry> LogEntries { get; private set; }
    }
}