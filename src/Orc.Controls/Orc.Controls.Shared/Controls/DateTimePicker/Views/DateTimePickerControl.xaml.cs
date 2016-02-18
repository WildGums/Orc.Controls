// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
    using Calendar = System.Windows.Controls.Calendar;
    using Converters;

    /// <summary>
    /// Interaction logic for DateTimePickerControl.xaml
    /// </summary>
    public partial class DateTimePickerControl
    {
        #region Fields
        private readonly List<NumericTextBox> _numericTextBoxes;
        private DateTimePart _activeDateTimePart;
        #endregion

        #region Constructors
        static DateTimePickerControl()
        {
            typeof(DateTimePickerControl).AutoDetectViewPropertiesToSubscribe();
        }

        public DateTimePickerControl()
        {
            InitializeComponent();

            _numericTextBoxes = new List<NumericTextBox>()
            {
                NumericTBDay,
                NumericTBMonth,
                NumericTBYear,
                NumericTBHour,
                NumericTBMinute,
                NumericTBSecond,
            };

            SubscribeNumericTextBoxes();

            ApplyFormat();
        }

        private void SubscribeNumericTextBoxes()
        {
            _numericTextBoxes[0].RightBoundReached += NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[1].RightBoundReached += NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[2].RightBoundReached += NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[3].RightBoundReached += NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[4].RightBoundReached += NumericTextBoxOnRightBoundReached;

            _numericTextBoxes[5].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[4].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[3].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[2].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[1].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
        }

        private void UnsubscribeNumericTextBoxes()
        {
            _numericTextBoxes[0].RightBoundReached -= NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[1].RightBoundReached -= NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[2].RightBoundReached -= NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[3].RightBoundReached -= NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[4].RightBoundReached -= NumericTextBoxOnRightBoundReached;

            _numericTextBoxes[5].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[4].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[3].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[2].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[1].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime),
            typeof(DateTimePickerControl), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register("ShowOptionsButton", typeof(bool),
            typeof(DateTimePickerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof(Brush),
            typeof(DateTimePickerControl), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((DateTimePickerControl)sender).OnAccentColorBrushChanged()));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool),
            typeof(DateTimePickerControl), new PropertyMetadata(false));

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string),
            typeof(DateTimePickerControl), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, (sender, e) => ((DateTimePickerControl)sender).OnFormatChanged()));

        public bool IsHour12Format
        {
            get { return (bool)GetValue(IsHour12FormatProperty); }
            private set { SetValue(IsHour12FormatKey, value); }
        }

        private static readonly DependencyPropertyKey IsHour12FormatKey = DependencyProperty.RegisterReadOnly("IsHour12Format", typeof(bool),
            typeof(DateTimePickerControl), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Contains("h") ? true : false));

        public static readonly DependencyProperty IsHour12FormatProperty = IsHour12FormatKey.DependencyProperty;

        #endregion

        #region Methods
        private void NumericTextBoxOnLeftBoundReached(object sender, EventArgs e)
        {
            var currentTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var prevTextBox = _numericTextBoxes[currentTextBoxIndex - 1];

            prevTextBox.CaretIndex = prevTextBox.Text.Length;
            prevTextBox.Focus();
        }

        private void NumericTextBoxOnRightBoundReached(object sender, EventArgs eventArgs)
        {
            var currentTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var nextTextBox = _numericTextBoxes[currentTextBoxIndex + 1];

            nextTextBox.CaretIndex = 0;
            nextTextBox.Focus();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            _activeDateTimePart = (DateTimePart)((ToggleButton)sender).Tag;

            var activeNumericTextBox = (NumericTextBox)FindName(_activeDateTimePart.GetDateTimePartName());
            var activeToggleButton = (ToggleButton)FindName(_activeDateTimePart.GetDateTimePartToggleButtonName());

            var dateTimePartHelper = new DateTimePartHelper(Value, _activeDateTimePart, activeNumericTextBox, activeToggleButton);
            dateTimePartHelper.CreatePopup();
        }

        private void NumericTBMonth_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var daysInMonth = DateTime.DaysInMonth(Value.Year, Value.Month);
            NumericTBDay.MaxValue = daysInMonth;
        }

        private Calendar CreateCalendarPopupSource()
        {
            var calendar = new Calendar()
            {
                Margin = new Thickness(0, -3, 0, -3),
                DisplayDate = Value,
                SelectedDate = Value
            };

            calendar.PreviewKeyDown += CalendarOnPreviewKeyDown;
            calendar.SelectedDatesChanged += CalendarOnSelectedDatesChanged;

            return calendar;
        }

        private void CalendarOnSelectedDatesChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var calendar = (((Calendar)sender));
            if (calendar.SelectedDate.HasValue)
            {
                UpdateDate(calendar.SelectedDate.Value);
            }
            ((Popup)calendar.Parent).IsOpen = false;

        }

        private void CalendarOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var calendar = ((Calendar)sender);
            if (e.Key == Key.Escape)
            {
                ((Popup)calendar.Parent).IsOpen = false;
                NumericTBDay.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                if (calendar.SelectedDate.HasValue)
                {
                    UpdateDate(calendar.SelectedDate.Value);
                }
                ((Popup)calendar.Parent).IsOpen = false;

                e.Handled = true;
            }
        }

        private void UpdateDate(DateTime date)
        {
            Value = new DateTime(date.Year, date.Month, date.Day, Value.Hour, Value.Minute, Value.Second);
        }

        private void UpdateDateTime(DateTime date)
        {
            Value = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
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

        private void TodayButton_OnClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.IsChecked = false;
            UpdateDateTime(DateTime.Today.Date);
        }

        private void NowButton_OnClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.IsChecked = false;
            UpdateDateTime(DateTime.Now);
        }

        private void SelectDateButton_OnClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.IsChecked = false;

            var calendarPopup = CreateCalendarPopup();
            var calendarPopupSource = CreateCalendarPopupSource();
            calendarPopup.Child = calendarPopupSource;

            calendarPopupSource.Focus();
        }

        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush)AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("DateTimePicker");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;

            EnableOrDisableHourConverterDependingOnFormat();
        }

        private void OnFormatChanged()
        {
            ApplyFormat();
        }

        private void ApplyFormat()
        {
            var formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(Format, false);

            IsHour12Format = formatInfo.IsHour12Format.Value;
            NumericTBHour.MinValue = formatInfo.IsHour12Format.Value == true ? 1 : 0;
            NumericTBHour.MaxValue = formatInfo.IsHour12Format.Value == true ? 12 : 23;
            ToggleButtonH.Tag = formatInfo.IsHour12Format.Value == true ? DateTimePart.Hour12 : DateTimePart.Hour;

            EnableOrDisableHourConverterDependingOnFormat();

            NumericTBDay.Format = GetFormat(formatInfo.DayFormat.Length);
            NumericTBMonth.Format = GetFormat(formatInfo.MonthFormat.Length);
            NumericTBYear.Format = GetFormat(formatInfo.YearFormat.Length);
            NumericTBHour.Format = GetFormat(formatInfo.HourFormat.Length);
            NumericTBMinute.Format = GetFormat(formatInfo.MinuteFormat.Length);
            NumericTBSecond.Format = GetFormat(formatInfo.SecondFormat.Length);

            UnsubscribeNumericTextBoxes();

            Grid.SetColumn(NumericTBDay, GetPosition(formatInfo.DayPosition));
            Grid.SetColumn(NumericTBMonth, GetPosition(formatInfo.MonthPosition));
            Grid.SetColumn(NumericTBYear, GetPosition(formatInfo.YearPosition));
            Grid.SetColumn(NumericTBHour, GetPosition(formatInfo.HourPosition.Value));
            Grid.SetColumn(NumericTBMinute, GetPosition(formatInfo.MinutePosition.Value));
            Grid.SetColumn(NumericTBSecond, GetPosition(formatInfo.SecondPosition.Value));
            Grid.SetColumn(TextTBAmPm, GetPosition(formatInfo.AmPmPosition.HasValue == false ? 6 : formatInfo.AmPmPosition.Value));

            Grid.SetColumn(ToggleButtonD, GetPosition(formatInfo.DayPosition) + 1);
            Grid.SetColumn(ToggleButtonMo, GetPosition(formatInfo.MonthPosition) + 1);
            Grid.SetColumn(ToggleButtonY, GetPosition(formatInfo.YearPosition) + 1);
            Grid.SetColumn(ToggleButtonH, GetPosition(formatInfo.HourPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonM, GetPosition(formatInfo.MinutePosition.Value) + 1);
            Grid.SetColumn(ToggleButtonS, GetPosition(formatInfo.SecondPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonT, GetPosition(formatInfo.AmPmPosition.HasValue == false ? 6 : formatInfo.AmPmPosition.Value) + 1);

            // Fix positions which could be broken, because of AM/PM textblock.
            int dayPos = formatInfo.DayPosition, monthPos = formatInfo.MonthPosition, yearPos = formatInfo.YearPosition,
                hourPos = formatInfo.HourPosition.Value, minutePos = formatInfo.MinutePosition.Value, secondPos = formatInfo.SecondPosition.Value;
            FixNumericTextBoxesPositions(ref dayPos, ref monthPos, ref yearPos, ref hourPos, ref minutePos, ref secondPos);

            _numericTextBoxes[dayPos] = NumericTBDay;
            _numericTextBoxes[monthPos] = NumericTBMonth;
            _numericTextBoxes[yearPos] = NumericTBYear;
            _numericTextBoxes[hourPos] = NumericTBHour;
            _numericTextBoxes[minutePos] = NumericTBMinute;
            _numericTextBoxes[secondPos] = NumericTBSecond;

            SubscribeNumericTextBoxes();

            Separator1.Text = formatInfo.Separator1;
            Separator2.Text = formatInfo.Separator2;
            Separator3.Text = formatInfo.Separator3;
            Separator4.Text = formatInfo.Separator4;
            Separator5.Text = formatInfo.Separator5;
            Separator6.Text = formatInfo.Separator6;
            Separator7.Text = formatInfo.Separator7;
        }

        private int GetPosition(int index)
        {
            return index * 2;
        }

        private string GetFormat(int digits)
        {
            return new string(Enumerable.Repeat('0', digits).ToArray());
        }

        private void EnableOrDisableHourConverterDependingOnFormat()
        {
            var converter = TryFindResource("HourConverter") as Hour24ToHour12Converter;
            if (converter != null)
            {
                converter.IsEnabled = IsHour12Format;
                BindingOperations.GetBindingExpression(NumericTBHour, NumericTextBox.ValueProperty).UpdateTarget();
            }
        }

        private void FixNumericTextBoxesPositions(ref int dayPosition, ref int monthPosition, ref int yearPosition, ref int hourPosition, ref int minutePosition, ref int secondPosition)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>()
            {
                { "dayPosition", dayPosition },
                { "monthPosition", monthPosition },
                { "yearPosition", yearPosition },
                { "hourPosition", hourPosition },
                { "minutePosition", minutePosition },
                { "secondPosition", secondPosition }
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
        }
        #endregion
    }
}