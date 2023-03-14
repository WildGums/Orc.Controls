namespace Orc.Controls;

using Catel.Logging;

public class LogViewerLogListener : RollingInMemoryLogListener
{
    public LogViewerLogListener()
    {
        MaximumNumberOfLogEntries = 500;
    }
}
