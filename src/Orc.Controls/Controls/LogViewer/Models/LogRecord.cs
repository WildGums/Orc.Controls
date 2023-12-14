namespace Orc.Controls;

using System;
using Catel.Logging;

public class LogRecord
{
    public DateTime DateTime { get; set; }

    public LogEvent LogEvent { get; set; }

    public string? Message { get; set; }
}
