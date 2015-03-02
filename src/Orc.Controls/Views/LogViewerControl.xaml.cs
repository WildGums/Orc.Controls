// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Net.Mime;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Catel.Logging;
    using Catel.MVVM.Views;
    using Examples.Models;

    /// <summary>
    /// Interaction logic for LogViewerControl.xaml.
    /// </summary>
    public partial class LogViewerControl
    {
        #region Constants
        private static readonly Dictionary<LogEvent, Brush> ColorSets = new Dictionary<LogEvent, Brush>();
        #endregion

        #region Fields
        private INotifyCollectionChanged _previousLogRecords = null;
        #endregion

        #region Constructors
        public LogViewerControl()
        {
            InitializeComponent();

            ColorSets[LogEvent.Debug] = Brushes.Gray;
            ColorSets[LogEvent.Info] = Brushes.Black;
            ColorSets[LogEvent.Warning] = Brushes.DarkOrange;
            ColorSets[LogEvent.Error] = Brushes.Red;

            Clear();

            DependencyPropertyDescriptor
                .FromProperty(LogRecordsProperty, typeof (LogViewerControl))
                .AddValueChanged(this, (sender, args) =>
                {
                    UnSubscribeToDataContext();
                    UpdateControl();
                    SubscribeToDataContext();
                });
        }
        #endregion

        #region Methods
        private void UnSubscribeToDataContext()
        {
            if (_previousLogRecords != null)
            {
                _previousLogRecords.CollectionChanged -= LogRecordsOnCollectionChanged;
                _previousLogRecords = null;
            }
        }

        private void SubscribeToDataContext()
        {
            if (LogRecords is INotifyCollectionChanged)
            {
                _previousLogRecords = LogRecords as INotifyCollectionChanged;
                _previousLogRecords.CollectionChanged += LogRecordsOnCollectionChanged;
            }
        }

        private void LogRecordsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            //Todo: Do not remove existing items, only add new rows.
            Clear();
            UpdateControl();
        }

        private void UpdateControl()
        {
            foreach (var logRecord in LogRecords)
            {
                var paragraph = new Paragraph();

                var record = (LogRecord) logRecord;
                var text = string.Format("{0} {1}", record.DateTime, record.Message);

                paragraph.Inlines.Add(text);
                paragraph.Foreground = ColorSets[record.LogEvent];
                textBox.Document.Blocks.Add(paragraph);
            }
            textBox.ScrollToEnd();
        }

        public void Clear()
        {
            textBox.Document.Blocks.Clear();
        }
        #endregion

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public IEnumerable LogRecords
        {
            get { return (IEnumerable)GetValue(LogRecordsProperty); }
            set { SetValue(LogRecordsProperty, value); }
        }

        public static readonly DependencyProperty LogRecordsProperty = DependencyProperty.Register("LogRecords", typeof(IEnumerable), typeof(LogViewerControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}