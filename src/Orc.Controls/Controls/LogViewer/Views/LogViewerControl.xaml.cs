// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
        private bool _hasClearedEntries;
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


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool AutoScroll
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }

        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.Register("AutoScroll", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowMultilineMessagesExpanded
        {
            get { return (bool)GetValue(ShowMultilineMessagesExpandedProperty); }
            set { SetValue(ShowMultilineMessagesExpandedProperty, value); }
        }

        public static readonly DependencyProperty ShowMultilineMessagesExpandedProperty = DependencyProperty.Register("ShowMultilineMessagesExpanded", typeof(bool),
            typeof(LogViewerControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((LogViewerControl)sender).UpdateControl()));


        public bool SupportCommandManager
        {
            get { return (bool)GetValue(SupportCommandManagerProperty); }
            set { SetValue(SupportCommandManagerProperty, value); }
        }

        public static readonly DependencyProperty SupportCommandManagerProperty = DependencyProperty.Register("SupportCommandManager", typeof(bool),
            typeof(LogViewerControl), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public int MaximumUpdateBatchSize
        {
            get { return (int)GetValue(MaximumUpdateBatchSizeProperty); }
            set { SetValue(MaximumUpdateBatchSizeProperty, value); }
        }

        public static readonly DependencyProperty MaximumUpdateBatchSizeProperty = DependencyProperty.Register(nameof(MaximumUpdateBatchSize), 
            typeof(int), typeof(LogViewerControl), new PropertyMetadata(250));
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

            ScrollToEnd();
        }

        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnViewModelPropertyChanged(e);

            if (e.HasPropertyChanged(nameof(LogViewerViewModel.LogListenerType)))
            {
                UpdateControl();
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == AutoScrollProperty)
            {
                ScrollToEnd();
            }
        }

        private void OnViewModelLogMessage(object sender, LogEntryEventArgs e)
        {
            UpdateControl(false, e.LogEntries);
        }

        private void UpdateControl(bool rebuild = true, List<LogEntry> logEntries = null)
        {
            // Using BeginInvoke in order to call properties mapping first. Otherwise filtering by buttons doesen't work.
            // UpdateControl will be called *before* the properties mapping,
            // but because we call BeginInvoke, it will be placed at the end of the execution stack
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (LogRecordsRichTextBox.Document == null)
                {
                    LogRecordsRichTextBox.Document = new FlowDocument
                    {
                        Tag = DateTime.MinValue
                    };
                }

                if (ViewModel is LogViewerViewModel vm)
                {
                    if (rebuild)
                    {
                        ClearScreen();

                        // Note: no need to get filtered ones, we will filter below
                        logEntries = vm.LogEntries.ToList();
                    }

                    if (logEntries != null)
                    {
                        var document = LogRecordsRichTextBox.Document;
                        foreach (var logEntry in logEntries)
                        {
                            if (!vm.IsValidLogEntry(logEntry))
                            {
                                continue;
                            }

                            var paragraph = CreateLogEntryParagraph(logEntry);
                            if (paragraph != null)
                            {
                                document.Blocks.Add(paragraph);
                                document.SetCurrentValue(FrameworkContentElement.TagProperty, logEntry.Time);
                            }
                        }

                        if (AutoScroll)
                        {
                            ScrollToEnd();
                        }
                    }
                }
            }));
        }

        protected override void OnLoaded(EventArgs e)
        {
            base.OnLoaded(e);

            ScrollToEnd();
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
                var icon = new Label
                {
                    DataContext = logEntry
                };

                paragraph.Inlines.Add(icon);
            }

            if (EnableTextColoring)
            {
                paragraph.Foreground = ColorSets[logEntry.LogEvent];
            }

            paragraph.SetData(EnableTimestamp, EnableThreadId, ShowMultilineMessagesExpanded);

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
            if (AutoScroll)
            {
                LogRecordsRichTextBox.ScrollToEnd();
            }
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
            _hasClearedEntries = true;

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
            Clipboard.SetDataObject(text);
        }

        public void ExpandAllMultilineLogMessages()
        {
            if (LogRecordsRichTextBox.Document != null)
            {
                foreach (var richTextBoxParagraph in LogRecordsRichTextBox.Document.Blocks.OfType<RichTextBoxParagraph>())
                {
                    richTextBoxParagraph.SetData(EnableTimestamp, EnableThreadId, true);
                }
            }
        }

        public void CollapseAllMultilineLogMessages()
        {
            if (LogRecordsRichTextBox.Document != null)
            {
                foreach (var richTextBoxParagraph in LogRecordsRichTextBox.Document.Blocks.OfType<RichTextBoxParagraph>())
                {
                    richTextBoxParagraph.SetData(this.EnableTimestamp, this.EnableThreadId);
                }
            }
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
                        var keyEventArgs = new KeyEventArgs(e.KeyboardDevice, PresentationSource.FromVisual(this), e.Timestamp, e.Key)
                        {
                            RoutedEvent = Keyboard.KeyDownEvent
                        };

                        RaiseEvent(keyEventArgs);
                        break;
                    }
                }
            }
        }

        private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_hasClearedEntries)
            {
                _hasClearedEntries = false;
                return;
            }

            // Disable auto scroll automatically if we are scrolling up
            if (e.VerticalChange < 0)
            {
#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
                SetValue(AutoScrollProperty, false);
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.
            }
        }
    }
}