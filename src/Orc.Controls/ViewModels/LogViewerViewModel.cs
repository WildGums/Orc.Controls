// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.MVVM;
    using Logging;

    public class LogViewerViewModel : ViewModelBase
    {
        #region Fields
        private readonly LogViewerLogListener _logListener = new LogViewerLogListener();

        private readonly List<LogEntry> _logEntries = new List<LogEntry>();
        #endregion

        #region Constructors
        public LogViewerViewModel()
        {

        }
        #endregion

        #region Properties
        public List<LogEntry> LogEntries { get { return _logEntries; } }

        public string LogFilter { get; set; }

        public bool ShowDebug { get; set; }
        public bool ShowInfo { get; set; }
        public bool ShowWarning { get; set; }
        public bool ShowError { get; set; }
        #endregion

        #region Events
        public event EventHandler<LogMessageEventArgs> LogMessage;
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();

            _logListener.LogMessage += OnLogMessage;

            LogManager.AddListener(_logListener);
        }

        protected override async Task Close()
        {
            _logListener.LogMessage -= OnLogMessage;

            LogManager.RemoveListener(_logListener);

            await base.Close();
        }

        public IEnumerable<LogEntry> GetFilteredLogEntries()
        {
            var entries = new List<LogEntry>();

            lock (_logEntries)
            {
                foreach (var entry in _logEntries)
                {
                    if (IsValidLogEntry(entry))
                    {
                        entries.Add(entry);
                    }
                }
            }

            return entries;
        }

        public bool IsValidLogEntry(LogEntry logEntry)
        {
            if (!PassFilter(logEntry))
            {
                return false;
            }

            if (!IsAcceptable(logEntry.LogEvent))
            {
                return false;
            }

            return true;
        }

        private bool IsAcceptable(LogEvent logEvent)
        {
            switch (logEvent)
            {
                case LogEvent.Debug:
                    return ShowDebug;

                case LogEvent.Info:
                    return ShowInfo;

                case LogEvent.Warning:
                    return ShowWarning;

                case LogEvent.Error:
                    return ShowError;
            }

            return false;
        }

        private bool PassFilter(LogEntry logEntry)
        {
            if (string.IsNullOrEmpty(LogFilter))
            {
                return true;
            }

            if (logEntry.Message.Contains(LogFilter))
            {
                return true;
            }

            return false;
        }

        private void OnLogMessage(object sender, LogMessageEventArgs e)
        {
            var logEntry = new LogEntry(e);

            lock (_logEntries)
            {
                _logEntries.Add(logEntry);
            }
        }
        #endregion
    }
}