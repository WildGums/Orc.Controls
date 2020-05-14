// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePicker1.cs" company="WildGums">
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
    using Catel.MVVM.Views;
    using Converters;
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

    [TemplatePart(Name = "PART_DatePickerIconToggleButton", Type = typeof(ToggleButton))]

    [TemplatePart(Name = "PART_TodayButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NowButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_SelectDateButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CopyButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_PasteButton", Type = typeof(Button))]

    [TemplatePart(Name = "PART_MainGrid", Type = typeof(Grid))]

    [TemplatePart(Name = "PART_AmPmListTextBox", Type = typeof(ListTextBox))]

    public class DateTimePicker1 : Control
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private List<TextBox> _textBoxes;
        private DateTime _todayValue;
        private DateTimeFormatInfo _formatInfo;
        private bool _hideTime;
        private bool _applyingFormat;

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
        
        private ToggleButton _datePickerIconToggleButton;

        private Button _todayButton;
        private Button _nowButton;
        private Button _selectDateButton;
        private Button _clearButton;
        private Button _copyButton;
        private Button _pasteButton;

        private Grid _mainGrid;

        private ListTextBox _ampmListTextBox;

        #region Dependency properties
        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(DateTime?),
            typeof(DateTimePicker1), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((DateTimePicker1)sender).OnValueChanged((DateTime?)e.OldValue, (DateTime?) e.NewValue)));

        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register(nameof(ShowOptionsButton), typeof(bool),
            typeof(DateTimePicker1), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, args) => ((DateTimePicker1)sender).OnShowOptionsButtonChanged(args)));

        public bool AllowNull
        {
            get { return (bool)GetValue(AllowNullProperty); }
            set { SetValue(AllowNullProperty, value); }
        }

        public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register(nameof(AllowNull), typeof(bool),
            typeof(DateTimePicker1), new PropertyMetadata(false));

        public bool AllowCopyPaste
        {
            get { return (bool)GetValue(AllowCopyPasteProperty); }
            set { SetValue(AllowCopyPasteProperty, value); }
        }

        public static readonly DependencyProperty AllowCopyPasteProperty = DependencyProperty.Register(nameof(AllowCopyPaste), typeof(bool),
            typeof(DateTimePicker1), new PropertyMetadata(true));

        public bool HideTime
        {
            get { return (bool)GetValue(HideTimeProperty); }
            set { SetValue(HideTimeProperty, value); }
        }

        public static readonly DependencyProperty HideTimeProperty = DependencyProperty.Register(nameof(HideTime), typeof(bool),
            typeof(DateTimePicker1), new FrameworkPropertyMetadata(false, 
                (sender, e) => ((DateTimePicker1)sender).OnHideTimeChanged()));

        public bool HideSeconds
        {
            get { return (bool)GetValue(HideSecondsProperty); }
            set { SetValue(HideSecondsProperty, value); }
        }

        public static readonly DependencyProperty HideSecondsProperty = DependencyProperty.Register(nameof(HideSeconds), typeof(bool),
            typeof(DateTimePicker1), new FrameworkPropertyMetadata(false, (sender, e) => ((DateTimePicker1)sender).OnHideSecondsChanged()));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool),
            typeof(DateTimePicker1), new PropertyMetadata(false));

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string),
            typeof(DateTimePicker1), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, (sender, e) => ((DateTimePicker1)sender).OnFormatChanged()));

        public bool IsYearShortFormat
        {
            get { return (bool)GetValue(IsYearShortFormatProperty); }
            private set { SetValue(IsYearShortFormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsYearShortFormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsYearShortFormat), typeof(bool),
            typeof(DateTimePicker1), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Count(x => x == 'y') < 3));

        public static readonly DependencyProperty IsYearShortFormatProperty = IsYearShortFormatPropertyKey.DependencyProperty;

        public bool IsHour12Format
        {
            get { return (bool)GetValue(IsHour12FormatProperty); }
            private set { SetValue(IsHour12FormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsHour12FormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsHour12Format), typeof(bool),
            typeof(DateTimePicker1), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Contains("h")));

        public static readonly DependencyProperty IsHour12FormatProperty = IsHour12FormatPropertyKey.DependencyProperty;

        public bool IsAmPmShortFormat
        {
            get { return (bool)GetValue(IsAmPmShortFormatProperty); }
            private set { SetValue(IsAmPmShortFormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsAmPmShortFormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsAmPmShortFormat), typeof(bool),
            typeof(DateTimePicker1), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Count(x => x == 't') < 2));

        public static readonly DependencyProperty IsAmPmShortFormatProperty = IsAmPmShortFormatPropertyKey.DependencyProperty;
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            /*Days numeric text box*/
            _daysNumericTextBox = GetTemplateChild("PART_DaysNumericTextBox") as NumericTextBox;
            if (_daysNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_DaysNumericTextBox'");
            }
            _daysNumericTextBox.ValueChanged += OnDaysValueChanged;

            /*Month numeric text box*/
            _monthNumericTextBox = GetTemplateChild("PART_MonthNumericTextBox") as NumericTextBox;
            if (_monthNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MonthNumericTextBox'");
            }
            _monthNumericTextBox.ValueChanged += OnMonthValueChanged;
            _monthNumericTextBox.TextChanged += OnMonthTextChanged;

            /*Year numeric text box*/
            _yearNumericTextBox = GetTemplateChild("PART_YearNumericTextBox") as NumericTextBox;
            if (_yearNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_YearNumericTextBox'");
            }
            _yearNumericTextBox.ValueChanged += OnYearValueChanged;

            /*Hour numeric text box*/
            _hourNumericTextBox = GetTemplateChild("PART_HourNumericTextBox") as NumericTextBox;
            if (_hourNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_HourNumericTextBox'");
            }
            _hourNumericTextBox.ValueChanged += OnHourValueChanged;

            /*Hour numeric text box*/
            _minuteNumericTextBox = GetTemplateChild("PART_MinuteNumericTextBox") as NumericTextBox;
            if (_minuteNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MinuteNumericTextBox'");
            }
            _minuteNumericTextBox.ValueChanged += OnMinuteValueChanged;

            _secondNumericTextBox = GetTemplateChild("PART_SecondNumericTextBox") as NumericTextBox;
            if (_secondNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_SecondNumericTextBox'");
            }
            _secondNumericTextBox.ValueChanged += OnSecondValueChanged;

            /*Separators*/
            _daysMonthsSeparatorTextBlock = GetTemplateChild("PART_DaysMonthsSeparatorTextBlock") as TextBlock;
            if (_daysMonthsSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_DaysMonthsSeparatorTextBlock'");
            }

            _monthsYearSeparatorTextBlock = GetTemplateChild("PART_MonthsYearSeparatorTextBlock") as TextBlock;
            if (_monthsYearSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MonthsYearSeparatorTextBlock'");
            }

            _yearSeparatorTextBlock = GetTemplateChild("PART_YearSeparatorTextBlock") as TextBlock;
            if (_yearSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_YearSeparatorTextBlock'");
            }

            _hourMinuteSeparatorTextBlock = GetTemplateChild("PART_HourMinuteSeparatorTextBlock") as TextBlock;
            if (_hourMinuteSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_HourMinuteSeparatorTextBlock'");
            }

            _minuteSecondSeparatorTextBlock = GetTemplateChild("PART_MinuteSecondSeparatorTextBlock") as TextBlock;
            if (_minuteSecondSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MinuteSecondSeparatorTextBlock'");
            }

            _secondAmPmSeparatorTextBlock = GetTemplateChild("PART_SecondAmPmSeparatorTextBlock") as TextBlock;
            if (_secondAmPmSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_SecondAmPmSeparatorTextBlock'");
            }

            _amPmSeparatorTextBlock = GetTemplateChild("PART_AmPmSeparatorTextBlock") as TextBlock;
            if (_amPmSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_AmPmSeparatorTextBlock'");
            }

            _ampmListTextBox = GetTemplateChild("PART_AmPmListTextBox") as ListTextBox;
            if (_ampmListTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_AmPmListTextBox'");
            }

            /*Toggles*/
            _daysToggleButton = GetTemplateChild("PART_DaysToggleButton") as ToggleButton;
            if (_daysToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_DaysToggleButton'");
            }
            _daysToggleButton.Checked += OnToggleButtonChecked;

            _monthToggleButton = GetTemplateChild("PART_MonthToggleButton") as ToggleButton;
            if (_monthToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MonthToggleButton'");
            }
            _monthToggleButton.Checked += OnToggleButtonChecked;

            _yearToggleButton = GetTemplateChild("PART_YearToggleButton") as ToggleButton;
            if (_yearToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_YearToggleButton'");
            }

            _hourToggleButton = GetTemplateChild("PART_HourToggleButton") as ToggleButton;
            if (_hourToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_HourToggleButton'");
            }
            _hourToggleButton.Checked += OnToggleButtonChecked;

            _minuteToggleButton = GetTemplateChild("PART_MinuteToggleButton") as ToggleButton;
            if (_minuteToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MinuteToggleButton'");
            }
            _minuteToggleButton.Checked += OnToggleButtonChecked;

            _secondToggleButton = GetTemplateChild("PART_SecondToggleButton") as ToggleButton;
            if (_secondToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_SecondToggleButton'");
            }
            _secondToggleButton.Checked += OnToggleButtonChecked;

            _amPmToggleButton = GetTemplateChild("PART_AmPmToggleButton") as ToggleButton;
            if (_amPmToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_AmPmToggleButton'");
            }
            _amPmToggleButton.Checked += OnToggleButtonChecked;



            _datePickerIconToggleButton = GetTemplateChild("PART_DatePickerIconToggleButton") as ToggleButton;
            if (_datePickerIconToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_DatePickerIconToggleButton'");
            }
            _datePickerIconToggleButton.Visibility = ShowOptionsButton ? Visibility.Visible : Visibility.Hidden;

            /*Buttons*/
            _todayButton = GetTemplateChild("PART_TodayButton") as Button;
            if (_todayButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_TodayButton'");
            }
            _todayButton.Click += OnTodayButtonClick;     
            
            _nowButton = GetTemplateChild("PART_NowButton") as Button;
            if (_nowButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_NowButton'");
            }
            _nowButton.Click += OnNowButtonClick;

            _selectDateButton = GetTemplateChild("PART_SelectDateButton") as Button;
            if (_selectDateButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_SelectDateButton'");
            }
            _selectDateButton.Click += OnSelectDateButtonClick;

            _clearButton = GetTemplateChild("PART_ClearButton") as Button;
            if (_clearButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_ClearButton'");
            }
            _clearButton.Click += OnClearButtonClick;

            _copyButton = GetTemplateChild("PART_CopyButton") as Button;
            if (_copyButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_CopyButton'");
            }
            _copyButton.Click += OnCopyButtonClick;

            _pasteButton = GetTemplateChild("PART_PasteButton") as Button;
            if (_pasteButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_PasteButton'");
            }
            _pasteButton.Click += OnPasteButtonClick;

            /*Main grid*/
            _mainGrid = GetTemplateChild("PART_MainGrid") as Grid;
            if (_mainGrid is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MainGrid'");
            }


            _textBoxes = new List<TextBox>
            {
                _daysNumericTextBox,
                _monthNumericTextBox,
                _yearNumericTextBox,
                _hourNumericTextBox,
                _minuteNumericTextBox,
                _secondNumericTextBox,
                _ampmListTextBox
            };

            var now = DateTime.Now;
            _todayValue = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            ApplyFormat();

            UpdateUi();
        }

        private void OnTodayButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
            UpdateDateTime(DateTime.Today.Date);
        }

        private void OnNowButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
            UpdateDateTime(DateTime.Now);
        }

        private void OnSelectDateButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            var calendarPopup = CreateCalendarPopup();
            var calendarPopupSource = CreateCalendarPopupSource();
            calendarPopup.SetCurrentValue(Popup.ChildProperty, calendarPopupSource);

            calendarPopupSource.Focus();
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
            UpdateDateTime(null);
        }

        private void OnCopyButtonClick(object sender, RoutedEventArgs e)
        {
            _datePickerIconToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            var value = Value;
            if (value != null)
            {
                Clipboard.SetText(DateTimeFormatter.Format(value.Value, _formatInfo), TextDataFormat.Text);
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
                    || DateTime.TryParseExact(text, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, null, DateTimeStyles.None, out value)
                    || DateTime.TryParseExact(text, CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.InvariantCulture.DateTimeFormat.LongTimePattern, null, DateTimeStyles.None, out value)))
            {
                SetCurrentValue(ValueProperty, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second));
            }
        }

        private void OnFormatChanged()
        {
            ApplyFormat();
        }

        private void ApplyFormat()
        {
            _applyingFormat = true;

            try
            {
                var format = Format;
                _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(format);
                var hasLongTimeFormat = !(_formatInfo.HourFormat is null
                                       || _formatInfo.MinuteFormat is null
                                       || _formatInfo.SecondFormat is null);

                var hasAnyTimeFormat = !(_formatInfo.HourFormat is null
                                   && _formatInfo.MinuteFormat is null
                                   && _formatInfo.SecondFormat is null);

                SetCurrentValue(HideTimeProperty, !hasAnyTimeFormat || _hideTime);

                if (!hasLongTimeFormat)
                {
                    var timePattern = DateTimeFormatHelper.ExtractTimePatternFromFormat(format);
                    if (!string.IsNullOrEmpty(timePattern))
                    {
                        timePattern = DateTimeFormatHelper.FindMatchedLongTimePattern(CultureInfo.CurrentUICulture, timePattern);
                    }

                    if (string.IsNullOrEmpty(timePattern))
                    {
                        timePattern = CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern;
                    }

                    var datePattern = DateTimeFormatHelper.ExtractDatePatternFromFormat(format);

                    format = $"{datePattern} {timePattern}";

                    _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(format);
                }

                if (!hasAnyTimeFormat)
                { }

                IsYearShortFormat = _formatInfo.IsYearShortFormat;
                _yearNumericTextBox.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(_formatInfo.IsYearShortFormat ? 0 : 1));
                _yearNumericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(_formatInfo.IsYearShortFormat ? 99 : 3000));

                var isHour12Format = _formatInfo.IsHour12Format ?? true;
                IsHour12Format = isHour12Format;
                _hourNumericTextBox.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(isHour12Format ? 1 : 0));
                _hourNumericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(isHour12Format ? 12 : 23));
                _hourToggleButton.SetCurrentValue(TagProperty, isHour12Format ? DateTimePart.Hour12 : DateTimePart.Hour);

                IsAmPmShortFormat = _formatInfo.IsAmPmShortFormat.Value;

                EnableOrDisableYearConverterDependingOnFormat();
                EnableOrDisableHourConverterDependingOnFormat();
                EnableOrDisableAmPmConverterDependingOnFormat();

                _ampmListTextBox.SetCurrentValue(ListTextBox.ListOfValuesProperty, new List<string>
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
                Grid.SetColumn(_hourNumericTextBox, GetPosition(_formatInfo.HourPosition.Value));
                Grid.SetColumn(_minuteNumericTextBox, GetPosition(_formatInfo.MinutePosition.Value));
                Grid.SetColumn(_secondNumericTextBox, GetPosition(_formatInfo.SecondPosition ?? _defaultSecondFormatPosition));
                Grid.SetColumn(_ampmListTextBox, GetPosition(_formatInfo.AmPmPosition ?? _defaultAmPmFormatPosition));

                Grid.SetColumn(_daysToggleButton, GetPosition(_formatInfo.DayPosition) + 1);
                Grid.SetColumn(_monthToggleButton, GetPosition(_formatInfo.MonthPosition) + 1);
                Grid.SetColumn(_yearToggleButton, GetPosition(_formatInfo.YearPosition) + 1);
                Grid.SetColumn(_hourToggleButton, GetPosition(_formatInfo.HourPosition.Value) + 1);
                Grid.SetColumn(_minuteToggleButton, GetPosition(_formatInfo.MinutePosition.Value) + 1);
                Grid.SetColumn(_secondToggleButton, GetPosition(_formatInfo.SecondPosition ?? _defaultSecondFormatPosition) + 1);

                //hide parts according to Hide options and format
                var isHiddenManual = HideSeconds || HideTime;

                _secondNumericTextBox.SetCurrentValue(NumericTextBox.VisibilityProperty, _formatInfo.SecondFormat is null || isHiddenManual ? Visibility.Collapsed : Visibility.Visible);
                _secondToggleButton.SetCurrentValue(NumericTextBox.VisibilityProperty, _formatInfo.SecondFormat is null || isHiddenManual ? Visibility.Collapsed : Visibility.Visible);

                Grid.SetColumn(_amPmToggleButton, GetPosition((_formatInfo.AmPmPosition ?? _defaultAmPmFormatPosition) + 1));

                // Fix positions which could be broken, because of AM/PM textblock.
                // Fix for seconds in a same way
                int dayPos = _formatInfo.DayPosition, monthPos = _formatInfo.MonthPosition, yearPos = _formatInfo.YearPosition,
                    hourPos = _formatInfo.HourPosition.Value, minutePos = _formatInfo.MinutePosition.Value, secondPos = _formatInfo.SecondPosition ?? 5,
                    amPmPos = _formatInfo.AmPmPosition ?? 6;
                FixNumericTextBoxesPositions(ref dayPos, ref monthPos, ref yearPos, ref hourPos, ref minutePos, ref secondPos, ref amPmPos);

                _textBoxes[dayPos] = _daysNumericTextBox;
                _textBoxes[monthPos] = _monthNumericTextBox;
                _textBoxes[yearPos] = _yearNumericTextBox;
                _textBoxes[hourPos] = _hourNumericTextBox;
                _textBoxes[minutePos] = _minuteNumericTextBox;
                _textBoxes[secondPos] = _secondNumericTextBox;
                _textBoxes[amPmPos] = _ampmListTextBox;

                // Fix tab order inside control.
                _daysNumericTextBox.SetCurrentValue(TabIndexProperty, dayPos);
                _monthNumericTextBox.SetCurrentValue(TabIndexProperty, monthPos);
                _yearNumericTextBox.SetCurrentValue(TabIndexProperty, yearPos);
                _hourNumericTextBox.SetCurrentValue(TabIndexProperty, hourPos);
                _minuteNumericTextBox.SetCurrentValue(TabIndexProperty, minutePos);
                _secondNumericTextBox.SetCurrentValue(TabIndexProperty, secondPos);
                _ampmListTextBox.SetCurrentValue(TabIndexProperty, amPmPos);
                _datePickerIconToggleButton.SetCurrentValue(TabIndexProperty, int.MaxValue);

                SubscribeNumericTextBoxes();

                _daysMonthsSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator1);
                _monthsYearSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator2);
                _yearSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator3);
                _hourMinuteSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator4);
                _minuteSecondSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator5);
                _secondAmPmSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator6);
                _amPmSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator7);
            }
            finally
            {
                _applyingFormat = false;
            }

        }

        private void OnToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            var activeDateTimePart = (DateTimePart)((ToggleButton)sender).Tag;

            var activeTextBox = (TextBox)FindName(activeDateTimePart.GetDateTimePartName());
            var activeToggleButton = (ToggleButton)FindName(activeDateTimePart.GetDateTimePartToggleButtonName());

            var dateTime = Value ?? _todayValue;
            var dateTimePartHelper = new DateTimePartHelper(dateTime, activeDateTimePart, _formatInfo, activeTextBox, activeToggleButton);
            dateTimePartHelper.CreatePopup();
        }

        private void OnValueChanged(DateTime? oldValue, DateTime? newValue)
        {
            var ov = oldValue;
            var nv = newValue;

            if (!AllowNull && newValue == null)
            {
                var dateTime = DateTime.Now;
                nv = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
            }

            if (ov == null && nv != null || ov != null && nv == null)
            {
                ApplyFormat();
            }

            if (newValue == null && nv != null)
            {
               // Dispatcher.BeginInvoke(() => SetCurrentValue(ValueProperty, nv));
            }
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
            if (date != null)
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

        }


        private void OnDaysValueChanged(object sender, EventArgs e)
        {

        }

        private void OnMonthValueChanged(object sender, EventArgs e)
        {
         
        }

        private void OnYearValueChanged(object sender, EventArgs e)
        {
           
        }

        private void OnHourValueChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMinuteValueChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnSecondValueChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

        private void OnShowOptionsButtonChanged(DependencyPropertyChangedEventArgs args)
        {
            _datePickerIconToggleButton.Visibility = ShowOptionsButton ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnHideTimeChanged()
        {
            if (!_applyingFormat)
            {
                _hideTime = HideTime;
            }

            ApplyFormat();
        }

        private void OnHideSecondsChanged()
        {
            ApplyFormat();
        }

        private void OnTextBoxLeftBoundReached(object sender, EventArgs e)
        {

        }

        private void OnTextBoxRightBoundReached(object sender, EventArgs eventArgs)
        {

        }

        private void SubscribeNumericTextBoxes()
        {
            // Enable support for switching between textboxes,
            // 0-5 because we can't switch to right on last textbox.
            for (var i = 0; i <= 5; i++)
            {
                _textBoxes[i].SubscribeToOnRightBoundReachedEvent(OnTextBoxRightBoundReached);
            }

            // Enable support for switching between textboxes,
            // 5-1 because we can't switch to left on first textbox.
            for (var i = 6; i >= 1; i--)
            {
                _textBoxes[i].SubscribeToOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
            }
        }

        private void UnsubscribeNumericTextBoxes()
        {
            // Disable support for switching between textboxes,
            // 0-4 because we can't switch to right on last textbox.
            for (var i = 0; i <= 5; i++)
            {
                _textBoxes[i].UnsubscribeFromOnRightBoundReachedEvent(OnTextBoxRightBoundReached);
            }

            // Disable support for switching between textboxes,
            // 5-1 because we can't switch to left on first textbox.
            for (var i = 6; i >= 1; i--)
            {
                _textBoxes[i].UnsubscribeFromOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
            }
        }

        private void EnableOrDisableHourConverterDependingOnFormat()
        {
            if (TryFindResource(nameof(Hour24ToHour12Converter)) is Hour24ToHour12Converter converter)
            {
                converter.IsEnabled = IsHour12Format;
                BindingOperations.GetBindingExpression(_hourNumericTextBox, NumericTextBox.ValueProperty)?.UpdateTarget();
            }
        }

        private void EnableOrDisableAmPmConverterDependingOnFormat()
        {
            if (TryFindResource(nameof(AmPmLongToAmPmShortConverter)) is AmPmLongToAmPmShortConverter converter)
            {
                converter.IsEnabled = IsAmPmShortFormat;
                BindingOperations.GetBindingExpression(_ampmListTextBox, ListTextBox.ValueProperty)?.UpdateTarget();
            }
        }

        private void EnableOrDisableYearConverterDependingOnFormat()
        {
            if (TryFindResource(nameof(YearLongToYearShortConverter)) is YearLongToYearShortConverter converter)
            {
                converter.IsEnabled = IsYearShortFormat;
                BindingOperations.GetBindingExpression(_ampmListTextBox, NumericTextBox.ValueProperty)?.UpdateTarget();
            }
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

            calendar.PreviewKeyDown += CalendarOnPreviewKeyDown;
            calendar.SelectedDatesChanged += CalendarOnSelectedDatesChanged;

            return calendar;
        }

        private void CalendarOnSelectedDatesChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var calendar = (((Calendar)sender));
            if (AllowNull || calendar.SelectedDate.HasValue)
            {
                UpdateDate(calendar.SelectedDate);
            }

            ((Popup)calendar.Parent).SetCurrentValue(Popup.IsOpenProperty, false);
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
                return;
            }

            if (calendar.SelectedDate.HasValue)
            {
                UpdateDate(calendar.SelectedDate.Value);
            }

            ((Popup)calendar.Parent).SetCurrentValue(Popup.IsOpenProperty, false);

            e.Handled = true;
        }
        #endregion
    }
}
