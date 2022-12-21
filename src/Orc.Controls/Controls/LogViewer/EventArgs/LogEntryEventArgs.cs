namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using Catel.Logging;

    public class LogEntryEventArgs : EventArgs
    {
        #region Constructors
        public LogEntryEventArgs(List<LogEntry> logEntries, List<LogEntry> filteredLogEntries)
        {
            LogEntries = logEntries;
            FilteredLogEntries = filteredLogEntries;
        }
        #endregion

        #region Properties
        public List<LogEntry> LogEntries { get; private set; }

        public List<LogEntry> FilteredLogEntries { get; private set; }
        #endregion
    }
}
