// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.MVVM;

    public class DatePickerViewModel : ViewModelBase
    {
        #region Fields
        private bool _showOptionsButton;
        private DateTime? _value;
        private readonly DateTime _todayValue;
        #endregion

        #region Constructors
        public DatePickerViewModel()
        {
            DateTime now = DateTime.Now;
            _todayValue = new DateTime(now.Year, now.Month, now.Day);
        }
        #endregion

        #region Properties
        public bool ShowOptionsButton
        {
            get { return _showOptionsButton; }
            set
            {
                if (_showOptionsButton == value)
                {
                    return;
                }

                _showOptionsButton = value;

                RaisePropertyChanged(nameof(ShowOptionsButton));
            }
        }

        public DateTime? Value
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

                // Note: For some reason we need to notify that Year, Month, Day properties changed.
                RaisePropertyChanged(nameof(Year));
                RaisePropertyChanged(nameof(Month));
                RaisePropertyChanged(nameof(Day));

                // Required for mappings
                RaisePropertyChanged(nameof(Value));
            }
        }

        public int? Year
        {
            get { return _value == null ? (int?)null : _value.Value.Year; }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new DateTime(value.Value, _todayValue.Month, _todayValue.Day);
                }
                else
                {
                    if (_value.Value.Year == value)
                    {
                        return;
                    }

                    var month = Month??1;

                    Value = new DateTime(value.Value, month, Day.Value);
                }
            }
        }

        public int? Month
        {
            get { return _value?.Month; }
            set
            {
                if (value == null)
                {
                    return;
                }

                var month = value.Value;
                if (month < 1)
                {
                    month = 1;
                }

                if (month > 12)
                {
                    month = 12;
                }

                if (_value == null)
                {
                    Value = new DateTime(_todayValue.Year, month, _todayValue.Day);
                }
                else
                {
                    if (_value.Value.Month == value)
                    {
                        return;
                    }

                    var year = Year ?? DateTime.Today.Year;
                    var daysInMonth = DateTime.DaysInMonth(year, month);

                    if (Day <= daysInMonth)
                    {
                        Value = new DateTime(year, month, Day.Value);
                        return;
                    }

                    Day = daysInMonth;
                    Value = new DateTime(year, month, daysInMonth);
                }
            }
        }

        public int? Day
        {
            get { return _value?.Day; }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new DateTime(_todayValue.Year, _todayValue.Month, value.Value);
                }
                else
                {
                    if (_value.Value.Day == value)
                    {
                        return;
                    }

                    var month = Month??DateTime.Today.Month;
                    var year = Year??DateTime.Today.Year;

                    Value = new DateTime(year, month, value.Value);
                }
            }
        }
        #endregion
    }
}
