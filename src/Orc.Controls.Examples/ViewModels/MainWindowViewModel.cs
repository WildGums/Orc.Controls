// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Catel.Logging;
    using Catel.MVVM;
    using Models;
    using System.Collections.Generic;
    using System.Windows.Media;
    using Catel.MVVM;

    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private TimeSpan _timeSpanValue;
        private DateTime _dateTimeValue;
        private List<KeyValuePair<string, string>> _filterSource;
        private string _filterText;
        private Brush _accentColorBrush;
        private ObservableCollection<LogRecord> _logRecords;
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            TimeSpanValue = new TimeSpan(10, 11, 12, 13);
            DateTimeValue = DateTime.Now;
            AccentColorBrush = Brushes.Red;

            FilterSource = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("1", "abcd"),
                new KeyValuePair<string, string>("2", "basdf"),
                new KeyValuePair<string, string>("3", "bwerr"),
                new KeyValuePair<string, string>("4", "oiydd"),
                new KeyValuePair<string, string>("5", "klhhs"),
                new KeyValuePair<string, string>("6", "sdfhi"),
            };
            LogRecords = new ObservableCollection<LogRecord>()
            {
                new LogRecord(){DateTime = DateTime.Now, LogEvent = LogEvent.Debug, Message = "log record 1"},
                new LogRecord(){DateTime = DateTime.Now, LogEvent = LogEvent.Error, Message = "log record 2"},
                new LogRecord(){DateTime = DateTime.Now, LogEvent = LogEvent.Info, Message = "log record 3"},
                new LogRecord(){DateTime = DateTime.Now, LogEvent = LogEvent.Warning, Message = "log record 4"},
            };
            AddLogRecords = new Command(OnAddLogRecordsExecute);
        }
        #endregion

        #region Properties
        public TimeSpan TimeSpanValue
        {
            get { return _timeSpanValue; }
            set
            {
                if (_timeSpanValue == value)
                {
                    return;
                }
                _timeSpanValue = value;
                RaisePropertyChanged(() => TimeSpanValue);
            }
        }

        public DateTime DateTimeValue
        {
            get { return _dateTimeValue; }
            set
            {
                if (_dateTimeValue == value)
                {
                    return;
                }
                _dateTimeValue = value;
                RaisePropertyChanged(() => DateTimeValue);
            }
        }

        public Brush AccentColorBrush
        {
            get { return _accentColorBrush; }
            set
            {
                if (_accentColorBrush == value)
                {
                    return;
                }
                _accentColorBrush = value;
                RaisePropertyChanged(() => AccentColorBrush);
            }
        }

        public List<KeyValuePair<string, string>> FilterSource
        {
            get { return _filterSource; }
            set
            {
                if (_filterSource == value)
                {
                    return;
                }
                _filterSource = value;
                RaisePropertyChanged(() => FilterSource);
            }
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (_filterText == value)
                {
                    return;
                }
                _filterText = value;
                RaisePropertyChanged(() => FilterText);
            }
        }

        public ObservableCollection<LogRecord> LogRecords
        {
            get { return _logRecords; }
            set
            {
                if (_logRecords == value)
                {
                    return;
                }
                _logRecords = value;
                RaisePropertyChanged(() => LogRecords);
            }
        }

        #endregion

        public Command AddLogRecords { get; set; }
        private void OnAddLogRecordsExecute()
        {
           LogRecords.Add(new LogRecord()
           {
               DateTime = DateTime.Now, LogEvent = LogEvent.Warning, Message = "log record 4"
           });
        }
    }
}