// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntryEventArgs.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Logging;

    public class LogEntryEventArgs : EventArgs
    {
        public LogEntryEventArgs(LogEntry logEntry)
        {
            LogEntry = logEntry;
        }

        public LogEntry LogEntry { get; private set; }
    }
}