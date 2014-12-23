// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.MVVM;

    public class TimeSpanViewModel : ViewModelBase
    {
        #region Fields
        private TimeSpan _value;
        #endregion

        #region Properties
        public TimeSpan Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;

                // Raise all property changed
                RaisePropertyChanged(string.Empty);
            }
        }

        public int Days
        {
            get { return _value.Days; }
            set
            {
                if (_value.Days == value)
                {
                    return;
                }

                Value = new TimeSpan(value, Hours, Minutes, Seconds);
            }
        }

        public int Hours
        {
            get { return _value.Hours; }
            set
            {
                if (_value.Hours == value)
                {
                    return;
                }

                Value = new TimeSpan(Days, value, Minutes, Seconds);
            }
        }

        public int Minutes
        {
            get { return _value.Minutes; }
            set
            {
                if (_value.Minutes == value)
                {
                    return;
                }

                Value = new TimeSpan(Days, Hours, value, Seconds);
            }
        }

        public int Seconds
        {
            get { return _value.Seconds; }
            set
            {
                if (_value.Seconds == value)
                {
                    return;
                }

                Value = new TimeSpan(Days, Hours, Minutes, value);
            }
        }
        #endregion
    }
}