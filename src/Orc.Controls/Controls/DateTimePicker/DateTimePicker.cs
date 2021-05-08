// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePicker.cs" company="WildGums">
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
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Catel.Threading;
    using Catel.Windows;
    using Catel.Windows.Input;
    using Converters;
    using Enums;
    using Calendar = System.Windows.Controls.Calendar;

    [TemplatePart(Name = "PART_DaysNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_MonthNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_YearNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_HourNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_MinuteNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_SecondNumericTextBox", Type = typeof(NumericTextBox))]

    [TemplatePart(Name = "PART_DaysMonthsSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_MonthsYearSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_YearSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_HourMinuteSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_MinuteSecondSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_SecondAmPmSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_AmPmSeparatorTextBlock", Type = typeof(TextBlock))]

    [TemplatePart(Name = "PART_DaysToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_MonthToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_YearToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_HourToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_MinuteToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_SecondToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_AmPmToggleButton", Type = typeof(ToggleButton))]

    [TemplatePart(Name = "PART_DatePickerIconDropDownButton", Type = typeof(DropDownButton))]

    [TemplatePart(Name = "PART_TodayMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_NowMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_SelectDateMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_SelectTimeMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_ClearMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_CopyMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_PasteMenuItem", Type = typeof(MenuItem))]

    [TemplatePart(Name = "PART_MainGrid", Type = typeof(Grid))]

    [TemplatePart(Name = "PART_AmPmListTextBox", Type = typeof(ListTextBox))]

    [TemplatePart(Name = "PART_CalendarPopup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_Calendar", Type = typeof(Calendar))]

    [TemplatePart(Name = "PART_TimePickerPopup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_TimePicker", Type = typeof(TimePicker))]
    public class DateTimePicker : Control, IEditableControl
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly HashSet<string> SecondMinuteCantAutoProceedBeginningChars = new() {"1", "2", "3", "4", "5"};

        private bool _isTemplateApplied;

        private List<TextBox> _textBoxes;
        private DateTime _todayValue;
        private DateTimeFormatInfo _formatInfo;
        private bool _safeHideTimeValue;
        private bool _applyingFormat;
        private bool _calendarSelectionChangedByKey;
        private bool _suspendTextChanged;

        private readonly int _defaultSecondFormatPosition = 5;
        private readonly int _defaultAmPmFormatPosition = 6;

        private NumericTextBox _daysNumericTextBox;
        private NumericTextBox _monthNumericTextBox;
        private NumericTextBox _yearNumericTextBox;
        private NumericTextBox _hourNumericTextBox;
        private NumericTextBox _minuteNumericTextBox;
        private NumericTextBox _secondNumericTextBox;

        private TextBlock _daysMonthsSeparatorTextBlock;
        private TextBlock _monthsYearSeparatorTextBlock;
        private TextBlock _yearSeparatorTextBlock;
        private TextBlock _hourMinuteSeparatorTextBlock;
        private TextBlock _minuteSecondSeparatorTextBlock;
        private TextBlock _secondAmPmSeparatorTextBlock;
        private TextBlock _amPmSeparatorTextBlock;

        private ToggleButton _daysToggleButton;
        private ToggleButton _monthToggleButton;
        private ToggleButton _yearToggleButton;
        private ToggleButton _hourToggleButton;
        private ToggleButton _minuteToggleButton;
        private ToggleButton _secondToggleButton;
        private ToggleButton _amPmToggleButton;
        
        private DropDownButton _datePickerIconDropDownButton;

        private MenuItem _todayMenuItem;
        private MenuItem _nowMenuItem;
        private MenuItem _selectDateMenuItem;
        private MenuItem _selectTimeMenuItem;
        private MenuItem _clearMenuItem;
        private MenuItem _copyMenuItem;
        private MenuItem _pasteMenuItem;

        private Grid _mainGrid;
        
        private ListTextBox _amPmListTextBox;

        private Popup _calendarPopup;
        private Calendar _calendar;

        private Popup _timePickerPopup;
        private TimePicker _timePicker;

        #region Dependency properties
        public DayOfWeek? FirstDayOfWeek
        {
            get { return (DayOfWeek?)GetValue(FirstDayOfWeekProperty); }
            set { SetValue(FirstDayOfWeekProperty, value); }
        }

        public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register(
            nameof(FirstDayOfWeek), typeof(DayOfWeek?), typeof(DateTimePicker), new PropertyMetadata(default(DayOfWeek?),
                (sender, args) => ((DateTimePicker)sender).OnFirstDayOfWeekChanged(args)));

        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            nameof(Culture), typeof(CultureInfo), typeof(DateTimePicker), new PropertyMetadata(default(CultureInfo),
                (sender, args) => ((DateTimePicker)sender).OnCultureChanged(args)));

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(DateTime?),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((DateTimePicker)sender).OnValueChanged((DateTime?)e.OldValue, (DateTime?) e.NewValue)));

        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register(nameof(ShowOptionsButton), typeof(bool),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, args) => ((DateTimePicker)sender).OnShowOptionsButtonChanged()));

        public bool AllowNull
        {
            get { return (bool)GetValue(AllowNullProperty); }
            set { SetValue(AllowNullProperty, value); }
        }

        public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register(nameof(AllowNull), typeof(bool),
            typeof(DateTimePicker), new PropertyMetadata(false));

        public bool AllowCopyPaste
        {
            get { return (bool)GetValue(AllowCopyPasteProperty); }
            set { SetValue(AllowCopyPasteProperty, value); }
        }

        public static readonly DependencyProperty AllowCopyPasteProperty = DependencyProperty.Register(nameof(AllowCopyPaste), typeof(bool),
            typeof(DateTimePicker), new PropertyMetadata(true));

        public bool HideTime
        {
            get { return (bool)GetValue(HideTimeProperty); }
            set { SetValue(HideTimeProperty, value); }
        }

        public static readonly DependencyProperty HideTimeProperty = DependencyProperty.Register(nameof(HideTime), typeof(bool),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(false, 
                (sender, e) => ((DateTimePicker)sender).OnHideTimeChanged()));

        public bool HideSeconds
        {
            get { return (bool)GetValue(HideSecondsProperty); }
            set { SetValue(HideSecondsProperty, value); }
        }

        public static readonly DependencyProperty HideSecondsProperty = DependencyProperty.Register(nameof(HideSeconds), typeof(bool),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(false, (sender, e) => ((DateTimePicker)sender).OnHideSecondsChanged()));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool),
            typeof(DateTimePicker), new PropertyMetadata(false));

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, (sender, e) => ((DateTimePicker)sender).OnFormatChanged()));

        public bool IsYearShortFormat
        {
            get { return (bool)GetValue(IsYearShortFormatProperty); }
            private set { SetValue(IsYearShortFormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsYearShortFormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsYearShortFormat), typeof(bool),
            typeof(DateTimePicker), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Count(x => x == 'y') < 3));

        public static readonly DependencyProperty IsYearShortFormatProperty = IsYearShortFormatPropertyKey.DependencyProperty;

        public bool IsHour12Format
        {
            get { return (bool)GetValue(IsHour12FormatProperty); }
            private set { SetValue(IsHour12FormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsHour12FormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsHour12Format), typeof(bool),
            typeof(DateTimePicker), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Contains("h")));

        public static readonly DependencyProperty IsHour12FormatProperty = IsHour12FormatPropertyKey.DependencyProperty;

        public bool IsAmPmShortFormat
        {
            get { return (bool)GetValue(IsAmPmShortFormatProperty); }
            private set { SetValue(IsAmPmShortFormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsAmPmShortFormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsAmPmShortFormat), typeof(bool),
            typeof(DateTimePicker), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Count(x => x == 't') < 2));

        public static readonly DependencyProperty IsAmPmShortFormatProperty = IsAmPmShortFormatPropertyKey.DependencyProperty;

        public TimeSpan? TimeValue
        {
            get { return (TimeSpan?)GetValue(TimeValueProperty); }
            set { SetValue(TimeValueProperty, value); }
        }

        public static readonly DependencyProperty TimeValueProperty = DependencyProperty.Register(nameof(TimeValue), typeof(TimeSpan?),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((DateTimePicker)sender).OnTimeValueChanged((TimeSpan?)e.NewValue)));

        public Meridiem AmPmValue
        {
            get { return (Meridiem)GetValue(AmPmValueProperty); }
            set { SetValue(AmPmValueProperty, value); }
        }

        public static readonly DependencyProperty AmPmValueProperty = DependencyProperty.Register(nameof(AmPmValue), typeof(Meridiem),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(Meridiem.AM, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((DateTimePicker)sender).OnAmPmValueChanged((Meridiem)e.NewValue)));

        #endregion

        #region Properties
        public bool IsInEditMode { get; private set; }

        private int? Day
        {
            get => (int?)_daysNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _daysNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Month
        {
            get => (int?)_monthNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _monthNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Year
        {
            get => (int?)_yearNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _yearNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Hour
        {
            get => (int?)_hourNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _hourNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Minute
        {
            get => (int?)_minuteNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _minuteNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Second
        {
            get => (int?)_secondNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _secondNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private string AmPm
        {
            get => _amPmListTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _amPmListTextBox.SetCurrentValue(ListTextBox.ValueProperty, value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetCurrentValue(KeyboardNavigation.TabNavigationProperty, KeyboardNavigationMode.Local);

            /*Days numeric text box*/
            _daysNumericTextBox = GetTemplateChild("PART_DaysNumericTextBox") as NumericTextBox;
            if (_daysNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_DaysNumericTextBox'");
            }
            _daysNumericTextBox.ValueChanged += OnDaysValueChanged;
            _daysNumericTextBox.TextChanged += OnDaysTextChanged;

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
            _yearNumericTextBox.TextChanged += OnYearTextChanged;

            /*Hour numeric text box*/
            _hourNumericTextBox = GetTemplateChild("PART_HourNumericTextBox") as NumericTextBox;
            if (_hourNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HourNumericTextBox'");
            }
            _hourNumericTextBox.ValueChanged += OnHourValueChanged;
            _hourNumericTextBox.TextChanged += OnHourTextChanged;

            /*Hour numeric text box*/
            _minuteNumericTextBox = GetTemplateChild("PART_MinuteNumericTextBox") as NumericTextBox;
            if (_minuteNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MinuteNumericTextBox'");
            }
            _minuteNumericTextBox.ValueChanged += OnMinuteValueChanged;
            _minuteNumericTextBox.TextChanged += OnMinuteTextChanged;

            _secondNumericTextBox = GetTemplateChild("PART_SecondNumericTextBox") as NumericTextBox;
            if (_secondNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SecondNumericTextBox'");
            }
            _secondNumericTextBox.ValueChanged += OnSecondValueChanged;

            _amPmListTextBox = GetTemplateChild("PART_AmPmListTextBox") as ListTextBox;
            if (_amPmListTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_AmPmListTextBox'");
            }
            _amPmListTextBox.ValueChanged += OnAmPmListTextBoxValueChanged;

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

            _hourMinuteSeparatorTextBlock = GetTemplateChild("PART_HourMinuteSeparatorTextBlock") as TextBlock;
            if (_hourMinuteSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HourMinuteSeparatorTextBlock'");
            }

            _minuteSecondSeparatorTextBlock = GetTemplateChild("PART_MinuteSecondSeparatorTextBlock") as TextBlock;
            if (_minuteSecondSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MinuteSecondSeparatorTextBlock'");
            }

            _secondAmPmSeparatorTextBlock = GetTemplateChild("PART_SecondAmPmSeparatorTextBlock") as TextBlock;
            if (_secondAmPmSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SecondAmPmSeparatorTextBlock'");
            }

            _amPmSeparatorTextBlock = GetTemplateChild("PART_AmPmSeparatorTextBlock") as TextBlock;
            if (_amPmSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_AmPmSeparatorTextBlock'");
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

            _hourToggleButton = GetTemplateChild("PART_HourToggleButton") as ToggleButton;
            if (_hourToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HourToggleButton'");
            }
            _hourToggleButton.Checked += OnToggleButtonChecked;

            _minuteToggleButton = GetTemplateChild("PART_MinuteToggleButton") as ToggleButton;
            if (_minuteToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MinuteToggleButton'");
            }
            _minuteToggleButton.Checked += OnToggleButtonChecked;

            _secondToggleButton = GetTemplateChild("PART_SecondToggleButton") as ToggleButton;
            if (_secondToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SecondToggleButton'");
            }
            _secondToggleButton.Checked += OnToggleButtonChecked;

            _amPmToggleButton = GetTemplateChild("PART_AmPmToggleButton") as ToggleButton;
            if (_amPmToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_AmPmToggleButton'");
            }
            _amPmToggleButton.Checked += OnToggleButtonChecked;

            _datePickerIconDropDownButton = GetTemplateChild("PART_DatePickerIconDropDownButton") as DropDownButton;
            if (_datePickerIconDropDownButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_DatePickerIconDropDownButton'");
            }
            _datePickerIconDropDownButton.SetCurrentValue(VisibilityProperty, ShowOptionsButton ? Visibility.Visible : Visibility.Hidden);

            /*Menu items*/
            _todayMenuItem = GetTemplateChild("PART_TodayMenuItem") as MenuItem;
            if (_todayMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_TodayMenuItem'");
            }
            _todayMenuItem.Click += OnTodayMenuItemClick;

            _nowMenuItem = GetTemplateChild("PART_NowMenuItem") as MenuItem;
            if (_nowMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_NowMenuItem'");
            }
            _nowMenuItem.Click += OnNowMenuItemClick;

            _selectDateMenuItem = GetTemplateChild("PART_SelectDateMenuItem") as MenuItem;
            if (_selectDateMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SelectDateMenuItem'");
            }
            _selectDateMenuItem.Click += OnSelectDateMenuItemClick;

            _selectTimeMenuItem = GetTemplateChild("PART_SelectTimeMenuItem") as MenuItem;
            if (_selectTimeMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SelectTimeMenuItem'");
            }
            _selectTimeMenuItem.Click += OnSelectTimeMenuItemClick;

            _clearMenuItem = GetTemplateChild("PART_ClearMenuItem") as MenuItem;
            if (_clearMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ClearMenuItem'");
            }
            _clearMenuItem.Click += OnClearMenuItemClick;

            _copyMenuItem = GetTemplateChild("PART_CopyMenuItem") as MenuItem;
            if (_copyMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_CopyMenuItem'");
            }
            _copyMenuItem.Click += OnCopyMenuItemClick;

            _pasteMenuItem = GetTemplateChild("PART_PasteMenuItem") as MenuItem;
            if (_pasteMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_PasteMenuItem'");
            }
            _pasteMenuItem.Click += OnPasteMenuItemClick;

            /*Main grid*/
            _mainGrid = GetTemplateChild("PART_MainGrid") as Grid;
            if (_mainGrid is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MainGrid'");
            }
            _mainGrid.MouseEnter += OnMouseEnter;
            _mainGrid.MouseLeave += OnMouseLeave;
            _mainGrid.IsKeyboardFocusWithinChanged += OnIsKeyboardFocusWithinChanged;

            /*Pop up*/
            _calendarPopup = GetTemplateChild("PART_CalendarPopup") as Popup;
            if (_calendarPopup is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_CalendarPopup'");
            }
            _calendarPopup.Closed += OnCalendarPopupClosed;

            _calendar = GetTemplateChild("PART_Calendar") as Calendar;
            if (_calendar is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Calendar'");
            }

            /*Time picker Pop up*/
            _timePickerPopup = GetTemplateChild("PART_TimePickerPopup") as Popup;
            if (_timePickerPopup is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_TimePickerPopup'");
            }
            _timePickerPopup.Closed += OnTimePickerPopupClosed;

            _timePicker = GetTemplateChild("PART_TimePicker") as TimePicker;
            if (_timePicker is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_TimePicker'");
            }

            _textBoxes = new List<TextBox>
            {
                _daysNumericTextBox,
                _monthNumericTextBox,
                _yearNumericTextBox,
                _hourNumericTextBox,
                _minuteNumericTextBox,
                _secondNumericTextBox,
                _amPmListTextBox
            };

            var now = DateTime.Now;
            _todayValue = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            _isTemplateApplied = true;

            ApplyFormat();

            UpdateUi();
        }

        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);

            var textBox = _textBoxes[0];

            textBox.SetCurrentValue(FocusableProperty, true);
            Keyboard.Focus(textBox);
        }

        private void OnTodayMenuItemClick(object sender, RoutedEventArgs e)
        {
            UpdateDateTime(DateTime.Today.Date);

            RaiseStopEdit();
        }

        private void OnNowMenuItemClick(object sender, RoutedEventArgs e)
        {
            UpdateDateTime(DateTime.Now);

            RaiseStopEdit();
        }

        private void OnSelectDateMenuItemClick(object sender, RoutedEventArgs e)
        {
            UnsubscribeFromCalendarEvents();

            SetCurrentValue(EnterKeyTraversal.IsEnabledProperty, false);

            _calendarPopup.SetCurrentValue(Popup.IsOpenProperty, true);

            var dateTime = Value ?? _todayValue;
            _calendar.SetCurrentValue(Calendar.DisplayDateProperty, dateTime);
            _calendar.SetCurrentValue(Calendar.SelectedDateProperty, Value);

            _calendar.Focus();
            Keyboard.Focus(_calendar);

            SubscribeToCalendarEvents();
        }

        private void SubscribeToCalendarEvents()
        {
            _calendar.PreviewKeyDown += CalendarOnPreviewKeyDown;
            _calendar.SelectedDatesChanged += CalendarOnSelectedDatesChanged;
        }

        private void UnsubscribeFromCalendarEvents()
        {
            _calendar.PreviewKeyDown -= CalendarOnPreviewKeyDown;
            _calendar.SelectedDatesChanged -= CalendarOnSelectedDatesChanged;
        }

        private void OnSelectTimeMenuItemClick(object sender, RoutedEventArgs e)
        {
            _timePickerPopup.SetCurrentValue(Popup.IsOpenProperty, true);
            var dateTime = Value ?? _todayValue;
            _timePicker.SetCurrentValue(TimePicker.TimeValueProperty, dateTime.TimeOfDay);
            _timePicker.SetCurrentValue(TimePicker.AmPmValueProperty, dateTime.TimeOfDay.Hours < 12 ? Meridiem.AM : Meridiem.PM);
            _timePicker.Focus();
        }
        private void OnTimeValueChanged(TimeSpan? newTimeValue)
        {
            if (newTimeValue is null)
            {
                return;
            }

            if (HideTime)
            {
                SetCurrentValue(HideTimeProperty, false);
            }

            var newDateTime = Value ?? DateTime.Now;

            if(AmPmValue.Equals(Meridiem.PM) && newTimeValue.Value.Hours < 12)
            {
                SetCurrentValue(ValueProperty, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day, newTimeValue.Value.Hours + 12, newTimeValue.Value.Minutes, newTimeValue.Value.Seconds));
            }
            else 
            {
                SetCurrentValue(ValueProperty, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day, newTimeValue.Value.Hours, newTimeValue.Value.Minutes, newTimeValue.Value.Seconds));
            }
        }

        private void OnClearMenuItemClick(object sender, RoutedEventArgs e)
        {
            UpdateDateTime(null);

            RaiseStopEdit();
        }

        private void OnCopyMenuItemClick(object sender, RoutedEventArgs e)
        {
            var value = Value;
            if (value is not null)
            {
                Clipboard.SetText(DateTimeFormatter.Format(value.Value, _formatInfo), TextDataFormat.Text);
            }
        }

        private void OnPasteMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (!Clipboard.ContainsData(DataFormats.Text))
            {
                return;
            }

            var text = Clipboard.GetText(TextDataFormat.Text);
            if (!string.IsNullOrEmpty(text)
                && (DateTimeParser.TryParse(text, _formatInfo, out var value)
                    || DateTime.TryParseExact(text, Format, null, DateTimeStyles.None, out value)
                    || DateTime.TryParseExact(text, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, null, DateTimeStyles.None, out value)
                    || DateTime.TryParseExact(text, CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern, null, DateTimeStyles.None, out value)))
            {
                SetCurrentValue(ValueProperty, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second));
            }
        }

        private void OnFormatChanged()
        {
            ApplyFormat();

            UpdateUi();
        }

        private void OnFirstDayOfWeekChanged(DependencyPropertyChangedEventArgs args)
        {
            ApplyFormat();

            UpdateUi();
        }

        private void OnCultureChanged(DependencyPropertyChangedEventArgs args)
        {
            ApplyFormat();

            UpdateUi();
        }

        private void ApplyFormat()
        {
            if (!_isTemplateApplied)
            {
                return;
            }

            using (new DisposableToken<DateTimePicker>(this, x => x.Instance._applyingFormat = true, x => x.Instance._applyingFormat = false))
            {
                var format = Format;

                try
                {
                    _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(format);
                }
                catch (FormatException)
                {
                    _applyingFormat = false;

                    return;
                }

                var hasLongTimeFormat = !(_formatInfo.HourFormat is null
                                          || _formatInfo.MinuteFormat is null
                                          || _formatInfo.SecondFormat is null);

                var hasAnyTimeFormat = !(_formatInfo.HourFormat is null
                                   && _formatInfo.MinuteFormat is null
                                   && _formatInfo.SecondFormat is null);

                SetCurrentValue(HideTimeProperty, !hasAnyTimeFormat || _safeHideTimeValue);

                var culture = GetCulture();
                var firstDayOfWeek = FirstDayOfWeek ?? culture.DateTimeFormat.FirstDayOfWeek;

                _calendar.SetCurrentValue(Calendar.FirstDayOfWeekProperty, firstDayOfWeek);

                if (!hasLongTimeFormat)
                {
                    var timePattern = DateTimeFormatHelper.ExtractTimePatternFromFormat(format);
                    if (!string.IsNullOrEmpty(timePattern))
                    {
                        timePattern = DateTimeFormatHelper.FindMatchedLongTimePattern(culture, timePattern);
                    }

                    if (string.IsNullOrEmpty(timePattern))
                    {
                        timePattern = culture.DateTimeFormat.LongTimePattern;
                    }

                    var datePattern = DateTimeFormatHelper.ExtractDatePatternFromFormat(format);

                    format = $"{datePattern} {timePattern}";

                    try
                    {
                        _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(format);
                    }
                    catch (FormatException)
                    {
                        _applyingFormat = false;

                        return;
                    }
                }

                UpdateUiPartVisibility();

                var hourPosition = _formatInfo?.HourPosition;
                if (hourPosition is null)
                {
                    return;
                }

                var minutePosition = _formatInfo.MinutePosition;
                if (minutePosition is null)
                {
                    return;
                }

                IsYearShortFormat = _formatInfo.IsYearShortFormat;
                _yearNumericTextBox.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(_formatInfo.IsYearShortFormat ? 0 : 1));
                _yearNumericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(_formatInfo.IsYearShortFormat ? 99 : 3000));

                var isHour12Format = _formatInfo.IsHour12Format ?? true;
                IsHour12Format = isHour12Format;
                _hourNumericTextBox.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(isHour12Format ? 1 : 0));
                _hourNumericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(isHour12Format ? 12 : 23));
                _hourToggleButton.SetCurrentValue(TagProperty, isHour12Format ? DateTimePart.Hour12 : DateTimePart.Hour);

                IsAmPmShortFormat = _formatInfo.IsAmPmShortFormat ?? true;

                EnableOrDisableYearConverterDependingOnFormat();
                EnableOrDisableHourConverterDependingOnFormat();
                EnableOrDisableAmPmConverterDependingOnFormat();

                _amPmListTextBox.SetCurrentValue(ListTextBox.ListOfValuesProperty, new List<string>
                {
                    Meridiems.GetAmForFormat(_formatInfo),
                    Meridiems.GetPmForFormat(_formatInfo)
                });

                _daysNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.DayFormat.Length));
                _monthNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.MonthFormat.Length));
                _yearNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.YearFormat.Length));
                _hourNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.HourFormat.Length));
                _minuteNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.MinuteFormat.Length));
                _secondNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.SecondFormat?.Length ?? 0));

                UnsubscribeNumericTextBoxes();
                
                Grid.SetColumn(_daysNumericTextBox, GetPosition(_formatInfo.DayPosition));
                Grid.SetColumn(_monthNumericTextBox, GetPosition(_formatInfo.MonthPosition));
                Grid.SetColumn(_yearNumericTextBox, GetPosition(_formatInfo.YearPosition));
                Grid.SetColumn(_hourNumericTextBox, GetPosition(hourPosition.Value));
                Grid.SetColumn(_minuteNumericTextBox, GetPosition(minutePosition.Value));
                Grid.SetColumn(_secondNumericTextBox, GetPosition(_formatInfo.SecondPosition ?? _defaultSecondFormatPosition));
                Grid.SetColumn(_amPmListTextBox, GetPosition(_formatInfo.AmPmPosition ?? _defaultAmPmFormatPosition));

                Grid.SetColumn(_daysToggleButton, GetPosition(_formatInfo.DayPosition) + 1);
                Grid.SetColumn(_monthToggleButton, GetPosition(_formatInfo.MonthPosition) + 1);
                Grid.SetColumn(_yearToggleButton, GetPosition(_formatInfo.YearPosition) + 1);
                Grid.SetColumn(_hourToggleButton, GetPosition(hourPosition.Value) + 1);
                Grid.SetColumn(_minuteToggleButton, GetPosition(minutePosition.Value) + 1);
                Grid.SetColumn(_secondToggleButton, GetPosition(_formatInfo.SecondPosition ?? _defaultSecondFormatPosition) + 1);

                Grid.SetColumn(_amPmToggleButton, GetPosition((_formatInfo.AmPmPosition ?? _defaultAmPmFormatPosition) + 1));

                // Fix positions which could be broken, because of AM/PM textBlock.
                // Fix for seconds in a same way
                int dayPos = _formatInfo.DayPosition, monthPos = _formatInfo.MonthPosition, yearPos = _formatInfo.YearPosition,
                    hourPos = hourPosition.Value, minutePos = minutePosition.Value, secondPos = _formatInfo.SecondPosition ?? 5,
                    amPmPos = _formatInfo.AmPmPosition ?? 6;
                FixNumericTextBoxesPositions(ref dayPos, ref monthPos, ref yearPos, ref hourPos, ref minutePos, ref secondPos, ref amPmPos);

                _textBoxes[dayPos] = _daysNumericTextBox;
                _textBoxes[monthPos] = _monthNumericTextBox;
                _textBoxes[yearPos] = _yearNumericTextBox;
                _textBoxes[hourPos] = _hourNumericTextBox;
                _textBoxes[minutePos] = _minuteNumericTextBox;
                _textBoxes[secondPos] = _secondNumericTextBox;
                _textBoxes[amPmPos] = _amPmListTextBox;

                // Fix tab order inside control.
                _daysNumericTextBox.SetCurrentValue(KeyboardNavigation.TabIndexProperty, dayPos);
                _monthNumericTextBox.SetCurrentValue(KeyboardNavigation.TabIndexProperty, monthPos);
                _yearNumericTextBox.SetCurrentValue(KeyboardNavigation.TabIndexProperty, yearPos);
                _hourNumericTextBox.SetCurrentValue(KeyboardNavigation.TabIndexProperty, hourPos);
                _minuteNumericTextBox.SetCurrentValue(KeyboardNavigation.TabIndexProperty, minutePos);
                _secondNumericTextBox.SetCurrentValue(KeyboardNavigation.TabIndexProperty, secondPos);
                _amPmListTextBox.SetCurrentValue(KeyboardNavigation.TabIndexProperty, amPmPos);
                _datePickerIconDropDownButton.SetCurrentValue(KeyboardNavigation.TabIndexProperty, amPmPos + 1);

                SubscribeNumericTextBoxes();

                _daysMonthsSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator1);
                _monthsYearSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator2);
                _yearSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator3);
                _hourMinuteSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator4);
                _minuteSecondSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator5);
                _secondAmPmSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator6);
                _amPmSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value is null ? string.Empty : _formatInfo.Separator7);
            }
        }
        
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            UpdateUiPartVisibility();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            UpdateUiPartVisibility();
        }

        private void OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateUiPartVisibility();
        }

        private void UpdateUiPartVisibility()
        {
            if(_formatInfo is null)
            {
                return;
            }

            //hide parts according to Hide options and format
            var isHiglighted = _mainGrid.IsMouseOver || _mainGrid.IsKeyboardFocusWithin;

            /**** date parts ****/
            var isYearVisible = _formatInfo.YearFormat is not null;
            var isMonthVisible = _formatInfo.MonthFormat is not null;
            var isDayVisible = _formatInfo.DayFormat is not null;

            var yearHiddenVisibility = isYearVisible ? Visibility.Hidden : Visibility.Collapsed;
            var monthHiddenVisibility = isMonthVisible ? Visibility.Hidden : Visibility.Collapsed;
            var dayHiddenVisibility = isDayVisible ? Visibility.Hidden : Visibility.Collapsed;

            /* year parts */
            _yearNumericTextBox.SetCurrentValue(VisibilityProperty, isYearVisible.ToVisibility());
            _yearSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isYearVisible && !isHiglighted).ToVisibility(yearHiddenVisibility));
            _monthsYearSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isYearVisible && !isHiglighted).ToVisibility(yearHiddenVisibility));
            _yearToggleButton.SetCurrentValue(VisibilityProperty, (isYearVisible && isHiglighted).ToVisibility(yearHiddenVisibility));

            /* month parts */
            _monthNumericTextBox.SetCurrentValue(VisibilityProperty, isMonthVisible.ToVisibility());
            _daysMonthsSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isMonthVisible && !isHiglighted).ToVisibility(monthHiddenVisibility));
            _monthToggleButton.SetCurrentValue(VisibilityProperty, (isMonthVisible && isHiglighted).ToVisibility(monthHiddenVisibility));

            /* day parts*/
            _daysNumericTextBox.SetCurrentValue(VisibilityProperty, isDayVisible.ToVisibility());
            _daysToggleButton.SetCurrentValue(VisibilityProperty, (isDayVisible && isHiglighted).ToVisibility(dayHiddenVisibility));


            /**** time parts ****/
            var isTimeVisible = !HideTime;
            var isAmPmVisible = _formatInfo.AmPmFormat is not null && isTimeVisible;
            var isSecondsVisible = _formatInfo.SecondFormat is not null && !HideSeconds && isTimeVisible;
            var isMinutesVisible = _formatInfo.MinuteFormat is not null && isTimeVisible;
            var isHoursVisible = _formatInfo.HourFormat is not null && isTimeVisible;

            var secondsHiddenVisibility = isSecondsVisible ? Visibility.Hidden : Visibility.Collapsed;
            var minutesHiddenVisibility = isMinutesVisible ? Visibility.Hidden : Visibility.Collapsed;
            var hoursHiddenVisibility = isHoursVisible ? Visibility.Hidden : Visibility.Collapsed;
            var ampmHiddenVisibility = isAmPmVisible ? Visibility.Hidden : Visibility.Collapsed;

            /* second parts */
            _secondNumericTextBox.SetCurrentValue(VisibilityProperty, isSecondsVisible.ToVisibility());
            _minuteSecondSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isSecondsVisible && !isHiglighted).ToVisibility(secondsHiddenVisibility));
            _secondToggleButton.SetCurrentValue(VisibilityProperty, (isSecondsVisible && isHiglighted).ToVisibility(secondsHiddenVisibility));
            
            /* minute parts */
            _minuteNumericTextBox.SetCurrentValue(VisibilityProperty, isMinutesVisible.ToVisibility());
            _hourMinuteSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isMinutesVisible && !isHiglighted).ToVisibility(minutesHiddenVisibility));
            _minuteToggleButton.SetCurrentValue(VisibilityProperty, (isMinutesVisible && isHiglighted).ToVisibility(minutesHiddenVisibility));

            /* hour parts */
            _hourNumericTextBox.SetCurrentValue(VisibilityProperty, isHoursVisible.ToVisibility());
            _hourToggleButton.SetCurrentValue(VisibilityProperty, (isHoursVisible && isHiglighted).ToVisibility(hoursHiddenVisibility));
          
            /* am pm parts */
            _amPmListTextBox.SetCurrentValue(VisibilityProperty, isAmPmVisible.ToVisibility());
            _secondAmPmSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isAmPmVisible && !isHiglighted).ToVisibility(ampmHiddenVisibility));
            _amPmSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isAmPmVisible && !isHiglighted).ToVisibility(ampmHiddenVisibility));
            _amPmToggleButton.SetCurrentValue(VisibilityProperty, (isAmPmVisible && isHiglighted).ToVisibility(ampmHiddenVisibility));
        }

        private void OnToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var activeDateTimePart = (DateTimePart)toggleButton.Tag;

            TextBox activeTextBox;
            switch (activeDateTimePart)
            {
                case DateTimePart.Day:
                    activeTextBox = _daysNumericTextBox;
                    break;

                case DateTimePart.Hour:
                    activeTextBox = _hourNumericTextBox;
                    break;

                case DateTimePart.Month:
                    activeTextBox = _monthNumericTextBox;
                    break;

                case DateTimePart.Year:
                    activeTextBox = _yearNumericTextBox;
                    break;

                case DateTimePart.Hour12:
                    activeTextBox = _hourNumericTextBox;
                    break;

                case DateTimePart.Minute:
                    activeTextBox = _minuteNumericTextBox;
                    break;

                case DateTimePart.Second:
                    activeTextBox = _secondNumericTextBox;
                    break;

                case DateTimePart.AmPmDesignator:
                    activeTextBox = _amPmListTextBox;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            var dateTime = Value ?? _todayValue;
            var dateTimePartHelper = new DateTimePartHelper(dateTime, activeDateTimePart, _formatInfo, activeTextBox, toggleButton);
            dateTimePartHelper.CreatePopup();
        }

        private void OnValueChanged(DateTime? oldValue, DateTime? newValue)
        {
            var ov = oldValue;
            var nv = newValue;

            if (!AllowNull && newValue is null)
            {
                var dateTime = DateTime.Now;
                nv = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
            }

            if (ov is null && nv is not null || ov is not null && nv is null)
            {
                ApplyFormat();
            }

            if (newValue is null && nv is not null)
            {
                var dispatcherService = this.GetServiceLocator().ResolveType<IDispatcherService>();
                dispatcherService.Invoke(() => SetCurrentValue(ValueProperty, nv));
            }
            
            UpdateUi();
        }

        private void UpdateDateTime(DateTime? dateTime)
        {
            if (!AllowNull && !dateTime.HasValue)
            {
                dateTime = _todayValue;
            }

            SetCurrentValue(ValueProperty, dateTime);
        }

        private void UpdateDate(DateTime? date)
        {
            if (!AllowNull && !date.HasValue)
            {
                date = _todayValue;
            }

            // Keep the time!
            if (date is not null)
            {
                var currentValue = Value;
                if (currentValue.HasValue)
                {
                    date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day,
                        currentValue.Value.Hour, currentValue.Value.Minute, currentValue.Value.Second);
                }
            }

            SetCurrentValue(ValueProperty, date);
        }

        private void UpdateUi()
        {
            if (!_isTemplateApplied)
            {
                return;
            }

            using (new DisposableToken<DateTimePicker>(this, x => x.Instance._suspendTextChanged = true, x => x.Instance._suspendTextChanged = false))
            {
                var value = Value;

                Day = value?.Day;
                Month = value?.Month;
                Year = value?.Year;

                var hour = value?.Hour;
                if (hour is not null && IsHour12Format && hour > 12)
                {
                    hour -= 12;
                }

                Hour = hour;
                Minute = value?.Minute;
                Second = value?.Second;
                AmPm = value is not null ? value.Value >= value.Value.Date.AddHours(12) ? Meridiems.LongPM : Meridiems.LongAM : null;
            }
        }

        private void OnDaysValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var day = Day;
            if (day is null)
            {
                return;
            }

            if (value?.Day == day)
            {
                return;
            }
            
            var currentValue = value ?? _todayValue;
            var newValue = new DateTime(currentValue.Year, currentValue.Month, day.Value, currentValue.Hour, currentValue.Minute, currentValue.Second);
            
            SetCurrentValue(ValueProperty, newValue);
        }

        private void OnDaysTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suspendTextChanged)
            {
                return;
            }

            var dayText = _daysNumericTextBox.Text;
            if (string.IsNullOrWhiteSpace(dayText))
            {
                return;
            }

            if (dayText == "1" || dayText == "2")
            {
                return;
            }

            var currentValue = Value ?? _todayValue;
            if (dayText == "3")
            {
                if (currentValue.Month == 2)
                {
                    var monthPosition = (int)_monthNumericTextBox.GetValue(KeyboardNavigation.TabIndexProperty);
                    var dayPosition = (int)_daysNumericTextBox.GetValue(KeyboardNavigation.TabIndexProperty);

                    if (dayPosition < monthPosition)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            using (new DisposableToken<DateTimePicker>(this, x => x.Instance._suspendTextChanged = true, x => x.Instance._suspendTextChanged = false))
            {
                _daysNumericTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void OnMonthValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var month = Month;
            if (month is null)
            {
                return;
            }

            if (value?.Month == month)
            {
                return;
            }

            var currentValue = value ?? _todayValue;

            var daysInMonth = DateTime.DaysInMonth(currentValue.Year, month.Value);
            if (Day <= daysInMonth)
            {
                SetCurrentValue(ValueProperty, new DateTime(currentValue.Year, month.Value, Day.Value, currentValue.Hour, currentValue.Minute, currentValue.Second));
                return;
            }

            Day = daysInMonth;
            SetCurrentValue(ValueProperty, new DateTime(currentValue.Year, month.Value, daysInMonth, currentValue.Hour, currentValue.Minute, currentValue.Second));
        }

        private void OnMonthTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMaxDaysInMonth();

            if (_suspendTextChanged)
            {
                return;
            }

            var monthText = _monthNumericTextBox.Text;
            if (string.IsNullOrWhiteSpace(monthText))
            {
                return;
            }

            if (monthText == "1")
            {
                return;
            }

            using (new DisposableToken<DateTimePicker>(this, x => x.Instance._suspendTextChanged = true, x => x.Instance._suspendTextChanged = false))
            {
                _monthNumericTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void OnYearValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var year = Year;
            if (year is null)
            {
                return;
            }

            if (value?.Year == year)
            {
                return;
            }

            var currentValue = value ?? _todayValue;
            SetCurrentValue(ValueProperty, new DateTime(year.Value, currentValue.Month, currentValue.Day, currentValue.Hour, currentValue.Minute, currentValue.Second));
        }

        private void OnYearTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suspendTextChanged)
            {
                return;
            }

            var yearText = _yearNumericTextBox.Text;
            if (string.IsNullOrWhiteSpace(yearText))
            {
                return;
            }

            if (yearText.Length != 4)
            {
                return;
            }

            using (new DisposableToken<DateTimePicker>(this, x => x.Instance._suspendTextChanged = true, x => x.Instance._suspendTextChanged = false))
            {
                _yearNumericTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void OnHourValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var hour = Hour;
            if (hour is null)
            {
                return;
            }

            if (value?.Hour == hour)
            {
                return;
            }

            if (AmPm == "PM" && IsHour12Format && hour < 12)
            {
                hour += 12;
            }

            var currentValue = value ?? _todayValue;
            SetCurrentValue(ValueProperty, new DateTime(currentValue.Year, currentValue.Month, currentValue.Day, hour.Value, currentValue.Minute, currentValue.Second));
        }

        private void OnHourTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suspendTextChanged)
            {
                return;
            }

            var hourText = _hourNumericTextBox.Text;
            if (string.IsNullOrWhiteSpace(hourText))
            {
                return;
            }

            if (hourText == "1")
            {
                return;
            }

            if (!IsHour12Format && hourText == "2")
            {
                return;
            }

            using (new DisposableToken<DateTimePicker>(this, x => x.Instance._suspendTextChanged = true, x => x.Instance._suspendTextChanged = false))
            {
                _hourNumericTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void OnMinuteValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var minute = Minute;
            if (minute is null)
            {
                return;
            }

            if (value?.Minute == minute)
            {
                return;
            }

            var currentValue = value ?? _todayValue;
            SetCurrentValue(ValueProperty, new DateTime(currentValue.Year, currentValue.Month, currentValue.Day, currentValue.Hour, minute.Value, currentValue.Second));
        }
        
        private void OnMinuteTextChanged(object sender, TextChangedEventArgs e)
        {
            TryProceedSecondMinuteEditing(_minuteNumericTextBox);
        }

        private void OnSecondValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var second = Second;
            if (second is null)
            {
                return;
            }

            if (value?.Second == second)
            {
                return;
            }

            var currentValue = value ?? _todayValue;
            SetCurrentValue(ValueProperty, new DateTime(currentValue.Year, currentValue.Month, currentValue.Day, currentValue.Hour, currentValue.Minute, second.Value));
        }

        private void OnSecondTextChanged(object sender, TextChangedEventArgs e)
        {
            TryProceedSecondMinuteEditing(_secondNumericTextBox);
        }

        private void OnAmPmListTextBoxValueChanged(object sender, EventArgs e)
        {
            var amPm = AmPm;
            var value = Value;

            if (!Meridiems.LongAM.Equals(amPm) && !Meridiems.LongPM.Equals(amPm))
            {
                return;
            }

            if (value is null)
            {
                SetCurrentValue(ValueProperty, new DateTime(_todayValue.Year, _todayValue.Month, _todayValue.Day, _todayValue.Hour, _todayValue.Minute, _todayValue.Second));
            }
            else
            {
                var currentValue = value.Value;
                if (currentValue.Hour >= 12 && Meridiems.LongPM.Equals(amPm)
                    || currentValue.Hour < 12 && Meridiems.LongAM.Equals(amPm))
                {
                    return;
                }

                var newValue = new DateTime(currentValue.Year, currentValue.Month, currentValue.Day, currentValue.Hour, currentValue.Minute, currentValue.Second);
                newValue = newValue.AddHours(Meridiems.LongAM.Equals(amPm) ? -12 : 12);

                SetCurrentValue(ValueProperty, newValue);
            }
        }

        private void OnAmPmValueChanged(Meridiem newValue)
        {
            var newDateTime = Value ?? DateTime.Now;

            var isMeridiemAm = newValue == Meridiem.AM;
            var isTimeAm = newDateTime.Hour < 12;

            var hours = new TimeSpan(12, 0, 0);

            if (!(isMeridiemAm ^ isTimeAm))
            {
                return;
            }

            var diffTime = isMeridiemAm ? newDateTime.TimeOfDay.Subtract(hours) : newDateTime.TimeOfDay.Add(hours);

            var diffDate = new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day, diffTime.Hours, diffTime.Minutes, diffTime.Seconds);
            SetCurrentValue(ValueProperty, diffDate);

        }

        private void UpdateMaxDaysInMonth()
        {
            if (!int.TryParse(_monthNumericTextBox.Text, out var month))
            {
                return;
            }

            if (month > 12 || month < 1)
            {
                return;
            }

            if (!int.TryParse(_yearNumericTextBox.Text, out var year))
            {
                return;
            }

            if (year < 0)
            {
                return;
            }

            var daysInMonth = DateTime.DaysInMonth(year, month);
            _daysNumericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)daysInMonth);
        }

        private void OnShowOptionsButtonChanged()
        {
            _datePickerIconDropDownButton?.SetCurrentValue(VisibilityProperty, ShowOptionsButton ? Visibility.Visible : Visibility.Collapsed);
        }

        private void OnHideTimeChanged()
        {
            if (!_applyingFormat)
            {
                _safeHideTimeValue = HideTime;
            }

            ApplyFormat();
        }

        private void OnHideSecondsChanged()
        {
            ApplyFormat();
        }

        private async void OnTextBoxLeftBoundReached(object sender, EventArgs e)
        {
            var currentTextBoxIndex = _textBoxes.IndexOf((TextBox)sender);
            var prevTextBox = _textBoxes[currentTextBoxIndex - 1];

            prevTextBox.CaretIndex = prevTextBox.Text.Length;

            await TaskShim.Delay(1); // Note: this is a hack. without this delay it is not possible to set focus

            prevTextBox.Focus();
        }

        private async void OnTextBoxRightBoundReached(object sender, EventArgs eventArgs)
        {
            var currentTextBoxIndex = _textBoxes.IndexOf((TextBox)sender);
            var nextTextBox = _textBoxes[currentTextBoxIndex + 1];

            nextTextBox.CaretIndex = 0;

            await TaskShim.Delay(1); // Note: this is a hack. without this delay it is not possible to set focus

            nextTextBox.Focus();
        }

        private void SubscribeNumericTextBoxes()
        {
            // Enable support for switching between textboxes,
            // 0-5 because we can't switch to right on last textBox.
            for (var i = 0; i <= 5; i++)
            {
                _textBoxes[i].SubscribeToOnRightBoundReachedEvent(OnTextBoxRightBoundReached);
            }

            // Enable support for switching between textboxes,
            // 5-1 because we can't switch to left on first textBox.
            for (var i = 6; i >= 1; i--)
            {
                _textBoxes[i].SubscribeToOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
            }
        }

        private void UnsubscribeNumericTextBoxes()
        {
            // Disable support for switching between textboxes,
            // 0-4 because we can't switch to right on last textBox.
            for (var i = 0; i <= 5; i++)
            {
                _textBoxes[i].UnsubscribeFromOnRightBoundReachedEvent(OnTextBoxRightBoundReached);
            }

            // Disable support for switching between textboxes,
            // 5-1 because we can't switch to left on first textBox.
            for (var i = 6; i >= 1; i--)
            {
                _textBoxes[i].UnsubscribeFromOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
            }
        }

        private void EnableOrDisableHourConverterDependingOnFormat()
        {
            if (!(TryFindResource(nameof(Hour24ToHour12Converter)) is Hour24ToHour12Converter converter))
            {
                return;
            }

            converter.IsEnabled = IsHour12Format;
            BindingOperations.GetBindingExpression(_hourNumericTextBox, NumericTextBox.ValueProperty)?.UpdateTarget();
        }

        private void EnableOrDisableAmPmConverterDependingOnFormat()
        {
            if (!(TryFindResource(nameof(AmPmLongToAmPmShortConverter)) is AmPmLongToAmPmShortConverter converter))
            {
                return;
            }

            converter.IsEnabled = IsAmPmShortFormat;
            BindingOperations.GetBindingExpression(_amPmListTextBox, ListTextBox.ValueProperty)?.UpdateTarget();
        }

        private void EnableOrDisableYearConverterDependingOnFormat()
        {
            if (!(TryFindResource(nameof(YearLongToYearShortConverter)) is YearLongToYearShortConverter converter))
            {
                return;
            }

            converter.IsEnabled = IsYearShortFormat;
            BindingOperations.GetBindingExpression(_amPmListTextBox, NumericTextBox.ValueProperty)?.UpdateTarget();
        }

        private static void FixNumericTextBoxesPositions(ref int dayPosition, ref int monthPosition, ref int yearPosition, ref int hourPosition, ref int minutePosition, ref int secondPosition, ref int amPmPosition)
        {
            var dict = new Dictionary<string, int>
            {
                { "dayPosition", dayPosition },
                { "monthPosition", monthPosition },
                { "yearPosition", yearPosition },
                { "hourPosition", hourPosition },
                { "minutePosition", minutePosition },
                { "secondPosition", secondPosition },
                { "amPmPosition", amPmPosition }
            };

            var current = 0;
            foreach (var entry in dict.OrderBy(x => x.Value))
            {
                dict[entry.Key] = current++;
            }

            dayPosition = dict["dayPosition"];
            monthPosition = dict["monthPosition"];
            yearPosition = dict["yearPosition"];
            hourPosition = dict["hourPosition"];
            minutePosition = dict["minutePosition"];
            secondPosition = dict["secondPosition"];
            amPmPosition = dict["amPmPosition"];
        }

        private static int GetPosition(int index)
        {
            return index * 2;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key != Key.OemSemicolon)
            {
                return;
            }

            if (KeyboardHelper.AreKeyboardModifiersPressed(ModifierKeys.Control))
            {
                UpdateDateTime(DateTime.Today.Date);
                e.Handled = true;
            }

            if (KeyboardHelper.AreKeyboardModifiersPressed(ModifierKeys.Control | ModifierKeys.Shift))
            {
                UpdateDateTime(DateTime.Now);
                e.Handled = true;
            }
        }

        private void CalendarOnSelectedDatesChanged(object sender, SelectionChangedEventArgs args)
        {
            if (!(sender is Calendar calendar))
            {
                return;
            }

            if (_calendarSelectionChangedByKey)
            {
                _calendarSelectionChangedByKey = false;

                return;
            }

            if (AllowNull || calendar.SelectedDate.HasValue)
            {
                UpdateDate(calendar.SelectedDate);
            }

            ((Popup)calendar.Parent).SetCurrentValue(Popup.IsOpenProperty, _calendarSelectionChangedByKey);

            _calendarSelectionChangedByKey = false;
        }

        private void CalendarOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var calendar = ((Calendar)sender);
            if (e.Key == Key.Escape)
            {
                ((Popup)calendar.Parent).SetCurrentValue(Popup.IsOpenProperty, false);
                _daysNumericTextBox.Focus();
                e.Handled = true;
            }

            if (e.Key != Key.Enter)
            {
                _calendarSelectionChangedByKey = true;

                return;
            }

            if (calendar.SelectedDate.HasValue)
            {
                UpdateDate(calendar.SelectedDate.Value);
            }

            ((Popup)calendar.Parent).SetCurrentValue(Popup.IsOpenProperty, false);

            e.Handled = true;
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            UpdateMaxDaysInMonth();

            IsInEditMode = true;

            EditStarted?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);

            InvalidateEditMode();
        }

        private void OnCalendarPopupClosed(object sender, EventArgs e)
        {
            SetCurrentValue(EnterKeyTraversal.IsEnabledProperty, true);

            InvalidateEditMode();
        }

        private void OnTimePickerPopupClosed(object sender, EventArgs e)
        {
            InvalidateEditMode();
        }

        private void InvalidateEditMode()
        {
            if (!IsInEditMode)
            {
                return;
            }

            if (IsKeyboardFocusWithin || IsKeyboardFocused || IsFocused)
            {
                return;
            }

            var focusedControl = FocusManager.GetFocusedElement(this) as FrameworkElement;
            var keyboardFocusedControl = focusedControl ?? Keyboard.FocusedElement as FrameworkElement;
            var root = keyboardFocusedControl?.FindLogicalOrVisualAncestor(x => Equals(this, x));
            if (root is not null)
            {
                return;
            }

            if (_datePickerIconDropDownButton.IsChecked == true)
            {
                return;
            }

            if (_calendarPopup is not null && _calendarPopup.IsOpen)
            {
                _calendarPopup.Closed += OnCalendarPopupClosed;
                return;
            }

            if (_timePickerPopup is not null && _timePickerPopup.IsOpen)
            {
                _timePickerPopup.Closed += OnTimePickerPopupClosed;
                return;
            }

            if (_daysToggleButton.IsChecked == true
                || _minuteToggleButton.IsChecked == true
                || _hourToggleButton.IsChecked == true
                || _monthToggleButton.IsChecked == true
                || _amPmToggleButton.IsChecked == true
                || _secondToggleButton.IsChecked == true)
            {
                return;
            }

            RaiseStopEdit();
        }

        private void RaiseStopEdit()
        {
            IsInEditMode = false;

            EditEnded?.Invoke(this, EventArgs.Empty);
        }

        private CultureInfo GetCulture()
        {
            return Culture ?? CultureInfo.CurrentUICulture;
        }

        private void TryProceedSecondMinuteEditing(TextBox secondOrMinuteTextBox)
        {
            if (_suspendTextChanged)
            {
                return;
            }

            var text = secondOrMinuteTextBox.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (SecondMinuteCantAutoProceedBeginningChars.Contains(text))
            {
                return;
            }

            using (new DisposableToken<DateTimePicker>(this, x => x.Instance._suspendTextChanged = true, x => x.Instance._suspendTextChanged = false))
            {
                secondOrMinuteTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
        #endregion

        public event EventHandler<EventArgs> EditStarted;
        public event EventHandler<EventArgs> EditEnded;
    }
}
