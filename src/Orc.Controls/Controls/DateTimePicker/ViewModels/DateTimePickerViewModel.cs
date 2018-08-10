// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.MVVM;

    public class DateTimePickerViewModel : ViewModelBase
    {
        #region Fields
        private bool _showOptionsButton;
        private bool _hideTime;
        private bool _hideSeconds;
        private DateTime? _value;
        private readonly DateTime _todayValue;
        #endregion

        #region Constructors
        public DateTimePickerViewModel()
        {
            var now = DateTime.Now;
            _todayValue = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        }
        #endregion

        #region Properties
        public bool ShowOptionsButton
        {
            get => _showOptionsButton;
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

        public bool HideTime
        {
            get => _hideTime;
            set
            {
                if (_hideTime == value)
                {
                    return;
                }

                _hideTime = value;

                RaisePropertyChanged(nameof(HideTime));
            }
        }

        public bool HideSeconds
        {
            get => _hideSeconds;
            set
            {
                if (_hideSeconds == value)
                {
                    return;
                }

                _hideSeconds = value;

                RaisePropertyChanged(nameof(HideSeconds));
            }
        }

        public DateTime? Value
        {
            get => _value;
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
            get => _value?.Year;
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
            get => _value?.Month;
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
            get => _value?.Day;
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
            get => _value?.Hour;
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
            get => _value?.Minute;
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
            get => _value?.Second;
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
            get => _value == null ? null : (_value.Value >= _value.Value.Date.AddHours(12)) ? Meridiems.LongPM : Meridiems.LongAM;
            set
            {
                if (!Meridiems.LongAM.Equals(value) && !Meridiems.LongPM.Equals(value))
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new DateTime(_todayValue.Year, _todayValue.Month, _todayValue.Day, _todayValue.Hour, _todayValue.Minute, _todayValue.Second);
                }
                else
                {
                    if (_value.Value.Hour >= 12 && Meridiems.LongPM.Equals(value)
                        || _value.Value.Hour < 12 && Meridiems.LongAM.Equals(value))
                    {
                        return;
                    }

                    var newValue = new DateTime(Year.Value, Month.Value, Day.Value, Hour.Value, Minute.Value, Second.Value);
                    newValue = newValue.AddHours(Meridiems.LongAM.Equals(value) ? -12 : 12);

                    Value = newValue;
                }
            }
        }
        #endregion
    }
}
