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
    using Extensions;

    public class DatePickerViewModel : ViewModelBase
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
                var accentColor = ((SolidColorBrush)AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary();
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

                Value = new DateTime(value, Month, Day);
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
                    Value = new DateTime(Year, value, Day);
                    return;
                }
                Day = daysInMonth;
                Value = new DateTime(Year, value, daysInMonth);
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

                Value = new DateTime(Year, Month, value);
            }
        }
        #endregion
    }
}