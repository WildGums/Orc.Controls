﻿namespace Orc.Controls;

using System.Collections.ObjectModel;
using System.Linq;
using Catel.Data;
using Catel.Logging;

public class LogFilterGroup : ModelBase
{
    public string? Name { get; set; }

    public bool IsRuntime { get; set; }

    public bool IsEnabled { get; set; } = true;

    public ObservableCollection<LogFilter> LogFilters { get; } = new();

    public bool Pass(LogEntry logEntry)
    {
        return LogFilters.All(filter => filter.Pass(logEntry));
    }

    public override string? ToString()
    {
        return Name;
    }
}
