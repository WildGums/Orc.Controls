namespace Orc.Controls;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Catel;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;

public class DateRangePickerViewModel : ViewModelBase
{
    private ObservableCollection<DateRange>? _ranges;
    private DateRange? _selectedRange;
    private TimeSpan _span;
    private DateTime _startDate;
    private DateTime _endDate;
    private bool _isUpdatingRanges;
    private readonly ILanguageService _languageService;

    public DateRangePickerViewModel(ILanguageService languageService)
    {
        _languageService = languageService;

        ValidateUsingDataAnnotations = false;
    }

    public TimeAdjustmentStrategy TimeAdjustmentStrategy { get; set; }

    public ObservableCollection<DateRange>? Ranges
    {
        get => _ranges;
        set
        {
            if (_ranges == value)
            {
                return;
            }

            using (StartUpdateRanges())
            {
                _ranges = value;

                RaisePropertyChanged(nameof(Ranges));
            }
        }
    }

    public DateRange? SelectedRange
    {
        get => _selectedRange;
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
        get => _startDate;
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
        get => _endDate;
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
        get => _span;
        set
        {
            if (UpdateSpan(_span, value))
            {
                OnSpanChanged();
            }
        }
    }

    protected override void OnValidating(IValidationContext validationContext)
    {
        if (EndDate < StartDate)
        {
            validationContext.Add(FieldValidationResult.CreateError(nameof(EndDate),
                _languageService.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_EndDate_NotLess_StartDate_Validation))));
        }

        if (Span < TimeSpan.Zero)
        {
            validationContext.Add(FieldValidationResult.CreateError(nameof(Span),
                _languageService.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_Duration_NotLess_Zero_Validation))));
        }
    }

    private void OnSelectedRangeChanged()
    {
        RemoveTemporaryRanges();

        if (_selectedRange is null)
        {
            return;
        }

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

    private void OnStartDateChanged()
    {
        if (TimeAdjustmentStrategy == TimeAdjustmentStrategy.AdjustEndTime)
        {
            UpdateEndDate(EndDate, StartDate + Span);
        }

        UpdateAfterDatesHaveChanged();
    }

    private void OnEndDateChanged()
    {
        UpdateAfterDatesHaveChanged();
    }

    private void UpdateAfterDatesHaveChanged()
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

    private bool UpdateSelectedRange(DateRange? currentSelectedDateRange, DateRange? newSelectedDateRange)
    {
        if (currentSelectedDateRange == newSelectedDateRange)
        {
            return false;
        }

        _selectedRange = newSelectedDateRange;
        RaisePropertyChanged(nameof(SelectedRange));

        return true;
    }

    private bool UpdateStartDate(DateTime currentStartDate, DateTime newStartDate)
    {
        if (currentStartDate == newStartDate)
        {
            return false;
        }

        _startDate = newStartDate;
        RaisePropertyChanged(nameof(StartDate));

        return true;
    }

    private bool UpdateEndDate(DateTime currentEndDate, DateTime newEndDate)
    {
        if (currentEndDate == newEndDate)
        {
            return false;
        }

        _endDate = newEndDate;
        RaisePropertyChanged(nameof(EndDate));

        return true;
    }

    private bool UpdateSpan(TimeSpan currentSpan, TimeSpan newSpan)
    {
        if (currentSpan == newSpan)
        {
            return false;
        }

        _span = newSpan;
        RaisePropertyChanged(nameof(Span));

        return true;
    }

    private DateRange? AddTemporaryRange()
    {
        if (_ranges is null)
        {
            return null;
        }

        var selectedRange = Ranges?.FirstOrDefault(x => x.Start == _startDate && x.End == _endDate);
        if (selectedRange is not null)
        {
            return selectedRange;
        }

        var temporaryRange = new DateRange()
        {
            Name = _languageService.GetString("Controls_DateRangePicker_Custom"),
            Start = _startDate,
            End = _endDate,
            IsTemporary = true
        };

        _ranges.Add(temporaryRange);

        return temporaryRange;
    }

    private void RemoveTemporaryRanges()
    {
        if (_ranges is null)
        {
            return;
        }

        var oldCustomRanges = _ranges.Where(x => x.IsTemporary).ToList();
        foreach (var oldCustomRange in oldCustomRanges)
        {
            _ranges.Remove(oldCustomRange);
        }
    }

    private IDisposable StartUpdateRanges()
    {
        return new DisposableToken<DateRangePickerViewModel>(this, x => x.Instance._isUpdatingRanges = true,
            x => x.Instance._isUpdatingRanges = false);
    }
}
