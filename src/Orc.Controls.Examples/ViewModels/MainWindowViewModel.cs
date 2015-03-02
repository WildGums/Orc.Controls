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
    using Catel.MVVM;

    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private TimeSpan _timeSpanValue;
        private DateTime _dateTimeValue;
        private List<KeyValuePair<string, string>> _filterSource;
        private string _filterText;
        private ObservableCollection<string> _logRecords;
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            TimeSpanValue = new TimeSpan(10, 11, 12, 13);
            DateTimeValue = DateTime.Now;
            FilterSource = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("1","abcd"),
                new KeyValuePair<string, string>("2","basdf"),
                new KeyValuePair<string, string>("3","bwerr"),
                new KeyValuePair<string, string>("4","oiydd"),
                new KeyValuePair<string, string>("5","klhhs"),
                new KeyValuePair<string, string>("6","sdfhi"),
            };
            LogRecords = new ObservableCollection<string>()
            {
                "log record 1",
                "log record 2",
                "log record 3",
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

        public ObservableCollection<string> LogRecords
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
        #endregion

        public Command AddLogRecords { get; set; }
        private void OnAddLogRecordsExecute()
        {
           LogRecords.Add(DateTime.Now.Ticks.ToString());
        }
    }
}