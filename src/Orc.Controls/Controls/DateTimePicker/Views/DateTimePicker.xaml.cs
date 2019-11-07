// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePicker.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
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
    using System.Windows.Media;
    using Catel.MVVM.Views;
    using Catel.Windows.Threading;
    using Calendar = System.Windows.Controls.Calendar;
    using Converters;

    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker
    {
        #region Fields
        private readonly List<TextBox> _textBoxes;
        private readonly DateTime _todayValue;

        private DateTimePart _activeDateTimePart;
        private DateTimeFormatInfo _formatInfo;
        #endregion

        #region Constructors
        static DateTimePicker()
        {
            typeof(DateTimePicker).AutoDetectViewPropertiesToSubscribe();

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
        }

        public DateTimePicker()
        {
            InitializeComponent();

            _textBoxes = new List<TextBox>()
            {
                NumericTBDay,
                NumericTBMonth,
                NumericTBYear,
                NumericTBHour,
                NumericTBMinute,
                NumericTBSecond,
                ListTBAmPm
            };

            var now = DateTime.Now;
            _todayValue = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
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
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(DateTime?),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((DateTimePicker)sender).OnValueChanged(e.OldValue, e.NewValue)));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register(nameof(ShowOptionsButton), typeof(bool),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use AccentColorBrush markup extension instead")]
        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use AccentColorBrush markup extension instead")]
        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register(nameof(AccentColorBrush), typeof(Brush),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((DateTimePicker)sender).OnAccentColorBrushChanged()));

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

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool HideTime
        {
            get { return (bool)GetValue(HideTimeProperty); }
            set { SetValue(HideTimeProperty, value); }
        }

        public static readonly DependencyProperty HideTimeProperty = DependencyProperty.Register(nameof(HideTime), typeof(bool),
            typeof(DateTimePicker), new FrameworkPropertyMetadata(false, (sender, e) => ((DateTimePicker)sender).OnHideTimeChanged()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
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
        #endregion

        #region Methods
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

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            _activeDateTimePart = (DateTimePart)((ToggleButton)sender).Tag;

            var activeTextBox = (TextBox)FindName(_activeDateTimePart.GetDateTimePartName());
            var activeToggleButton = (ToggleButton)FindName(_activeDateTimePart.GetDateTimePartToggleButtonName());

            var dateTime = Value ?? _todayValue;
            var dateTimePartHelper = new DateTimePartHelper(dateTime, _activeDateTimePart, _formatInfo, activeTextBox, activeToggleButton);
            dateTimePartHelper.CreatePopup();
        }

        private void NumericTBMonth_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(NumericTBMonth.Text, out var month))
            {
                return;
            }

            if (!int.TryParse(NumericTBYear.Text, out var year))
            {
                return;
            }

            var daysInMonth = DateTime.DaysInMonth(year, month);
            NumericTBDay.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)daysInMonth);
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
                NumericTBDay.Focus();
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

        private void UpdateDateTime(DateTime? dateTime)
        {
            if (!AllowNull && !dateTime.HasValue)
            {
                dateTime = _todayValue;
            }

            SetCurrentValue(ValueProperty, dateTime);
        }

        private Popup CreateCalendarPopup()
        {
            var popup = new Popup
            {
                PlacementTarget = MainGrid,
                Placement = PlacementMode.Bottom,
                VerticalOffset = -4,
                IsOpen = true,
                StaysOpen = false,
            };

            return popup;
        }

        private void OnTodayButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
            UpdateDateTime(DateTime.Today.Date);
        }

        private void OnNowButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
            UpdateDateTime(DateTime.Now);
        }

        private void OnSelectDateButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            var calendarPopup = CreateCalendarPopup();
            var calendarPopupSource = CreateCalendarPopupSource();
            calendarPopup.SetCurrentValue(Popup.ChildProperty, calendarPopupSource);

            calendarPopupSource.Focus();
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
            UpdateDateTime(null);
        }

        private void OnCopyButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            var value = Value;
            if (value != null)
            {
                Clipboard.SetText(DateTimeFormatter.Format(value.Value, _formatInfo), TextDataFormat.Text);
            }
        }

        private void OnPasteButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

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

        private void OnAccentColorBrushChanged()
        {
            if (AccentColorBrush is SolidColorBrush brush)
            {
                var accentColor = brush.Color;
                accentColor.CreateAccentColorResourceDictionary(nameof(DateTimePicker));
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetCurrentValue(AccentColorBrushProperty, TryFindResource("AccentColorBrush") as SolidColorBrush);
        }

        protected override void OnLoaded(EventArgs e)
        {
            // Ensure that we have a template.
            ApplyTemplate();

            ApplyFormat();
            base.OnLoaded(e);
        }

        protected override void OnUnloaded(EventArgs e)
        {
            base.OnUnloaded(e);
            UnsubscribeNumericTextBoxes();
        }

        private void OnFormatChanged()
        {
            ApplyFormat();
        }

        private void OnValueChanged(object oldValue, object newValue)
        {
            var ov = oldValue as DateTime?;
            var nv = newValue as DateTime?;

            if (!AllowNull && newValue == null)
            {
                var dateTime = DateTime.Now;
                nv = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
            }

            if ((ov == null && nv != null)
                || (ov != null && nv == null))
            {
                ApplyFormat();
            }

            if (newValue == null && nv != null)
            {
                Dispatcher.BeginInvoke(() => SetCurrentValue(ValueProperty, nv));
            }
        }

        private void OnHideTimeChanged()
        {
            ApplyFormat();
        }

        private void OnHideSecondsChanged()
        {
            ApplyFormat();
        }

        private void ApplyFormat()
        {
            var format = Format;
            _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(format);
            var hasLongTimeFormat = !(_formatInfo.HourFormat is null
                                   || _formatInfo.MinuteFormat is null
                                   || _formatInfo.SecondFormat is null);

            var hasAnyTimeFormat = !(_formatInfo.HourFormat is null
                               && _formatInfo.MinuteFormat is null
                               && _formatInfo.SecondFormat is null);


            if (!hasAnyTimeFormat)
            {
                HideTime = true;
            }

            if (hasAnyTimeFormat && !hasLongTimeFormat)
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

            IsYearShortFormat = _formatInfo.IsYearShortFormat;
            NumericTBYear.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(_formatInfo.IsYearShortFormat ? 0 : 1));
            NumericTBYear.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(_formatInfo.IsYearShortFormat ? 99 : 3000));

            var isHour12Format = _formatInfo.IsHour12Format??true;
            IsHour12Format = isHour12Format;
            NumericTBHour.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(isHour12Format ? 1 : 0));
            NumericTBHour.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(isHour12Format ? 12 : 23));
            ToggleButtonH.SetCurrentValue(TagProperty, isHour12Format ? DateTimePart.Hour12 : DateTimePart.Hour);

            IsAmPmShortFormat = _formatInfo.IsAmPmShortFormat.Value;

            EnableOrDisableYearConverterDependingOnFormat();
            EnableOrDisableHourConverterDependingOnFormat();
            EnableOrDisableAmPmConverterDependingOnFormat();

            ListTBAmPm.SetCurrentValue(ListTextBox.ListOfValuesProperty, new List<string>()
            {
                Meridiems.GetAmForFormat(_formatInfo),
                Meridiems.GetPmForFormat(_formatInfo)
            });

            NumericTBDay.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.DayFormat.Length));
            NumericTBMonth.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.MonthFormat.Length));
            NumericTBYear.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.YearFormat.Length));
            NumericTBHour.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.HourFormat.Length));
            NumericTBMinute.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.MinuteFormat.Length));
            NumericTBSecond.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.SecondFormat.Length));

            UnsubscribeNumericTextBoxes();

            Grid.SetColumn(NumericTBDay, GetPosition(_formatInfo.DayPosition));
            Grid.SetColumn(NumericTBMonth, GetPosition(_formatInfo.MonthPosition));
            Grid.SetColumn(NumericTBYear, GetPosition(_formatInfo.YearPosition));
            Grid.SetColumn(NumericTBHour, GetPosition(_formatInfo.HourPosition.Value));
            Grid.SetColumn(NumericTBMinute, GetPosition(_formatInfo.MinutePosition.Value));
            Grid.SetColumn(NumericTBSecond, GetPosition(_formatInfo.SecondPosition.Value));
            Grid.SetColumn(ListTBAmPm, GetPosition(_formatInfo.AmPmPosition.HasValue == false ? 6 : _formatInfo.AmPmPosition.Value));

            Grid.SetColumn(ToggleButtonD, GetPosition(_formatInfo.DayPosition) + 1);
            Grid.SetColumn(ToggleButtonMo, GetPosition(_formatInfo.MonthPosition) + 1);
            Grid.SetColumn(ToggleButtonY, GetPosition(_formatInfo.YearPosition) + 1);
            Grid.SetColumn(ToggleButtonH, GetPosition(_formatInfo.HourPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonM, GetPosition(_formatInfo.MinutePosition.Value) + 1);
            Grid.SetColumn(ToggleButtonS, GetPosition(_formatInfo.SecondPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonT, GetPosition(_formatInfo.AmPmPosition.HasValue == false ? 6 : _formatInfo.AmPmPosition.Value) + 1);

            // Fix positions which could be broken, because of AM/PM textblock.
            int dayPos = _formatInfo.DayPosition, monthPos = _formatInfo.MonthPosition, yearPos = _formatInfo.YearPosition,
                hourPos = _formatInfo.HourPosition.Value, minutePos = _formatInfo.MinutePosition.Value, secondPos = _formatInfo.SecondPosition.Value,
                amPmPos = _formatInfo.AmPmPosition.HasValue == false ? 6 : _formatInfo.AmPmPosition.Value;
            FixNumericTextBoxesPositions(ref dayPos, ref monthPos, ref yearPos, ref hourPos, ref minutePos, ref secondPos, ref amPmPos);

            _textBoxes[dayPos] = NumericTBDay;
            _textBoxes[monthPos] = NumericTBMonth;
            _textBoxes[yearPos] = NumericTBYear;
            _textBoxes[hourPos] = NumericTBHour;
            _textBoxes[minutePos] = NumericTBMinute;
            _textBoxes[secondPos] = NumericTBSecond;
            _textBoxes[amPmPos] = ListTBAmPm;

            // Fix tab order inside control.
            NumericTBDay.SetCurrentValue(TabIndexProperty, dayPos);
            NumericTBMonth.SetCurrentValue(TabIndexProperty, monthPos);
            NumericTBYear.SetCurrentValue(TabIndexProperty, yearPos);
            NumericTBHour.SetCurrentValue(TabIndexProperty, hourPos);
            NumericTBMinute.SetCurrentValue(TabIndexProperty, minutePos);
            NumericTBSecond.SetCurrentValue(TabIndexProperty, secondPos);
            ListTBAmPm.SetCurrentValue(TabIndexProperty, amPmPos);
            DatePickerIcon.SetCurrentValue(TabIndexProperty, Int32.MaxValue);

            SubscribeNumericTextBoxes();

            Separator1.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator1);
            Separator2.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator2);
            Separator3.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator3);
            Separator4.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator4);
            Separator5.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator5);
            Separator6.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator6);
            Separator7.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator7);
        }

        private int GetPosition(int index)
        {
            return index * 2;
        }

        private void EnableOrDisableHourConverterDependingOnFormat()
        {
            if (TryFindResource(nameof(Hour24ToHour12Converter)) is Hour24ToHour12Converter converter)
            {
                converter.IsEnabled = IsHour12Format;
                BindingOperations.GetBindingExpression(NumericTBHour, NumericTextBox.ValueProperty)?.UpdateTarget();
            }
        }

        private void EnableOrDisableAmPmConverterDependingOnFormat()
        {
            if (TryFindResource(nameof(AmPmLongToAmPmShortConverter)) is AmPmLongToAmPmShortConverter converter)
            {
                converter.IsEnabled = IsAmPmShortFormat;
                BindingOperations.GetBindingExpression(ListTBAmPm, ListTextBox.ValueProperty)?.UpdateTarget();
            }
        }

        private void EnableOrDisableYearConverterDependingOnFormat()
        {
            if (TryFindResource(nameof(YearLongToYearShortConverter)) is YearLongToYearShortConverter converter)
            {
                converter.IsEnabled = IsYearShortFormat;
                BindingOperations.GetBindingExpression(NumericTBYear, NumericTextBox.ValueProperty)?.UpdateTarget();
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
        #endregion
    }
}
