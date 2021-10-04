// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePicker.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using Catel.Logging;
    using Converters;
    using Calendar = System.Windows.Controls.Calendar;

    [TemplatePart(Name = "PART_DaysNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_MonthNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_YearNumericTextBox", Type = typeof(NumericTextBox))]

    [TemplatePart(Name = "PART_DaysMonthsSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_MonthsYearSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_YearSeparatorTextBlock", Type = typeof(TextBlock))]

    [TemplatePart(Name = "PART_DaysToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_MonthToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_YearToggleButton", Type = typeof(ToggleButton))]

    [TemplatePart(Name = "PART_DatePickerIconToggleButton", Type = typeof(ToggleButton))]

    [TemplatePart(Name = "PART_TodayButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_SelectDateButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CopyButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_PasteButton", Type = typeof(Button))]

    [TemplatePart(Name = "PART_MainGrid", Type = typeof(Grid))]
    [ObsoleteEx(Message = "Use DateTimePicker instead", RemoveInVersion = "5.0.0")]
    public class DatePicker : Control
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private List<TextBox> _textBoxes;
        private DateTime _todayValue;
        private DateTimeFormatInfo _formatInfo;

        private NumericTextBox _daysNumericTextBox;
        private NumericTextBox _monthNumericTextBox;
        private NumericTextBox _yearNumericTextBox;

        private TextBlock _daysMonthsSeparatorTextBlock;
        private TextBlock _monthsYearSeparatorTextBlock;
        private TextBlock _yearSeparatorTextBlock;

        private ToggleButton _daysToggleButton;
        private ToggleButton _monthToggleButton;
        private ToggleButton _yearToggleButton;

        private ToggleButton _datePickerIconToggleButton;

        private Button _todayButton;
        private Button _selectDateButton;
        private Button _clearButton;
        private Button _copyButton;
        private Button _pasteButton;

        private Popup _calendarPopup;

        private Grid _mainGrid;

        #region Dependency properties
        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(DateTime?), typeof(DatePicker),
            new FrameworkPropertyMetadata(DateTime.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((DatePicker)sender).OnValueChanged()));

        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register(nameof(ShowOptionsButton), typeof(bool),
            typeof(DatePicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                (sender, args) => ((DatePicker)sender).OnShowOptionsButtonChanged()));

        public bool AllowNull
        {
            get { return (bool)GetValue(AllowNullProperty); }
            set { SetValue(AllowNullProperty, value); }
        }

        public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register(nameof(AllowNull), typeof(bool),
            typeof(DatePicker), new PropertyMetadata(false));
        
        public bool AllowCopyPaste
        {
            get { return (bool)GetValue(AllowCopyPasteProperty); }
            set { SetValue(AllowCopyPasteProperty, value); }
        }

        public static readonly DependencyProperty AllowCopyPasteProperty = DependencyProperty.Register(nameof(AllowCopyPaste), typeof(bool),
            typeof(DatePicker), new PropertyMetadata(true));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool),
            typeof(DatePicker), new PropertyMetadata(false));

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string),
            typeof(DatePicker), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern,
                (sender, e) => ((DatePicker)sender).OnFormatChanged()));

        public bool IsYearShortFormat
        {
            get { return (bool)GetValue(IsYearShortFormatProperty); }
            private set { SetValue(IsYearShortFormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsYearShortFormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsYearShortFormat), typeof(bool),
            typeof(DatePicker), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Count(x => x == 'y') < 3));

        public static readonly DependencyProperty IsYearShortFormatProperty = IsYearShortFormatPropertyKey.DependencyProperty;
        #endregion

        #region Properties
        private int? Day
        {
            get => (int?)_daysNumericTextBox?.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _daysNumericTextBox?.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Month
        {
            get => (int?)_monthNumericTextBox?.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _monthNumericTextBox?.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Year
        {
            get => (int?)_yearNumericTextBox?.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _yearNumericTextBox?.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }
        #endregion
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            /*Days numeric text box*/
            _daysNumericTextBox = GetTemplateChild("PART_DaysNumericTextBox") as NumericTextBox;
            if (_daysNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_DaysNumericTextBox'");
            }
            _daysNumericTextBox.ValueChanged += OnDaysValueChanged;

            /*Month numeric text box*/
            _monthNumericTextBox = GetTemplateChild("PART_MonthNumericTextBox") as NumericTextBox;
            if (_monthNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MonthNumericTextBox'");
            }
            _monthNumericTextBox.ValueChanged += OnMonthValueChanged;
            _monthNumericTextBox.TextChanged += OnMonthTextChanged;

            /*Year numeric text box*/
            _yearNumericTextBox = GetTemplateChild("PART_YearNumericTextBox") as NumericTextBox;
            if (_yearNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_YearNumericTextBox'");
            }
            _yearNumericTextBox.ValueChanged += OnYearValueChanged;

            /*Separators*/
            _daysMonthsSeparatorTextBlock = GetTemplateChild("PART_DaysMonthsSeparatorTextBlock") as TextBlock;
            if (_daysMonthsSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_DaysMonthsSeparatorTextBlock'");
            }

            _monthsYearSeparatorTextBlock = GetTemplateChild("PART_MonthsYearSeparatorTextBlock") as TextBlock;
            if (_monthsYearSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MonthsYearSeparatorTextBlock'");
            }

            _yearSeparatorTextBlock = GetTemplateChild("PART_YearSeparatorTextBlock") as TextBlock;
            if (_yearSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_YearSeparatorTextBlock'");
            }

            /*Toggles*/
            _daysToggleButton = GetTemplateChild("PART_DaysToggleButton") as ToggleButton;
            if (_daysToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_DaysToggleButton'");
            }
            _daysToggleButton.Checked += OnToggleButtonChecked;

            _monthToggleButton = GetTemplateChild("PART_MonthToggleButton") as ToggleButton;
            if (_monthToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MonthToggleButton'");
            }
            _monthToggleButton.Checked += OnToggleButtonChecked;

            _yearToggleButton = GetTemplateChild("PART_YearToggleButton") as ToggleButton;
            if (_yearToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_YearToggleButton'");
            }
            

            _datePickerIconToggleButton = GetTemplateChild("PART_DatePickerIconToggleButton") as ToggleButton;
            if (_datePickerIconToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_DatePickerIconToggleButton'");
            }
            _datePickerIconToggleButton.SetCurrentValue(VisibilityProperty, ShowOptionsButton ? Visibility.Visible : Visibility.Hidden);

            /*Buttons*/
            _todayButton = GetTemplateChild("PART_TodayButton") as Button;
            if (_todayButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_TodayButton'");
            }
            _todayButton.Click += OnTodayButtonClick;  
            
            _selectDateButton = GetTemplateChild("PART_SelectDateButton") as Button;
            if (_selectDateButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SelectDateButton'");
            }
            _selectDateButton.Click += OnSelectDateButtonClick;

            _clearButton = GetTemplateChild("PART_ClearButton") as Button;
            if (_clearButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ClearButton'");
            }
            _clearButton.Click += OnClearButtonClick;   
            
            _copyButton = GetTemplateChild("PART_CopyButton") as Button;
            if (_copyButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_CopyButton'");
            }
            _copyButton.Click += OnCopyButtonClick;

            _pasteButton = GetTemplateChild("PART_PasteButton") as Button;
            if (_pasteButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_PasteButton'");
            }
            _pasteButton.Click += OnPasteButtonClick;         
            
            /*Main grid*/
            _mainGrid = GetTemplateChild("PART_MainGrid") as Grid;
            if (_mainGrid is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MainGrid'");
            }

            _textBoxes = new List<TextBox>
            {
                _daysNumericTextBox,
                _monthNumericTextBox,
                _yearNumericTextBox,
            };

            var now = DateTime.Now;
            _todayValue = new DateTime(now.Year, now.Month, now.Day);

            ApplyFormat();

            UpdateUi();
        }

        private void OnTodayButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
            UpdateDate(DateTime.Today.Date);
        }

        private void OnSelectDateButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            _calendarPopup = CreateCalendarPopup();
            var calendarPopupSource = CreateCalendarPopupSource();
            _calendarPopup.SetCurrentValue(Popup.ChildProperty, calendarPopupSource);

            _calendarPopup.PreviewKeyDown += OnCalendarPopupPreviewKeyDown;
            _calendarPopup.Closed += OnCalendarPopupClosed;

            calendarPopupSource.Focus();
        }

        private void OnCalendarPopupPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _calendarPopup is not null && _calendarPopup.IsOpen)
            {
                _calendarPopup.SetCurrentValue(Popup.IsOpenProperty, false);
            }
        }

        private void OnCalendarPopupClosed(object sender, EventArgs e)
        {
            _calendarPopup.PreviewKeyDown -= OnCalendarPopupPreviewKeyDown;
            _calendarPopup.Closed -= OnCalendarPopupClosed;
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
            UpdateDate(null);
        }

        private void OnCopyButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            var value = Value;
            if (value is null)
            {
                return;
            }

            try
            {
                Clipboard.SetText(DateTimeFormatter.Format(value.Value, _formatInfo), TextDataFormat.Text);
            }
            catch (FormatException exception)
            {
                Log.Warning(exception);
            }
        }

        private void OnPasteButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            if (!Clipboard.ContainsData(DataFormats.Text))
            {
                return;
            }

            var text = Clipboard.GetText(TextDataFormat.Text);
            if (!string.IsNullOrEmpty(text)
                && (DateTimeParser.TryParse(text, _formatInfo, out var value)
                    || DateTime.TryParseExact(text, Format, null, DateTimeStyles.None, out value)
                    || DateTime.TryParseExact(text, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, null, DateTimeStyles.None, out value)
                    || DateTime.TryParseExact(text, CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, null, DateTimeStyles.None, out value)))
            {
                SetCurrentValue(ValueProperty, new DateTime(value.Year, value.Month, value.Day));
            }
        }

        private void UpdateDate(DateTime? date)
        {
            if (!AllowNull && !date.HasValue)
            {
                date = _todayValue;
            }

            // Trim the time
            if (date is not null)
            {
                date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
            }

            SetCurrentValue(ValueProperty, date);
        }

        private void OnFormatChanged()
        {
            ApplyFormat();

            UpdateUi();
        }

        private void ApplyFormat()
        {
            try
            {
                _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(Format, true);
            }
            catch (FormatException exception)
            {
                Log.Warning(exception);

                return;
            }

            IsYearShortFormat = _formatInfo.IsYearShortFormat;
            _yearNumericTextBox.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(_formatInfo.IsYearShortFormat ? 0 : 1));
            _yearNumericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(_formatInfo.IsYearShortFormat ? 99 : 3000));

            EnableOrDisableYearConverterDependingOnFormat();

            _daysNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.DayFormat.Length));
            _monthNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.MonthFormat.Length));
            _yearNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.YearFormat.Length));

            UnsubscribeNumericTextBoxes();

            Grid.SetColumn(_daysNumericTextBox, GetPosition(_formatInfo.DayPosition));
            Grid.SetColumn(_monthNumericTextBox, GetPosition(_formatInfo.MonthPosition));
            Grid.SetColumn(_yearNumericTextBox, GetPosition(_formatInfo.YearPosition));

            Grid.SetColumn(_daysToggleButton, GetPosition(_formatInfo.DayPosition) + 1);
            Grid.SetColumn(_monthToggleButton, GetPosition(_formatInfo.MonthPosition) + 1);
            Grid.SetColumn(_yearToggleButton, GetPosition(_formatInfo.YearPosition) + 1);

            _textBoxes[_formatInfo.DayPosition] = _daysNumericTextBox;
            _textBoxes[_formatInfo.MonthPosition] = _monthNumericTextBox;
            _textBoxes[_formatInfo.YearPosition] = _yearNumericTextBox;

            // Fix tab order inside control.
            _daysNumericTextBox.SetCurrentValue(TabIndexProperty, _formatInfo.DayPosition);
            _monthNumericTextBox.SetCurrentValue(TabIndexProperty, _formatInfo.MonthPosition);
            _yearNumericTextBox.SetCurrentValue(TabIndexProperty, _formatInfo.YearPosition);
            _datePickerIconToggleButton.SetCurrentValue(TabIndexProperty, int.MaxValue);

            SubscribeNumericTextBoxes();

            _daysMonthsSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator1);
            _monthsYearSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator2);
            _yearSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator3);
        }

        private void OnToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var activeDateTimePart = (DateTimePart)toggleButton.Tag;

            var activeTextBox = toggleButton == _daysToggleButton ? _daysNumericTextBox : _monthNumericTextBox;
            var activeToggleButton = toggleButton;

            var dateTime = Value ?? _todayValue;
            var dateTimePartHelper = new DateTimePartHelper(dateTime, activeDateTimePart, _formatInfo, activeTextBox, activeToggleButton);
            dateTimePartHelper.CreatePopup();
        }

        private void OnValueChanged()
        {
            UpdateUi();
        }

        private void UpdateUi()
        {
            var value = Value;

            Day = value?.Day;
            Month = value?.Month;
            Year = value?.Year;
        }

        private void OnDaysValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var day = Day;

            if (day is null)
            {
                return;
            }

            if (value is null)
            {
                SetCurrentValue(ValueProperty, new DateTime(_todayValue.Year, _todayValue.Month, day.Value));
                return;
            }

            if (value.Value.Day == day)
            {
                return;
            }

            var month = Month ?? DateTime.Today.Month;
            var year = Year ?? DateTime.Today.Year;

            SetCurrentValue(ValueProperty, new DateTime(year, month, day.Value));
        }

        private void OnMonthValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var month = Month;

            if (month is null)
            {
                return;
            }

            if (month < 1)
            {
                month = 1;
            }

            if (month > 12)
            {
                month = 12;
            }

            if (value is null)
            {
                SetCurrentValue(ValueProperty, new DateTime(_todayValue.Year, month.Value, _todayValue.Day));
                return;
            }

            if (value.Value.Month == month)
            {
                return;
            }

            var year = Year ?? _todayValue.Year;
            var daysInMonth = DateTime.DaysInMonth(year, month.Value);

            if (Day <= daysInMonth)
            {
                SetCurrentValue(ValueProperty, new DateTime(year, month.Value, Day.Value));
                return;
            }

            Day = daysInMonth;
            SetCurrentValue(ValueProperty, new DateTime(year, month.Value, daysInMonth));
        }

        private void OnYearValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var year = Year;

            if (year is null)
            {
                return;
            }

            if (value is null)
            {
                SetCurrentValue(ValueProperty, new DateTime(year.Value, _todayValue.Month, _todayValue.Day));
                return;
            }

            if (value.Value.Year == year)
            {
                return;
            }

            var month = Month ?? _todayValue.Month;
            var day = Day ?? _todayValue.Day;
            SetCurrentValue(ValueProperty, new DateTime(year.Value, month, day));
        }
    
        private void OnMonthTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(_monthNumericTextBox.Text, out var month))
            {
                return;
            }

            if (!int.TryParse(_yearNumericTextBox.Text, out var year))
            {
                return;
            }

            var daysInMonth = DateTime.DaysInMonth(year, month);
            _daysNumericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)daysInMonth);
        }

        private Popup CreateCalendarPopup()
        {
            var popup = new Popup
            {
                PlacementTarget = _mainGrid,
                Placement = PlacementMode.Bottom,
                VerticalOffset = -4,
                IsOpen = true,
                StaysOpen = false,
            };

            return popup;
        }

        private Calendar CreateCalendarPopupSource()
        {
            var dateTime = Value ?? _todayValue;
            var calendar = new Calendar
            {
                Margin = new Thickness(0, -3, 0, -3),
                DisplayDate = dateTime,
                SelectedDate = Value
            };

            calendar.SelectedDatesChanged += CalendarOnSelectedDatesChanged;

            return calendar;
        }

        private void CalendarOnSelectedDatesChanged(object sender, SelectionChangedEventArgs args)
        {
            var calendar = (Calendar)sender;
            if (calendar.SelectedDate.HasValue)
            {
                UpdateDate(calendar.SelectedDate.Value);
            }

            ((Popup)calendar.Parent).SetCurrentValue(Popup.IsOpenProperty, false);
        }

        private void EnableOrDisableYearConverterDependingOnFormat()
        {
            if (!(TryFindResource(nameof(YearLongToYearShortConverter))
                is YearLongToYearShortConverter converter))
            {
                return;
            }

            converter.IsEnabled = IsYearShortFormat;
            BindingOperations.GetBindingExpression(_yearNumericTextBox, NumericTextBox.ValueProperty)?.UpdateTarget();
        }

        private void OnShowOptionsButtonChanged()
        {
            _datePickerIconToggleButton.SetCurrentValue(VisibilityProperty, ShowOptionsButton ? Visibility.Visible : Visibility.Collapsed);
        }

        private void SubscribeNumericTextBoxes()
        {
            // Enable support for switching between textboxes,
            // 0-1 because we can't switch to right on last textBox.
            _textBoxes[0].SubscribeToOnRightBoundReachedEvent(OnTextBoxRightBoundReached);
            _textBoxes[1].SubscribeToOnRightBoundReachedEvent(OnTextBoxRightBoundReached);

            // Enable support for switching between textboxes,
            // 2-1 because we can't switch to left on first textBox.
            _textBoxes[2].SubscribeToOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
            _textBoxes[1].SubscribeToOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
        }

        private void UnsubscribeNumericTextBoxes()
        {
            // Disable support for switching between textboxes,
            // 0-1 because we can't switch to right on last textBox.
            _textBoxes[0].UnsubscribeFromOnRightBoundReachedEvent(OnTextBoxRightBoundReached);
            _textBoxes[1].UnsubscribeFromOnRightBoundReachedEvent(OnTextBoxRightBoundReached);

            // Disable support for switching between textboxes,
            // 2-1 because we can't switch to left on first textBox.
            _textBoxes[2].UnsubscribeFromOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
            _textBoxes[1].UnsubscribeFromOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
        }

        private void OnTextBoxLeftBoundReached(object sender, EventArgs e)
        {
            var currentTextBoxIndex = _textBoxes.IndexOf((TextBox)sender);
            var prevTextBox = _textBoxes[currentTextBoxIndex - 1];

            prevTextBox.CaretIndex = prevTextBox.Text.Length;
            prevTextBox.Focus();
        }

        private void OnTextBoxRightBoundReached(object sender, EventArgs eventArgs)
        {
            var currentTextBoxIndex = _textBoxes.IndexOf((TextBox)sender);
            var nextTextBox = _textBoxes[currentTextBoxIndex + 1];

            nextTextBox.CaretIndex = 0;
            nextTextBox.Focus();
        }

        private int GetPosition(int index)
        {
            return index * 2;
        }
    }
}
