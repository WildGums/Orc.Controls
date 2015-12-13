// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Windows.Input;
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
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICommandManager _commandManager;

        private LogViewerViewModel _lastKnownViewModel;
        #endregion

        #region Events
        public event EventHandler<LogEntryDoubleClickEventArgs> LogEntryDoubleClick;
        #endregion

        #region Constructors
        static LogViewerControl()
        {
            typeof(LogViewerControl).AutoDetectViewPropertiesToSubscribe();
        }

        public LogViewerControl()
        {
            InitializeComponent();

            _commandManager = ServiceLocator.Default.ResolveType<ICommandManager>();

            ColorSets[LogEvent.Debug] = Brushes.Gray;
            ColorSets[LogEvent.Info] = Brushes.Black;
            ColorSets[LogEvent.Warning] = Brushes.DarkOrange;
            ColorSets[LogEvent.Error] = Brushes.Red;
        }
        #endregion

        #region Properties
        public bool EnableTimestamp
        {
            get { return (bool)GetValue(EnableTimestampProperty); }
            set { SetValue(EnableTimestampProperty, value); }
        }

        public static readonly DependencyProperty EnableTimestampProperty = DependencyProperty.Register("EnableTimestamp", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        public bool EnableIcons
        {
            get { return (bool)GetValue(EnableIconsProperty); }
            set { SetValue(EnableIconsProperty, value); }
        }

        public static readonly DependencyProperty EnableIconsProperty = DependencyProperty.Register("EnableIcons", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));


        public bool EnableThreadId
        {
            get { return (bool)GetValue(EnableThreadIdProperty); }
            set { SetValue(EnableThreadIdProperty, value); }
        }

        public static readonly DependencyProperty EnableThreadIdProperty = DependencyProperty.Register("EnableThreadId", typeof(bool), typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));


        public bool EnableTextColoring
        {
            get { return (bool)GetValue(EnableTextColoringProperty); }
            set { SetValue(EnableTextColoringProperty, value); }
        }

        public static readonly DependencyProperty EnableTextColoringProperty = DependencyProperty.Register("EnableTextColoring", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string LogFilter
        {
            get { return (string)GetValue(LogFilterProperty); }
            set { SetValue(LogFilterProperty, value); }
        }

        public static readonly DependencyProperty LogFilterProperty = DependencyProperty.Register("LogFilter", typeof(string),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string TypeFilter
        {
            get { return (string)GetValue(TypeFilterProperty); }
            set { SetValue(TypeFilterProperty, value); }
        }

        public static readonly DependencyProperty TypeFilterProperty = DependencyProperty.Register("TypeFilter", typeof(string),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public Type LogListenerType
        {
            get { return (Type)GetValue(LogListenerTypeProperty); }
            set { SetValue(LogListenerTypeProperty, value); }
        }

        public static readonly DependencyProperty LogListenerTypeProperty = DependencyProperty.Register("LogListenerType", typeof(Type),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(typeof(LogViewerLogListener), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IgnoreCatelLogging
        {
            get { return (bool)GetValue(IgnoreCatelLoggingProperty); }
            set { SetValue(IgnoreCatelLoggingProperty, value); }
        }

        public static readonly DependencyProperty IgnoreCatelLoggingProperty = DependencyProperty.Register("IgnoreCatelLogging", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowDebug
        {
            get { return (bool)GetValue(ShowDebugProperty); }
            set { SetValue(ShowDebugProperty, value); }
        }

        public static readonly DependencyProperty ShowDebugProperty = DependencyProperty.Register("ShowDebug", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowInfo
        {
            get { return (bool)GetValue(ShowInfoProperty); }
            set { SetValue(ShowInfoProperty, value); }
        }

        public static readonly DependencyProperty ShowInfoProperty = DependencyProperty.Register("ShowInfo", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowWarning
        {
            get { return (bool)GetValue(ShowWarningProperty); }
            set { SetValue(ShowWarningProperty, value); }
        }

        public static readonly DependencyProperty ShowWarningProperty = DependencyProperty.Register("ShowWarning", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowError
        {
            get { return (bool)GetValue(ShowErrorProperty); }
            set { SetValue(ShowErrorProperty, value); }
        }

        public static readonly DependencyProperty ShowErrorProperty = DependencyProperty.Register("ShowError", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((LogViewerControl)sender).UpdateControl()));

        public bool SupportCommandManager
        {
            get { return (bool)GetValue(SupportCommandManagerProperty); }
            set { SetValue(SupportCommandManagerProperty, value); }
        }

        public static readonly DependencyProperty SupportCommandManagerProperty = DependencyProperty.Register("SupportCommandManager", typeof(bool),
            typeof(LogViewerControl), new PropertyMetadata(true));
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

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnViewModelPropertyChanged(e);

            if (string.Equals(e.PropertyName, "LogListenerType"))
            {
                UpdateControl();
            }
        }

        private void OnViewModelLogMessage(object sender, LogEntryEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var vm = (LogViewerViewModel)sender;

                var logEntries = e.LogEntries;
                foreach (var logEntry in logEntries)
                {
                    if (vm.IsValidLogEntry(logEntry))
                    {
                        var document = LogRecordsRichTextBox.Document;
                        var lastLogMessage = (DateTime) document.Tag;

                        if (logEntry.Time < lastLogMessage)
                        {
                            // Always ignore, old message
                            return;
                        }
                        else if (logEntry.Time == lastLogMessage)
                        {
                            // This looks like hurting for performance, but there aren't many messages on exactly the same time
                            for (int i = document.Blocks.Count - 1; i >= 0; i--)
                            {
                                var existingParagraph = document.Blocks.ElementAt(i) as RichTextBoxParagraph;
                                if (existingParagraph != null)
                                {
                                    var paragraphLogEntry = existingParagraph.LogEntry;
                                    if (paragraphLogEntry.Time < logEntry.Time)
                                    {
                                        // We hit an older one, the message is not yet added
                                        break;
                                    }

                                    if (ReferenceEquals(paragraphLogEntry, logEntry))
                                    {
                                        // Already added, ignore
                                        return;
                                    }
                                }
                            }
                        }

                        var paragraph = CreateLogEntryParagraph(logEntry);
                        if (paragraph != null)
                        {
                            document.Blocks.Add(paragraph);
                            document.Tag = logEntry.Time;
                        }
                    }
                }

                ScrollToEnd();
            }));
        }

        private void UpdateControl()
        {
            // Using BeginInvoke in order to call properties mapping first. Otherwise filtering by buttons doesen't work.
            // UpdateControl will be called *before* the properties mapping,
            // but because we call BeginInvoke, it will be placed at the end of the execution stack

            Dispatcher.BeginInvoke(new Action(() =>
            {
                ClearScreen();

                var vm = ViewModel as LogViewerViewModel;
                if (vm != null)
                {
                    var document = new FlowDocument
                    {
                        Tag = DateTime.MinValue
                    };

                    var logEntries = vm.GetFilteredLogEntries();
                    foreach (LogEntry logEntry in logEntries)
                    {
                        var paragraph = CreateLogEntryParagraph(logEntry);
                        if (paragraph != null)
                        {
                            document.Blocks.Add(paragraph);
                            document.Tag = logEntry.Time;
                        }
                    }

                    LogRecordsRichTextBox.Document = document;
                }

                ScrollToEnd();
            }));
        }

        private RichTextBoxParagraph CreateLogEntryParagraph(LogEntry logEntry)
        {
            var vm = ViewModel as LogViewerViewModel;
            if (vm == null)
            {
                return null;
            }

            var paragraph = new RichTextBoxParagraph(logEntry);
            paragraph.MouseLeftButtonDown += OnParagraphMouseLeftButton;

            if (EnableIcons)
            {
                var icon = new Label() { DataContext = logEntry };
                paragraph.Inlines.Add(icon);
            }

            if (EnableTextColoring)
            {
                paragraph.Foreground = ColorSets[logEntry.LogEvent];
            }

            paragraph.SetData(EnableTimestamp, EnableThreadId);

            return paragraph;
        }

        private void OnParagraphMouseLeftButton(object sender, MouseButtonEventArgs e)
        {
            var paragraph = sender as RichTextBoxParagraph;
            if (paragraph == null)
            {
                return;
            }

            var logEntry = paragraph.LogEntry;
            if (logEntry == null)
            {
                return;
            }

            if (e.ClickCount == 2)
            {
                LogEntryDoubleClick.SafeInvoke(this, new LogEntryDoubleClickEventArgs(logEntry));
            }
        }

        private void ScrollToEnd()
        {
            // TODO: only scroll to end if user has not manually scrolled somewhere else, this code is already in Catel somewhere

            LogRecordsRichTextBox.ScrollToEnd();
        }

        private void ClearScreen()
        {
            var oldDoc = LogRecordsRichTextBox.Document;

            LogRecordsRichTextBox.Document = new FlowDocument
            {
                Tag = DateTime.MinValue
            };

            // TODO: Consider doing in a background thread
            if (oldDoc != null)
            {
                foreach (var block in oldDoc.Blocks)
                {
                    var paragraph = block as RichTextBoxParagraph;
                    if (paragraph != null)
                    {
                        paragraph.MouseLeftButtonDown -= OnParagraphMouseLeftButton;
                    }
                }

                oldDoc.Blocks.Clear();
            }
        }

        public void Clear()
        {
            var vm = ViewModel as LogViewerViewModel;
            if (vm != null)
            {
                vm.ClearEntries();
            }

            ClearScreen();
        }

        public void CopyToClipboard()
        {
            var text = LogRecordsRichTextBox.GetInlineText();
            Clipboard.SetText(text);
        }
        #endregion

        private void LogRecordsRichTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            // Some key down events are not fired by the RichTextBox, others are. To create a consistent
            // behavior, we don't forward any key downs on the RTB. If a command from the ICommandManager
            // is used, the PreviewKeyDown event will re-raise that event so the ICommandManager can
            // respond to it
            if (SupportCommandManager)
            {
                e.Handled = true;
            }
        }

        private void LogRecordsRichTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!SupportCommandManager)
            {
                return;
            }

            if (_commandManager == null)
            {
                return;
            }

            // This is required to support application-wide commands on the log viewer control, somehow the
            // RichTextBox does not fire KeyDown events for combinations of keys (CTRL + [Key])
            var commandNames = _commandManager.GetCommands();
            foreach (var commandName in commandNames)
            {
                var inputGesture = _commandManager.GetInputGesture(commandName);
                if (inputGesture != null)
                {
                    if (inputGesture.Matches(e))
                    {
                        var keyEventArgs = new KeyEventArgs(e.KeyboardDevice,
                            PresentationSource.FromVisual(this), e.Timestamp, e.Key)
                        {
                            RoutedEvent = Keyboard.KeyDownEvent
                        };

                        RaiseEvent(keyEventArgs);
                        break;
                    }
                }
            }
        }
    }
}