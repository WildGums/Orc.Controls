namespace Orc.Controls;

using System;
using System.Collections.Generic;
using Catel.Logging;

public class LogEntryEventArgs : EventArgs
{
    public LogEntryEventArgs(List<LogEntry> logEntries, List<LogEntry> filteredLogEntries)
    {
        LogEntries = logEntries;
        FilteredLogEntries = filteredLogEntries;
    }
        
    public List<LogEntry> LogEntries { get; }

    public List<LogEntry> FilteredLogEntries { get; }
}
