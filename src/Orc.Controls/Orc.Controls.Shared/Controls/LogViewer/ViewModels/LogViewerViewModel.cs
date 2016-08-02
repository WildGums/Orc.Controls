// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
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
        private readonly IDispatcherService _dispatcherService;
        private readonly LogViewerLogListener _logViewerLogListener;

        private readonly FastObservableCollection<LogEntry> _logEntries = new FastObservableCollection<LogEntry>();

        private bool _hasInitializedFirstLogListener;
        private bool _isClearingLog;

        private readonly object _lock = new object();
        #endregion

        #region Constructors
        public LogViewerViewModel(ITypeFactory typeFactory, IDispatcherService dispatcherService, LogViewerLogListener logViewerLogListener)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => logViewerLogListener);

            _typeFactory = typeFactory;
            _dispatcherService = dispatcherService;
            _logViewerLogListener = logViewerLogListener;

            LogListenerType = typeof(LogViewerLogListener);
            ShowDebug = true;
            ShowInfo = true;
            ShowWarning = true;
            ShowError = true;

            var typeNames = new FastObservableCollection<string>
            {
                defaultComboBoxItem
            };

            TypeNames = typeNames;

            ResetEntriesCount();
        }
        #endregion

        #region Events
        public event EventHandler<LogEntryEventArgs> LogMessage;
        #endregion

        public void ClearEntries()
        {
            _isClearingLog = true;

            // Note: we need to dispatch because the FastObservableCollection automatically dispatches (which is a good thing
            // when coming from a background thread). However... the ReplaceRange will be executed *outside* the lock
            // which is not good. So the lock is inside the dispatcher handler, and we manually dispatcher here.
            // Note: don't use BeginInvoke here because we need to wait until action will be processed
            _dispatcherService.Invoke(() =>
            {
                lock (_lock)
                {
                    using (_logEntries.SuspendChangeNotifications())
                    {
                        _logEntries.Clear();

                        var typeNames = TypeNames;
                        if (typeNames != null)
                        {
                            using (typeNames.SuspendChangeNotifications())
                            {
                                ((ICollection<string>)typeNames).ReplaceRange(new[] { defaultComboBoxItem });
                            }
                        }
                    }
                }

                ResetEntriesCount();

                _isClearingLog = false;
            }, false);
        }

        private void ResetEntriesCount()
        {
            DebugEntriesCount = 0;
            InfoEntriesCount = 0;
            WarningEntriesCount = 0;
            ErrorEntriesCount = 0;
        }

        #region Properties
        public ObservableCollection<LogEntry> LogEntries
        {
            get
            {
                lock (_lock)
                {
                    // We should return a copy, but for performance we return the original collection. Another
                    // solution would be to hold 2 collections in memory, but that goes bit too far
                    return _logEntries;
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
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _isViewModelActive = true;

            SubscribeLogListener();
        }

        protected override async Task CloseAsync()
        {
            UnsubscribeLogListener();

            _isViewModelActive = false;

            await base.CloseAsync();
        }

        private void OnIgnoreCatelLoggingChanged()
        {
            // As an exception, we completely disable Catel on the log listener for performance
            _logListener.IgnoreCatelLogging = IgnoreCatelLogging;
        }

        private void OnLogListenerTypeChanged()
        {
            UnsubscribeLogListener();

            if (_hasInitializedFirstLogListener)
            {
                ClearEntries();
            }

            _hasInitializedFirstLogListener = true;

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

            if (logListenerType == typeof(LogViewerLogListener))
            {
                _logListener = _logViewerLogListener;

                AddLogEntries(_logViewerLogListener.GetLogEntries(), true);
            }
            else
            {
                _logListener = _typeFactory.CreateInstance(logListenerType) as ILogListener;
                if (_logListener != null)
                {
                    LogManager.AddListener(_logListener);
                }
            }

            if (_logListener != null)
            {
                _logListener.IgnoreCatelLogging = IgnoreCatelLogging;
                _logListener.LogMessage += OnLogMessage;
            }
        }

        private void UnsubscribeLogListener()
        {
            if (_logListener == null)
            {
                return;
            }

            if (!(_logListener is LogViewerLogListener))
            {
                LogManager.RemoveListener(_logListener);
            }

            _logListener.LogMessage -= OnLogMessage;
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

        private void AddLogEntries(IEnumerable<LogEntry> entries, bool bypassClearingLog = false)
        {
            if (!bypassClearingLog && _isClearingLog)
            {
                return;
            }

            lock (_lock)
            {
                var typeNames = TypeNames;
                var requireSorting = false;

                LeanAndMeanModel = true;

                using (_logEntries.SuspendChangeNotifications())
                {
                    foreach (var entry in entries)
                    {
                        _logEntries.Add(entry);

                        if (!typeNames.Contains(entry.Log.TargetType.Name))
                        {
                            try
                            {
                                typeNames.Add(entry.Log.TargetType.Name);

                                requireSorting = true;
                            }
                            catch (Exception)
                            {
                                // we don't have time for this, let it go...
                            }
                        }

                        UpdateEntriesCount(entry);
                    }
                }

                if (requireSorting)
                {
                    using (typeNames.SuspendChangeNotifications())
                    {
                        typeNames.Sort();
                        typeNames.Remove(defaultComboBoxItem);
                        typeNames.Insert(0, defaultComboBoxItem);
                    }
                }

                LeanAndMeanModel = false;
                RaisePropertyChanged(string.Empty);
            }

            LogMessage.SafeInvoke(this, new LogEntryEventArgs(entries));
        }

        private void OnLogMessage(object sender, LogMessageEventArgs e)
        {
            var logEntry = new LogEntry(e);
            if (!logEntry.Data.ContainsKey("ThreadId"))
            {
                logEntry.Data["ThreadId"] = ThreadHelper.GetCurrentThreadId();
            }

            AddLogEntries(new[] { logEntry });
        }
        #endregion
    }
}