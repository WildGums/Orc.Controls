// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel.Data;
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
            var dayFormat = Format.Count(x => x == 'd');
            var monthFormat = Format.Count(x => x == 'M');
            var yearFormat = Format.Count(x => x == 'y');
            var hourFormat = 0;
            var hour12Format = Format.Count(x => x == 'h');
            var hour24Format = Format.Count(x => x == 'H');
            var minuteFormat = Format.Count(x => x == 'm');
            var secondFormat = Format.Count(x => x == 's');

            if (hour12Format > 0 && hour24Format > 0)
            {
                throw new FormatException("Format string is incorrect. Hour field must be 12-hour or 24-hour format, but no both");
            }
            if (hour12Format > 0)
            {
                IsHour12Format = true;
                NumericTBHour.MinValue = 1;
                NumericTBHour.MaxValue = 12;
                ToggleButtonH.Tag = DateTimePart.Hour12;

                EnableOrDisableHourConverterDependingOnFormat();

                hourFormat = hour12Format;
            }
            else
            {
                IsHour12Format = false;
                NumericTBHour.MinValue = 0;
                NumericTBHour.MaxValue = 23;
                ToggleButtonH.Tag = DateTimePart.Hour;

                EnableOrDisableHourConverterDependingOnFormat();

                hourFormat = hour24Format;
            }

            int? dayPosition = null;
            int? monthPosition = null;
            int? yearPosition = null;
            int? hourPosition = null;
            int? minutePosition = null;
            int? secondPosition = null;
            int? amPmPosition = null;

            string separator1 = string.Empty;
            string separator2 = string.Empty;
            string separator3 = string.Empty;
            string separator4 = string.Empty;
            string separator5 = string.Empty;
            string separator6 = string.Empty;
            string separator7 = string.Empty;

            var current = 0;
            foreach (var c in Format)
            {
                if (c == 'd' && dayPosition == null)
                {
                    dayPosition = current++;
                }
                else if (c == 'M' && monthPosition == null)
                {
                    monthPosition = current++;
                }
                else if (c == 'y' && yearPosition == null)
                {
                    yearPosition = current++;
                }
                else if ((c == 'H' || c == 'h') && hourPosition == null)
                {
                    hourPosition = current++;
                }
                else if (c == 'm' && minutePosition == null)
                {
                    minutePosition = current++;
                }
                else if (c == 's' && secondPosition == null)
                {
                    secondPosition = current++;
                }
                else if (c == 't' && amPmPosition == null)
                {
                    amPmPosition = current++;
                }
                else if (!(c == 'y' || c == 'M' || c == 'd' || c == 'H' || c == 'h' || c == 'm' || c == 's' || c == 't'))
                {
                    if (current == 1) separator1 += c;
                    else if (current == 2) separator2 += c;
                    else if (current == 3) separator3 += c;
                    else if (current == 4) separator4 += c;
                    else if (current == 5) separator5 += c;
                    else if (current == 6) separator6 += c;
                    else if (current == 7) separator7 += c;
                }
            }

            if (dayPosition == null || monthPosition == null || yearPosition == null || hourPosition == null || minutePosition == null || secondPosition == null)
            {
                throw new FormatException("Format string is incorrect. Day, month, year, hour, minute and second fields are mandatory");
            }

            UnsubscribeNumericTextBoxes();

            Grid.SetColumn(NumericTBDay, GetPosition(dayPosition.Value));
            Grid.SetColumn(NumericTBMonth, GetPosition(monthPosition.Value));
            Grid.SetColumn(NumericTBYear, GetPosition(yearPosition.Value));
            Grid.SetColumn(NumericTBHour, GetPosition(hourPosition.Value));
            Grid.SetColumn(NumericTBMinute, GetPosition(minutePosition.Value));
            Grid.SetColumn(NumericTBSecond, GetPosition(secondPosition.Value));
            if (amPmPosition.HasValue) Grid.SetColumn(TextTBAmPm, GetPosition(amPmPosition.Value));

            Grid.SetColumn(ToggleButtonD, GetPosition(dayPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonMo, GetPosition(monthPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonY, GetPosition(yearPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonH, GetPosition(hourPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonM, GetPosition(minutePosition.Value) + 1);
            Grid.SetColumn(ToggleButtonS, GetPosition(secondPosition.Value) + 1);
            if (amPmPosition.HasValue) Grid.SetColumn(ToggleButtonT, GetPosition(amPmPosition.Value) + 1);

            // Fix positions which could be broken, because of AM/PM textblock.
            int dayPos = dayPosition.Value, monthPos = monthPosition.Value, yearPos = yearPosition.Value,
                hourPos = hourPosition.Value, minutePos = minutePosition.Value, secondPos = secondPosition.Value;
            FixNumericTextBoxesPositions(ref dayPos, ref monthPos, ref yearPos, ref hourPos, ref minutePos, ref secondPos);

            _numericTextBoxes[dayPos] = NumericTBDay;
            _numericTextBoxes[monthPos] = NumericTBMonth;
            _numericTextBoxes[yearPos] = NumericTBYear;
            _numericTextBoxes[hourPos] = NumericTBHour;
            _numericTextBoxes[minutePos] = NumericTBMinute;
            _numericTextBoxes[secondPos] = NumericTBSecond;

            SubscribeNumericTextBoxes();

            NumericTBDay.Format = GetFormat(dayFormat);
            NumericTBMonth.Format = GetFormat(monthFormat);
            NumericTBYear.Format = GetFormat(yearFormat);
            NumericTBHour.Format = GetFormat(hourFormat);
            NumericTBMinute.Format = GetFormat(minuteFormat);
            NumericTBSecond.Format = GetFormat(secondFormat);

            Separator1.Text = separator1;
            Separator2.Text = separator2;
            Separator3.Text = separator3;
            Separator4.Text = separator4;
            Separator5.Text = separator5;
            Separator6.Text = separator6;
            Separator7.Text = separator7;
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