namespace Orc.Controls.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;

    public class LogViewerViewModel : ViewModelBase
    {
        private readonly string _defaultComboBoxItem;

        private readonly IDispatcherService _dispatcherService;

        private readonly object _lock = new object();

        private readonly FastObservableCollection<LogEntry> _logEntries = new FastObservableCollection<LogEntry>();
        private readonly LogViewerLogListener _logViewerLogListener;
        private readonly ILanguageService _languageService;

        private readonly Queue<LogEntry> _queuedEntries = new Queue<LogEntry>();
#pragma warning disable IDISP006 // Implement IDisposable.
        private readonly Timer _timer;
#pragma warning restore IDISP006 // Implement IDisposable.

        private readonly ITypeFactory _typeFactory;

        private bool _hasInitializedFirstLogListener;
        private bool _isClearingLog;
        private bool _isViewModelActive;
        private ILogListener _logListener;

        public LogViewerViewModel(ITypeFactory typeFactory, IDispatcherService dispatcherService,
            LogViewerLogListener logViewerLogListener, ILanguageService languageService)
        {
            ArgumentNullException.ThrowIfNull(typeFactory);
            ArgumentNullException.ThrowIfNull(dispatcherService);
            ArgumentNullException.ThrowIfNull(logViewerLogListener);

            _typeFactory = typeFactory;
            _dispatcherService = dispatcherService;
            _logViewerLogListener = logViewerLogListener;
            _languageService = languageService;

            _defaultComboBoxItem = _languageService.GetRequiredString("Controls_LogViewer_SelectTypeName");
            _timer = new Timer(OnTimerTick);

            LogListenerType = typeof(LogViewerLogListener);
            ShowDebug = true;
            ShowInfo = true;
            ShowWarning = true;
            ShowError = true;

            var typeNames = new FastObservableCollection<string>
            {
                _defaultComboBoxItem
            };

            TypeNames = typeNames;

            ResetEntriesCount();
        }

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
        public bool AutoScroll { get; set; }
        public ScrollMode ScrollMode { get; set; }
        public bool ShowMultilineMessagesExpanded { get; set; }
        public int DebugEntriesCount { get; private set; }
        public int InfoEntriesCount { get; private set; }
        public int WarningEntriesCount { get; private set; }
        public int ErrorEntriesCount { get; private set; }
        public int MaximumUpdateBatchSize { get; set; }
        public LogFilterGroup ActiveFilterGroup { get; set; }

        public event EventHandler<LogEntryEventArgs> LogMessage;
        public event EventHandler<EventArgs> ActiveFilterGroupChanged;

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
                        if (typeNames is not null)
                        {
                            using (typeNames.SuspendChangeNotifications())
                            {
                                typeNames.ReplaceRange(new[] { _defaultComboBoxItem });
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

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _isViewModelActive = true;

            StartTimer();

            SubscribeLogListener();
        }

        protected override async Task CloseAsync()
        {
            UnsubscribeLogListener();

            StopTimer();

            _isViewModelActive = false;

            await base.CloseAsync();
        }

        private void OnActiveFilterGroupChanged()
        {
            lock (_lock)
            {
                ResetEntriesCount();
                UpdateEntriesCount(_logEntries.Where(IsValidLogEntry).ToList());
            }

            ActiveFilterGroupChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnTimerTick(object? state)
        {
            var entries = new List<LogEntry>();
            var maximumBatchSize = MaximumUpdateBatchSize;

            lock (_queuedEntries)
            {
                while (entries.Count < maximumBatchSize && _queuedEntries.Count > 0)
                {
                    entries.Add(_queuedEntries.Dequeue());
                }

                var additionalItems = 0;

                // If what we have left is a lot more than our maximum update batch size, automatically add more. The UI
                // will slow down, but we don't want to get behind too much
                //if (_queuedEntries.Count > (maximumBatchSize * 10))
                //{
                //    // We are under serious pressure, add 4 times the batch, otherwise the UI will start stuttering
                //    additionalItems = maximumBatchSize * 4;
                //}
                //else if (_queuedEntries.Count > (maximumBatchSize * 5))
                //{
                //    // We will add 3 additional batches
                //    additionalItems = maximumBatchSize * 3;
                //}
                //else if (_queuedEntries.Count > (maximumBatchSize * 3))
                //{
                //    // We will add 2 additional batches
                //    additionalItems = maximumBatchSize * 2;
                //}

                if (_queuedEntries.Count > (maximumBatchSize * 5))
                {
                    // We will add 2 additional batches
                    additionalItems = maximumBatchSize * 4;
                }

                while (additionalItems > 0 && _queuedEntries.Count > 0)
                {
                    entries.Add(_queuedEntries.Dequeue());
                    additionalItems--;
                }

                // Make sure to do this inside the lock so we can restart the timer as soon as we release this lock
                if (_queuedEntries.Count == 0)
                {
                    StopTimer();
                }
            }

            if (entries.Count > 0)
            {
                AddLogEntries(entries);
            }
        }

        private void OnIgnoreCatelLoggingChanged()
        {
            // As an exception, we completely disable Catel on the log listener for performance
            if (_logListener is not null)
            {
                _logListener.IgnoreCatelLogging = IgnoreCatelLogging;
            }
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
            if (logListenerType is null)
            {
                return;
            }

            if (logListenerType == typeof(LogViewerLogListener))
            {
                var logViewerLogListener = _logViewerLogListener;
                if (logViewerLogListener is not null)
                {
                    _logListener = logViewerLogListener;
                    _dispatcherService.Invoke(() => AddLogEntries(logViewerLogListener.GetLogEntries().ToList(), true));
                }
            }
            else
            {
                var logListener = _typeFactory.CreateInstance(logListenerType) as ILogListener;
                if (logListener is not null)
                {
                    LogManager.AddListener(logListener);
                    _logListener = logListener;
                }
            }

            if (_logListener is null)
            {
                return;
            }

            _logListener.IgnoreCatelLogging = IgnoreCatelLogging;
            _logListener.LogMessage += OnLogMessage;
        }

        private void UnsubscribeLogListener()
        {
            if (_logListener is null)
            {
                return;
            }

            if (_logListener is not LogViewerLogListener)
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
            return IsAcceptable(logEntry.LogEvent) && PassFilters(logEntry);
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

        private void UpdateEntriesCount(List<LogEntry> entries)
        {
            var matchedEntries = entries.Where(PassApplicationFilterGroupsConfiguration).ToList();

            DebugEntriesCount += matchedEntries.Count(x => x.LogEvent == LogEvent.Debug);
            InfoEntriesCount += matchedEntries.Count(x => x.LogEvent == LogEvent.Info);
            WarningEntriesCount += matchedEntries.Count(x => x.LogEvent == LogEvent.Warning);
            ErrorEntriesCount += matchedEntries.Count(x => x.LogEvent == LogEvent.Error);
        }

        private bool PassFilters(LogEntry logEntry)
        {
            if (!PassTypeFilter(logEntry))
            {
                return false;
            }

            return PassLogFilter(logEntry) && PassApplicationFilterGroupsConfiguration(logEntry);
        }

        private bool PassApplicationFilterGroupsConfiguration(LogEntry logEntry)
        {
            var filterGroup = ActiveFilterGroup;
            return filterGroup is null || filterGroup.Pass(logEntry);
        }

        private bool PassLogFilter(LogEntry logEntry)
        {
            if (string.IsNullOrEmpty(LogFilter) || LogFilter.Equals(_defaultComboBoxItem))
            {
                return true;
            }

            return logEntry.Message.IndexOf(LogFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool PassTypeFilter(LogEntry logEntry)
        {
            var typeFilter = TypeFilter;
            if (string.IsNullOrEmpty(typeFilter) || typeFilter.Equals(_defaultComboBoxItem))
            {
                return true;
            }

            return logEntry.Log.TargetType.Name.IndexOf(typeFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void AddLogEntries(List<LogEntry> entries, bool bypassClearingLog = false)
        {
            if (!bypassClearingLog && _isClearingLog)
            {
                return;
            }

            _dispatcherService.BeginInvoke(() =>
            {
                var filteredLogEntries = new List<LogEntry>();
                var typeNames = TypeNames;
                var requireSorting = false;

                lock (_lock)
                {
                    using (SuspendChangeNotifications())
                    {
                        using (_logEntries.SuspendChangeNotifications())
                        {
                            foreach (var entry in entries)
                            {
                                _logEntries.Add(entry);

                                if (IsValidLogEntry(entry))
                                {
                                    filteredLogEntries.Add(entry);
                                }

                                var targetTypeName = entry?.Log?.TargetType?.Name ?? string.Empty;
                                if (typeNames.Contains(targetTypeName))
                                {
                                    continue;
                                }

                                try
                                {
                                    typeNames.Add(targetTypeName);

                                    requireSorting = true;
                                }
                                catch (Exception)
                                {
                                    // we don't have time for this, let it go...
                                }
                            }
                        }
                    }
                }

                UpdateEntriesCount(entries);

                if (requireSorting)
                {
                    lock (_lock)
                    {
                        using (typeNames.SuspendChangeNotifications())
                        {
                            typeNames.Sort();
                            typeNames.Remove(_defaultComboBoxItem);
                            typeNames.Insert(0, _defaultComboBoxItem);
                        }
                    }
                }

                LogMessage?.Invoke(this, new LogEntryEventArgs(entries, filteredLogEntries));
            });
        }

        private void OnLogMessage(object? sender, LogMessageEventArgs e)
        {
            var logEntry = new LogEntry(e);
            if (!logEntry.Data.ContainsKey("ThreadId"))
            {
                logEntry.Data["ThreadId"] = ThreadHelper.GetCurrentThreadId();
            }

            lock (_queuedEntries)
            {
                _queuedEntries.Enqueue(logEntry);
            }

            StartTimer();
        }

        private void StartTimer()
        {
            if (!_isViewModelActive)
            {
                return;
            }

            //if (_timer.Interval > 0)
            //{
            //    return;
            //}

            var timeout = TimeSpan.FromMilliseconds(500);
            _timer.Change(timeout, timeout);
        }

        private void StopTimer()
        {
            _timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }
    }
}
