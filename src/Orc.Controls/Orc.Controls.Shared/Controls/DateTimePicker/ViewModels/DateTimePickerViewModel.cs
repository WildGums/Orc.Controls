// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Catel.Data;
    using Catel.MVVM;

    public class DateTimePickerViewModel : ViewModelBase
    {
        #region Fields
        private bool _showOptionsButton;
        private bool _hideSeconds;
        private DateTime? _value;
        private DateTime _todayValue;
        #endregion

        #region Constructors
        public DateTimePickerViewModel()
        {
            DateTime now = DateTime.Now;
            _todayValue = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
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

                RaisePropertyChanged("ShowOptionsButton");
            }
        }

        public bool HideSeconds
        {
            get { return _hideSeconds; }
            set
            {
                if (_hideSeconds == value)
                {
                    return;
                }

                _hideSeconds = value;

                RaisePropertyChanged("HideSeconds");
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

                // Note: For some reason we need to notify that Year, Month, Day, Hour, Minute, Second, AmPm properties changed.
                RaisePropertyChanged(nameof(Year));
                RaisePropertyChanged(nameof(Month));
                RaisePropertyChanged(nameof(Day));
                RaisePropertyChanged(nameof(Hour));
                RaisePropertyChanged(nameof(Minute));
                RaisePropertyChanged(nameof(Second));
                RaisePropertyChanged(nameof(AmPm));

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
                    Value = new DateTime(value.Value, _todayValue.Month, _todayValue.Day, _todayValue.Hour, _todayValue.Minute, _todayValue.Second);
                }
                else
                {
                    if (_value.Value.Year == value)
                    {
                        return;
                    }

                    Value = new DateTime(value.Value, Month.Value, Day.Value, Hour.Value, Minute.Value, Second.Value);
                }
            }
        }

        public int? Month
        {
            get { return _value == null ? (int?)null : _value.Value.Month; }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new DateTime(_todayValue.Year, value.Value, _todayValue.Day, _todayValue.Hour, _todayValue.Minute, _todayValue.Second);
                }
                else
                {
                    if (_value.Value.Month == value)
                    {
                        return;
                    }
                    var daysInMonth = DateTime.DaysInMonth(Year.Value, value.Value);
                    if (Day <= daysInMonth)
                    {
                        Value = new DateTime(Year.Value, value.Value, Day.Value, Hour.Value, Minute.Value, Second.Value);
                        return;
                    }
                    Day = daysInMonth;
                    Value = new DateTime(Year.Value, value.Value, daysInMonth, Hour.Value, Minute.Value, Second.Value);
                }
            }
        }

        public int? Day
        {
            get { return _value == null ? (int?)null : _value.Value.Day; }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new DateTime(_todayValue.Year, _todayValue.Month, value.Value, _todayValue.Hour, _todayValue.Minute, _todayValue.Second);
                }
                else
                {
                    if (_value.Value.Day == value)
                    {
                        return;
                    }

                    Value = new DateTime(Year.Value, Month.Value, value.Value, Hour.Value, Minute.Value, Second.Value);
                }
            }
        }

        public int? Hour
        {
            get { return _value == null ? (int?)null : _value.Value.Hour; }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new DateTime(_todayValue.Year, _todayValue.Month, _todayValue.Day, value.Value, _todayValue.Minute, _todayValue.Second);
                }
                else
                {
                    if (_value.Value.Hour == value)
                    {
                        return;
                    }

                    Value = new DateTime(Year.Value, Month.Value, Day.Value, value.Value, Minute.Value, Second.Value);
                }
            }
        }

        public int? Minute
        {
            get { return _value == null ? (int?)null : _value.Value.Minute; }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new DateTime(_todayValue.Year, _todayValue.Month, _todayValue.Day, _todayValue.Hour, value.Value, _todayValue.Second);
                }
                else
                {
                    if (_value.Value.Minute == value)
                    {
                        return;
                    }

                    Value = new DateTime(Year.Value, Month.Value, Day.Value, Hour.Value, value.Value, Second.Value);
                }
            }
        }

        public int? Second
        {
            get { return _value == null ? (int?)null : _value.Value.Second; }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new DateTime(_todayValue.Year, _todayValue.Month, _todayValue.Day, _todayValue.Hour, _todayValue.Minute, value.Value);
                }
                else
                {
                    if (_value.Value.Second == value)
                    {
                        return;
                    }

                    Value = new DateTime(Year.Value, Month.Value, Day.Value, Hour.Value, Minute.Value, value.Value);
                }
            }
        }

        public string AmPm
        {
            get;
            private set;
        }
        #endregion

        #region Properties
        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.HasPropertyChanged("Value"))
            {
                var newValue = e.NewValue as DateTime?;
                AmPm = newValue == null ? null : newValue.Value.ToString("tt", CultureInfo.InvariantCulture);
            }
        }
        #endregion
    }
}