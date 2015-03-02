// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Documents;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for LogViewerControl.xaml.
    /// </summary>
    public partial class LogViewerControl
    {
        #region Fields
        private INotifyCollectionChanged _previousLogRecords = null;
        #endregion

        #region Constructors
        public LogViewerControl()
        {
            InitializeComponent();
            textBox.Document.Blocks.Clear();

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
            textBox.Document.Blocks.Clear();
            UpdateControl();
        }

        private void UpdateControl()
        {
            foreach (var logRecord in LogRecords)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(logRecord as string);
                textBox.Document.Blocks.Add(paragraph);
            }
            textBox.ScrollToEnd();
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