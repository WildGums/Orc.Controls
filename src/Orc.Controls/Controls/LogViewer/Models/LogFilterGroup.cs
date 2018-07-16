// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterGroup.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel.Data;
    using Catel.Logging;

    public class LogFilterGroup : ModelBase
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; } = true;

        public ObservableCollection<LogFilter> LogFilters { get; set; } = new ObservableCollection<LogFilter>();

        public bool Pass(LogEntry logEntry)
        {
            return LogFilters.All(filter => filter.Pass(logEntry));
        }
    }
}
