// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanPickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.MVVM;

    public class TimeSpanPickerViewModel : ViewModelBase
    {
        #region Fields
        private TimeSpan? _value;
        #endregion

        #region Properties
        public TimeSpan? Value
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

                // Note: For some reason we need to notify that Days, Hours, Minutes, Seconds properties changed.
                RaisePropertyChanged(nameof(Days));
                RaisePropertyChanged(nameof(Hours));
                RaisePropertyChanged(nameof(Minutes));
                RaisePropertyChanged(nameof(Seconds));

                // Required for mappings
                RaisePropertyChanged(nameof(Value));
            }
        }

        public int? Days
        {
            get => _value?.Days;
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new TimeSpan(value.Value, 0, 0, 0);
                }
                else
                {
                    if (_value.Value.Days == value)
                    {
                        return;
                    }

                    Value = new TimeSpan(value.Value, Hours.Value, Minutes.Value, Seconds.Value);
                }
            }
        }

        public int? Hours
        {
            get => _value?.Hours;
            set
            {
                if (value == null)
                {
                    return;
                }

                if (_value == null)
                {
                    Value = new TimeSpan(0, value.Value, 0, 0);
                }
                else
                {
                    if (_value.Value.Hours == value)
                    {
                        return;
                    }

                    Value = new TimeSpan(Days.Value, value.Value, Minutes.Value, Seconds.Value);
                }
            }
        }

        public int? Minutes
        {
            get => _value?.Minutes;
            set
            {
                if (_value == null)
                {
                    Value = new TimeSpan(0, 0, value.Value, 0);
                }
                else
                {
                    if (_value.Value.Minutes == value)
                    {
                        return;
                    }

                    Value = new TimeSpan(Days.Value, Hours.Value, value.Value, Seconds.Value);
                }
            }
        }

        public int? Seconds
        {
            get => _value?.Seconds;
            set
            {
                if (_value == null)
                {
                    Value = new TimeSpan(0, 0, 0, value.Value);
                }
                else
                {
                    if (_value.Value.Seconds == value)
                    {
                        return;
                    }

                    Value = new TimeSpan(Days.Value, Hours.Value, Minutes.Value, value.Value);
                }
            }
        }
        #endregion
    }
}
