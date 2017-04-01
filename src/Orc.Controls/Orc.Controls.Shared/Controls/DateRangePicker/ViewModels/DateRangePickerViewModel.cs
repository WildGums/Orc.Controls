// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateRangePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.ComponentModel;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using System.Linq;
    using System.Collections.ObjectModel;
    using Catel.Services;

    public class DateRangePickerViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<DateRange> _ranges;
        private DateRange _selectedRange;
        private DateTime _dateStart;
        private DateTime _dateEnd;
        private TimeSpan _value;
        #endregion

        #region Constructors
        public DateRangePickerViewModel()
        {

        }
        #endregion

        #region Properties
        public ObservableCollection<DateRange> Ranges
        {
            get { return _ranges; }
            set
            {
                if (_ranges == value)
                {
                    return;
                }

                _ranges = value;

                RaisePropertyChanged(nameof(Ranges));
            }
        }

        public DateRange SelectedRange
        {
            get { return _selectedRange; }
            set
            {
                if (UpdateSelectedRange(_selectedRange, value))
                {
                    OnSelectedRangeChanged();
                }
            }
        }

        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                if (UpdateDateStart(_dateStart, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second)))
                {
                    OnDateStartChanged();
                }
            }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set
            {
                if (UpdateDateEnd(_dateEnd, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second)))
                {
                    OnDateEndChanged();
                }
            }
        }

        public TimeSpan Value
        {
            get { return _value; }
            set
            {
                if (UpdateValue(_value, value))
                {
                    OnValueChanged();
                }
            }
        }
        #endregion

        #region Methods
        private void OnSelectedRangeChanged()
        {
            RemoveTemporaryRanges();

            if (_selectedRange != null)
            {
                var currentDateStart = _dateStart;
                var currentDateEnd = _dateEnd;
                var currentValue = _value;
                var newDateStart = _selectedRange.Start;
                var newDateEnd = _selectedRange.End;
                var newValue = newDateEnd.Subtract(newDateStart);

                UpdateDateStart(currentDateStart, newDateStart);
                UpdateDateEnd(currentDateEnd, newDateEnd);
                UpdateValue(currentValue, newValue);
            }
        }

        private void OnDateStartChanged()
        {
            RemoveTemporaryRanges();

            var currentValue = _value;
            var newValue = _dateEnd.Subtract(_dateStart);

            UpdateValue(currentValue, newValue);

            var currentSelectedDateRange = _selectedRange;
            var newSelectedDateRange = AddTemporaryRange();

            UpdateSelectedRange(currentSelectedDateRange, newSelectedDateRange);
        }

        private void OnDateEndChanged()
        {
            RemoveTemporaryRanges();

            var currentValue = _value;
            var newValue = _dateEnd.Subtract(_dateStart);

            UpdateValue(currentValue, newValue);

            var currentSelectedDateRange = _selectedRange;
            var newSelectedDateRange = AddTemporaryRange();

            UpdateSelectedRange(currentSelectedDateRange, newSelectedDateRange);
        }

        private void OnValueChanged()
        {
            RemoveTemporaryRanges();

            var currentDateEnd = _dateEnd;
            var newDateEnd = _dateStart.Add(_value);

            UpdateDateEnd(currentDateEnd, newDateEnd);

            var currentSelectedDateRange = _selectedRange;
            var newSelectedDateRange = AddTemporaryRange();

            UpdateSelectedRange(currentSelectedDateRange, newSelectedDateRange);
        }

        private bool UpdateSelectedRange(DateRange currentSelectedDateRange, DateRange newSelectedDateRange)
        {
            if (currentSelectedDateRange != newSelectedDateRange)
            {
                _selectedRange = newSelectedDateRange;
                RaisePropertyChanged(nameof(SelectedRange));

                return true;
            }

            return false;
        }

        private bool UpdateDateStart(DateTime currentDateStart, DateTime newDateStart)
        {
            if (currentDateStart != newDateStart)
            {
                _dateStart = newDateStart;
                RaisePropertyChanged(nameof(DateStart));

                return true;
            }

            return false;
        }

        private bool UpdateDateEnd(DateTime currentDateEnd, DateTime newDateEnd)
        {
            if (currentDateEnd != newDateEnd)
            {
                _dateEnd = newDateEnd;
                RaisePropertyChanged(nameof(DateEnd));

                return true;
            }

            return false;
        }

        private bool UpdateValue(TimeSpan currentValue, TimeSpan newValue)
        {
            if (currentValue != newValue)
            {
                _value = newValue;
                RaisePropertyChanged(nameof(Value));

                return true;
            }

            return false;
        }

        private DateRange AddTemporaryRange()
        {
            if (_ranges != null)
            {
                var temporaryRange = new DateRange()
                {
                    Name = LanguageHelper.GetString("Controls_DateRangePicker_Custom"),
                    Start = _dateStart,
                    End = _dateEnd,
                    IsTemporary = true
                };

                _ranges.Add(temporaryRange);

                return temporaryRange;
            }

            return null;
        }

        private void RemoveTemporaryRanges()
        {
            if (_ranges != null)
            {
                var oldCustomRanges = _ranges.Where(x => x.IsTemporary).ToList();
                foreach (var oldCustomRange in oldCustomRanges)
                {
                    _ranges.Remove(oldCustomRange);
                }
            }
        }
        #endregion
    }
}
