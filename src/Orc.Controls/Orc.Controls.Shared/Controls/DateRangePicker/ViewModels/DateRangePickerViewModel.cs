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
        private bool _isUpdatingRanges;
        private ObservableCollection<DateRange> _ranges;
        private DateRange _selectedRange;
        private DateTime _startDate;
        private DateTime _endDate;
        private TimeSpan _span;
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

                _isUpdatingRanges = true;

                _ranges = value;

                RaisePropertyChanged(nameof(Ranges));

                _isUpdatingRanges = false;
            }
        }

        public DateRange SelectedRange
        {
            get { return _selectedRange; }
            set
            {
                if (_isUpdatingRanges)
                {
                    // Skip update because combobox is resetting this value. Then we raise a property change event
                    // and the next update from the user will be ignored (since this will be a more recent change)
                    return;
                }

                if (UpdateSelectedRange(_selectedRange, value))
                {
                    OnSelectedRangeChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (UpdateStartDate(_startDate, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second)))
                {
                    OnStartDateChanged();
                }
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (UpdateEndDate(_endDate, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second)))
                {
                    OnEndDateChanged();
                }
            }
        }

        public TimeSpan Span
        {
            get { return _span; }
            set
            {
                if (UpdateSpan(_span, value))
                {
                    OnSpanChanged();
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
                var currentStartDate = _startDate;
                var currentEndDate = _endDate;
                var currentSpan = _span;
                var newStartDate = _selectedRange.Start;
                var newEndDate = _selectedRange.End;
                var newSpan = newEndDate.Subtract(newStartDate);

                UpdateStartDate(currentStartDate, newStartDate);
                UpdateEndDate(currentEndDate, newEndDate);
                UpdateSpan(currentSpan, newSpan);
            }
        }

        private void OnStartDateChanged()
        {
            RemoveTemporaryRanges();

            var currentSpan = _span;
            var newSpan = _endDate.Subtract(_startDate);

            UpdateSpan(currentSpan, newSpan);

            var currentSelectedDateRange = _selectedRange;
            var newSelectedDateRange = AddTemporaryRange();

            UpdateSelectedRange(currentSelectedDateRange, newSelectedDateRange);
        }

        private void OnEndDateChanged()
        {
            RemoveTemporaryRanges();

            var currentSpan = _span;
            var newSpan = _endDate.Subtract(_startDate);

            UpdateSpan(currentSpan, newSpan);

            var currentSelectedDateRange = _selectedRange;
            var newSelectedDateRange = AddTemporaryRange();

            UpdateSelectedRange(currentSelectedDateRange, newSelectedDateRange);
        }

        private void OnSpanChanged()
        {
            RemoveTemporaryRanges();

            var currentEndDate = _endDate;
            var newEndDate = _startDate.Add(_span);

            UpdateEndDate(currentEndDate, newEndDate);

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

        private bool UpdateStartDate(DateTime currentStartDate, DateTime newStartDate)
        {
            if (currentStartDate != newStartDate)
            {
                _startDate = newStartDate;
                RaisePropertyChanged(nameof(StartDate));

                return true;
            }

            return false;
        }

        private bool UpdateEndDate(DateTime currentEndDate, DateTime newEndDate)
        {
            if (currentEndDate != newEndDate)
            {
                _endDate = newEndDate;
                RaisePropertyChanged(nameof(EndDate));

                return true;
            }

            return false;
        }

        private bool UpdateSpan(TimeSpan currentSpan, TimeSpan newSpan)
        {
            if (currentSpan != newSpan)
            {
                _span = newSpan;
                RaisePropertyChanged(nameof(Span));

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
                    Start = _startDate,
                    End = _endDate,
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
