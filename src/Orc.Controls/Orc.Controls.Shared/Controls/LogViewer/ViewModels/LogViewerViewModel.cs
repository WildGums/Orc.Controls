// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Logging;

    public class LogViewerViewModel : ViewModelBase
    {
        #region Constants
        private const string defaultComboBoxItem = "-- Select type name --";
        #endregion

        #region Fields
        private ILogListener _logListener;

        private bool _isViewModelActive;

        private readonly ITypeFactory _typeFactory;

        private readonly List<LogEntry> _logEntries = new List<LogEntry>();

        private bool _isClearingLog;
        private bool _isUpdatingTypes;

        private readonly object _lock = new object();
        #endregion

        #region Constructors
        public LogViewerViewModel(ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => typeFactory);

            _typeFactory = typeFactory;

            LogListenerType = typeof(LogViewerLogListener);
            ShowDebug = true;
            ShowInfo = true;
            ShowWarning = true;
            ShowError = true;

            TypeNames = new FastObservableCollection<string>() { defaultComboBoxItem };

            ResetEntriesCount();
        }
        #endregion

        #region Events
        public event EventHandler<LogMessageEventArgs> LogMessage;
        #endregion

        public void ClearEntries()
        {
            _isClearingLog = true;

            lock (_lock)
            {
                _logEntries.Clear();

                var typeNames = TypeNames;
                if (typeNames != null)
                {
                    _isUpdatingTypes = true;

                    using (typeNames.SuspendChangeNotifications())
                    {
                        typeNames.ReplaceRange(new[] {defaultComboBoxItem});
                    }

                    _isUpdatingTypes = false;
                }
            }

            ResetEntriesCount();

            _isClearingLog = false;
        }

        private void ResetEntriesCount()
        {
            DebugEntriesCount = 0;
            InfoEntriesCount = 0;
            WarningEntriesCount = 0;
            ErrorEntriesCount = 0;
        }

        #region Properties
        public List<LogEntry> LogEntries
        {
            get
            {
                lock (_lock)
                {
                    // Return a copy, don't mess with our internal list
                    return _logEntries.ToList();
                }
            }
        }

        public FastObservableCollection<string> TypeNames { get; private set; }

        public Type LogListenerType { get; set; }

        public string LogFilter { get; set; }
        public string TypeFilter { get; set; }

        public bool IgnoreCatelLogging { get; set; }
        public bool ShowDebug { get; set; }
        public bool ShowInfo { get; set; }
        public bool ShowWarning { get; set; }
        public bool ShowError { get; set; }

        public int DebugEntriesCount { get; private set; }
        public int InfoEntriesCount { get; private set; }
        public int WarningEntriesCount { get; private set; }
        public int ErrorEntriesCount { get; private set; }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();

            _isViewModelActive = true;

            SubscribeLogListener();
        }

        protected override async Task Close()
        {
            UnsubscribeLogListener();

            _isViewModelActive = false;

            await base.Close();
        }

        private void OnIgnoreCatelLoggingChanged()
        {
            // As an exception, we completely disable Catel on the log listener for performance
            _logListener.IgnoreCatelLogging = IgnoreCatelLogging;
        }

        private void OnLogListenerTypeChanged()
        {
            UnsubscribeLogListener();

            ClearEntries();

            SubscribeLogListener();
        }

        private void SubscribeLogListener()
        {
            if (!_isViewModelActive)
            {
                return;
            }

            var logListenerType = LogListenerType;
            if (logListenerType == null)
            {
                return;
            }

            _logListener = _typeFactory.CreateInstance(logListenerType) as ILogListener;
            if (_logListener != null)
            {
                _logListener.IgnoreCatelLogging = IgnoreCatelLogging;
                _logListener.LogMessage += OnLogMessage;

                LogManager.AddListener(_logListener);
            }
        }

        private void UnsubscribeLogListener()
        {
            if (_logListener == null)
            {
                return;
            }

            _logListener.LogMessage -= OnLogMessage;

            LogManager.RemoveListener(_logListener);

            _logListener = null;
        }

        public IEnumerable<LogEntry> GetFilteredLogEntries()
        {
            var entries = new List<LogEntry>();

            lock (_lock)
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
            if (!PassFilters(logEntry))
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

        private void UpdateEntriesCount(LogEntry logEvent)
        {
            switch (logEvent.LogEvent)
            {
                case LogEvent.Debug:
                    DebugEntriesCount++;
                    break;

                case LogEvent.Info:
                    InfoEntriesCount++;
                    break;

                case LogEvent.Warning:
                    WarningEntriesCount++;
                    break;

                case LogEvent.Error:
                    ErrorEntriesCount++;
                    break;
            }
        }

        private bool PassFilters(LogEntry logEntry)
        {
            return PassTypeFilter(logEntry) && PassLogFilter(logEntry);
        }

        private bool PassLogFilter(LogEntry logEntry)
        {
            if (string.IsNullOrEmpty(LogFilter) || LogFilter.Equals(defaultComboBoxItem))
            {
                return true;
            }

            var contains = logEntry.Message.IndexOf(LogFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            if (contains)
            {
                return true;
            }

            return false;
        }

        private bool PassTypeFilter(LogEntry logEntry)
        {
            var typeFilter = TypeFilter;
            if (string.IsNullOrEmpty(typeFilter) || typeFilter.Equals(defaultComboBoxItem))
            {
                return true;
            }

            var isOrigin = logEntry.Log.TargetType.Name.IndexOf(typeFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            if (isOrigin)
            {
                return true;
            }

            return false;
        }

        private void OnLogMessage(object sender, LogMessageEventArgs e)
        {
            if (_isClearingLog)
            {
                return;
            }

            var logEntry = new LogEntry(e);

            logEntry.Data["ThreadId"] = ThreadHelper.GetCurrentThreadId();

            lock (_lock)
            {
                _logEntries.Add(logEntry);

                if (!_isUpdatingTypes)
                {
                    var typeNames = TypeNames;
                    if (!typeNames.Contains(logEntry.Log.TargetType.Name))
                    {
                        _isUpdatingTypes = true;

                        try
                        {
                            typeNames.Add(logEntry.Log.TargetType.Name);
                        }
                        catch (System.Exception)
                        {
                            // we don't have time for this, let it go...
                        }

                        _isUpdatingTypes = false;
                    }
                }
            }

            UpdateEntriesCount(logEntry);

            LogMessage.SafeInvoke(this, e);
        }
        #endregion
    }
}