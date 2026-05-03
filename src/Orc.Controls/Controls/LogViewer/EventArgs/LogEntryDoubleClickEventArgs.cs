namespace Orc.Controls;

using System;
using Catel.Logging;

public class LogEntryDoubleClickEventArgs : EventArgs
{
    public LogEntryDoubleClickEventArgs(LogEntry logEntry)
    {
        LogEntry = logEntry;
    }

    public LogEntry LogEntry { get; }
}
