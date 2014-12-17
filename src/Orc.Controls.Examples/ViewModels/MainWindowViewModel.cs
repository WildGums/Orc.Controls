// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using Catel.MVVM;

    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private TimeSpan _timeSpanValue;
        private DateTime _dateTimeValue;
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            TimeSpanValue = new TimeSpan(10, 11, 12, 13);
            DateTimeValue = DateTime.Now;
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
        #endregion
    }
}