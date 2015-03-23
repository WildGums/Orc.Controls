// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM.Views;
    using Logging;
    using ViewModels;

    /// <summary>
    /// Interaction logic for LogViewerControl.xaml.
    /// </summary>
    public partial class LogViewerControl
    {
        #region Constants
        private static readonly Dictionary<LogEvent, Brush> ColorSets = new Dictionary<LogEvent, Brush>();
        #endregion

        #region Fields
        private LogViewerViewModel _lastKnownViewModel;
        #endregion

        #region Constructors
        static LogViewerControl()
        {
            typeof(LogViewerControl).AutoDetectViewPropertiesToSubscribe();
        }

        public LogViewerControl()
        {
            InitializeComponent();

            ColorSets[LogEvent.Debug] = Brushes.Gray;
            ColorSets[LogEvent.Info] = Brushes.Black;
            ColorSets[LogEvent.Warning] = Brushes.DarkOrange;
            ColorSets[LogEvent.Error] = Brushes.Red;

            Clear();
        }
        #endregion

        #region Properties
        public bool ShowTimestamp
        {
            get { return (bool)GetValue(ShowTimestampProperty); }
            set { SetValue(ShowTimestampProperty, value); }
        }

        public static readonly DependencyProperty ShowTimestampProperty = DependencyProperty.Register("ShowTimestamp", typeof(bool), 
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel]
        public string LogFilter
        {
            get { return (string)GetValue(LogFilterProperty); }
            set { SetValue(LogFilterProperty, value); }
        }

        public static readonly DependencyProperty LogFilterProperty = DependencyProperty.Register("LogFilter", typeof(string),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel]
        public Type LogListenerType
        {
            get { return (Type)GetValue(LogListenerTypeProperty); }
            set { SetValue(LogListenerTypeProperty, value); }
        }

        public static readonly DependencyProperty LogListenerTypeProperty = DependencyProperty.Register("LogListenerType", typeof(Type), 
            typeof(LogViewerControl), new PropertyMetadata(typeof(LogViewerLogListener)));


        [ViewToViewModel]
        public bool ShowDebug
        {
            get { return (bool)GetValue(ShowDebugProperty); }
            set { SetValue(ShowDebugProperty, value); }
        }

        public static readonly DependencyProperty ShowDebugProperty = DependencyProperty.Register("ShowDebug", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));


        [ViewToViewModel]
        public bool ShowInfo
        {
            get { return (bool)GetValue(ShowInfoProperty); }
            set { SetValue(ShowInfoProperty, value); }
        }

        public static readonly DependencyProperty ShowInfoProperty = DependencyProperty.Register("ShowInfo", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));


        [ViewToViewModel]
        public bool ShowWarning
        {
            get { return (bool)GetValue(ShowWarningProperty); }
            set { SetValue(ShowWarningProperty, value); }
        }

        public static readonly DependencyProperty ShowWarningProperty = DependencyProperty.Register("ShowWarning", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));


        [ViewToViewModel]
        public bool ShowError
        {
            get { return (bool)GetValue(ShowErrorProperty); }
            set { SetValue(ShowErrorProperty, value); }
        }

        public static readonly DependencyProperty ShowErrorProperty = DependencyProperty.Register("ShowError", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));
        #endregion

        #region Events
        public event EventHandler<LogEntryDoubleClickEventArgs> LogEntryDoubleClick;
        #endregion

        #region Methods
        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (_lastKnownViewModel != null)
            {
                _lastKnownViewModel.LogMessage -= OnViewModelLogMessage;
                _lastKnownViewModel = null;
            }

            _lastKnownViewModel = ViewModel as LogViewerViewModel;
            if (_lastKnownViewModel != null)
            {
                _lastKnownViewModel.LogMessage += OnViewModelLogMessage;
            }
        }

        private void OnViewModelLogMessage(object sender, LogMessageEventArgs e)
        {
            var vm = (LogViewerViewModel) sender;

            var logEntry = new LogEntry(e);
            if (vm.IsValidLogEntry(logEntry))
            {
                AddLogEntry(logEntry);

                ScrollToEnd();
            }
        }

        private void UpdateControl()
        {
            // Using BeginInvoke in order to call properties mapping first. Otherwise filtering by buttons doesen't work.
            // UpdateControl will be called *before* the properties mapping,
            // but because we call BeginInvoke, it will be placed at the end of the execution stack

            Dispatcher.BeginInvoke(new Action(() =>
            {
                Clear();

                var vm = ViewModel as LogViewerViewModel;
                if (vm != null)
                {
                    IEnumerable<LogEntry> logEntries = vm.GetFilteredLogEntries();
                    foreach (LogEntry logEntry in logEntries)
                    {
                        AddLogEntry(logEntry);
                    }
                }

                ScrollToEnd();
            }));
        }

        private void AddLogEntry(LogEntry logEntry)
        {
            var vm = ViewModel as LogViewerViewModel;
            if (vm == null)
            {
                return;
            }

            var paragraph = new RichTextBoxParagraph(logEntry);
            paragraph.MouseLeftButtonDown += (sender, args) =>
            {
                if (args.ClickCount == 2)
                {
                    LogEntryDoubleClick.SafeInvoke(this, new LogEntryDoubleClickEventArgs(logEntry));
                }
            };

            var timestamp = paragraph.LogEntry.Time.ToString();

            if (!ShowTimestamp)
            {
                timestamp = string.Empty;
            }

            var text = string.Format("{0} {1}", timestamp, paragraph.LogEntry.Message);
            paragraph.Inlines.Add(text);

            paragraph.Foreground = ColorSets[logEntry.LogEvent];
            LogRecordsRichTextBox.Document.Blocks.Add(paragraph);

        }

        private void ScrollToEnd()
        {
            // TODO: only scroll to end if user has not manually scrolled somewhere else, this code is already in Catel somewhere

            LogRecordsRichTextBox.ScrollToEnd();
        }

        public void Clear()
        {
            LogRecordsRichTextBox.Document.Blocks.Clear();
        }
        #endregion
    }
}