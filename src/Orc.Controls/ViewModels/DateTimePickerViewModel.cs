// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows.Media;
    using Catel.MVVM;

    public class DateTimePickerViewModel : ViewModelBase
    {
        #region Fields
        private Brush _accentColorBrushProperty;
        private bool _showOptionsButton;
        private Brush _highlightColorBrush;
        private DateTime _value;
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

        public Brush AccentColorBrush
        {
            get { return _accentColorBrushProperty; }
            set
            {
                if (_accentColorBrushProperty == value)
                {
                    return;
                }

                _accentColorBrushProperty = value;
                HighlightColorBrush = GetHighlightBrush(_accentColorBrushProperty);

                RaisePropertyChanged("AccentColorBrush");
            }
        }

        public Brush HighlightColorBrush
        {
            get { return _highlightColorBrush; }
            set
            {
                if (_highlightColorBrush == value)
                {
                    return;
                }

                _highlightColorBrush = value;

                RaisePropertyChanged("HighlightColorBrush");
            }
        }

        public DateTime Value
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

                // Required for mappings
                RaisePropertyChanged("Value");
            }
        }

        public int Year
        {
            get { return _value.Year; }
            set
            {
                if (_value.Year == value)
                {
                    return;
                }

                Value = new DateTime(value, Month, Day, Hour, Minute, Second);
            }
        }

        public int Month
        {
            get { return _value.Month; }
            set
            {
                if (_value.Month == value)
                {
                    return;
                }
                var daysInMonth = DateTime.DaysInMonth(Year, value);
                if (Day <= daysInMonth)
                {
                    Value = new DateTime(Year, value, Day, Hour, Minute, Second);
                    return;
                }
                Day = daysInMonth;
                Value = new DateTime(Year, value, daysInMonth, Hour, Minute, Second);
            }
        }

        public int Day
        {
            get { return _value.Day; }
            set
            {
                if (_value.Day == value)
                {
                    return;
                }

                Value = new DateTime(Year, Month, value, Hour, Minute, Second);
            }
        }

        public int Hour
        {
            get { return _value.Hour; }
            set
            {
                if (_value.Hour == value)
                {
                    return;
                }

                Value = new DateTime(Year, Month, Day, value, Minute, Second);
            }
        }

        public int Minute
        {
            get { return _value.Minute; }
            set
            {
                if (_value.Minute == value)
                {
                    return;
                }

                Value = new DateTime(Year, Month, Day, Hour, value, Second);
            }
        }

        public int Second
        {
            get { return _value.Second; }
            set
            {
                if (_value.Second == value)
                {
                    return;
                }

                Value = new DateTime(Year, Month, Day, Hour, Minute, value);
            }
        }
        #endregion

        #region Methods
        private Brush GetHighlightBrush(Brush brush)
        {
            var color = ((SolidColorBrush) brush).Color;
            return new SolidColorBrush(Color.FromArgb(51, color.R, color.G, color.B));
        }
        #endregion
    }
}