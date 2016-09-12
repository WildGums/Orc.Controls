// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
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
    using System.Windows.Media;
    using Catel.MVVM.Views;
    using Catel.Windows.Threading;
    using Calendar = System.Windows.Controls.Calendar;
    using Converters;

    /// <summary>
    /// Interaction logic for DatePickerControl.xaml
    /// </summary>
    public partial class DatePickerControl
    {
        #region Fields
        private readonly List<NumericTextBox> _numericTextBoxes;
        private DateTimePart _activeDateTimePart;
        private DateTime _todayValue;
        #endregion

        #region Constructors
        static DatePickerControl()
        {
            typeof(DatePickerControl).AutoDetectViewPropertiesToSubscribe();
        }

        public DatePickerControl()
        {
            InitializeComponent();

            _numericTextBoxes = new List<NumericTextBox>()
            {
                NumericTBDay,
                NumericTBMonth,
                NumericTBYear,
            };

            DateTime now = DateTime.Now;
            _todayValue = new DateTime(now.Year, now.Month, now.Day);
        }

        private void SubscribeNumericTextBoxes()
        {
            // Enable support for switching between numeric textboxes, 
            // 0-1 because we can't switch to right on last numeric textbox.
            _numericTextBoxes[0].RightBoundReached += NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[1].RightBoundReached += NumericTextBoxOnRightBoundReached;

            // Enable support for switching between numeric textboxes, 
            // 2-1 because we can't switch to left on first numeric textbox.
            _numericTextBoxes[2].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[1].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
        }

        private void UnsubscribeNumericTextBoxes()
        {
            // Disable support for switching between numeric textboxes, 
            // 0-1 because we can't switch to right on last numeric textbox.
            _numericTextBoxes[0].RightBoundReached -= NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[1].RightBoundReached -= NumericTextBoxOnRightBoundReached;

            // Disable support for switching between numeric textboxes, 
            // 2-1 because we can't switch to left on first numeric textbox.
            _numericTextBoxes[2].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[1].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime?), typeof(DatePickerControl),
            new FrameworkPropertyMetadata(DateTime.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((DatePickerControl)sender).OnValueChanged(e.OldValue, e.NewValue)));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register("ShowOptionsButton", typeof(bool),
            typeof(DatePickerControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof(Brush),
            typeof(DatePickerControl), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((DatePickerControl)sender).OnAccentColorBrushChanged()));

        public bool AllowNull
        {
            get { return (bool)GetValue(AllowNullProperty); }
            set { SetValue(AllowNullProperty, value); }
        }

        public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register("AllowNull", typeof(bool),
            typeof(DatePickerControl), new PropertyMetadata(false));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool),
            typeof(DatePickerControl), new PropertyMetadata(false));

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string),
            typeof(DatePickerControl), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, (sender, e) => ((DatePickerControl)sender).OnFormatChanged()));

        public bool IsYearShortFormat
        {
            get { return (bool)GetValue(IsYearShortFormatProperty); }
            private set { SetValue(IsYearShortFormatKey, value); }
        }

        private static readonly DependencyPropertyKey IsYearShortFormatKey = DependencyProperty.RegisterReadOnly("IsYearShortFormat", typeof(bool),
            typeof(DatePickerControl), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Count(x => x == 'y') < 3 ? true : false));

        public static readonly DependencyProperty IsYearShortFormatProperty = IsYearShortFormatKey.DependencyProperty;
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

            var dateTime = Value == null ? _todayValue : Value.Value;
            var dateTimePartHelper = new DateTimePartHelper(dateTime, _activeDateTimePart, activeNumericTextBox, activeToggleButton);
            dateTimePartHelper.CreatePopup();
        }

        private void NumericTBMonth_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var dateTime = Value == null ? _todayValue : Value.Value;
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            NumericTBDay.MaxValue = daysInMonth;
        }

        private Calendar CreateCalendarPopupSource()
        {
            var dateTime = Value == null ? _todayValue : Value.Value;
            var calendar = new Calendar()
            {
                Margin = new Thickness(0, -3, 0, -3),
                DisplayDate = dateTime,
                SelectedDate = Value
            };

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

        private void UpdateDate(DateTime? date)
        {
            if (date == null)
            {
                Value = null;
            }
            else
            {
                Value = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
            }
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
            DatePickerIcon.IsChecked = false;
            UpdateDate(DateTime.Today.Date);
        }

        private void OnSelectDateButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.IsChecked = false;

            var calendarPopup = CreateCalendarPopup();
            var calendarPopupSource = CreateCalendarPopupSource();
            calendarPopup.Child = calendarPopupSource;

            calendarPopupSource.Focus();
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.IsChecked = false;
            UpdateDate(null);
        }

        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush)AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("DatePicker");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
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
                var date = DateTime.Now.Date;
                nv = new DateTime(date.Year, date.Month, date.Day);
            }

            if ((ov == null && nv != null)
                || (ov != null && nv == null))
            {
                ApplyFormat();
            }

            if (newValue == null && nv != null)
            {
                Dispatcher.BeginInvoke(() => Value = nv);
            }
        }

        private void ApplyFormat()
        {
            var formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(Format, true);

            IsYearShortFormat = formatInfo.IsYearShortFormat;
            NumericTBYear.MinValue = formatInfo.IsYearShortFormat == true ? 0 : 1;
            NumericTBYear.MaxValue = formatInfo.IsYearShortFormat == true ? 99 : 3000;

            EnableOrDisableYearConverterDependingOnFormat();

            NumericTBDay.Format = GetFormat(formatInfo.DayFormat.Length);
            NumericTBMonth.Format = GetFormat(formatInfo.MonthFormat.Length);
            NumericTBYear.Format = GetFormat(formatInfo.YearFormat.Length);

            UnsubscribeNumericTextBoxes();

            Grid.SetColumn(NumericTBDay, GetPosition(formatInfo.DayPosition));
            Grid.SetColumn(NumericTBMonth, GetPosition(formatInfo.MonthPosition));
            Grid.SetColumn(NumericTBYear, GetPosition(formatInfo.YearPosition));

            Grid.SetColumn(ToggleButtonD, GetPosition(formatInfo.DayPosition) + 1);
            Grid.SetColumn(ToggleButtonMo, GetPosition(formatInfo.MonthPosition) + 1);
            Grid.SetColumn(ToggleButtonY, GetPosition(formatInfo.YearPosition) + 1);

            _numericTextBoxes[formatInfo.DayPosition] = NumericTBDay;
            _numericTextBoxes[formatInfo.MonthPosition] = NumericTBMonth;
            _numericTextBoxes[formatInfo.YearPosition] = NumericTBYear;

            SubscribeNumericTextBoxes();

            Separator1.Text = Value == null ? string.Empty : formatInfo.Separator1;
            Separator2.Text = Value == null ? string.Empty : formatInfo.Separator2;
            Separator3.Text = Value == null ? string.Empty : formatInfo.Separator3;
        }

        private int GetPosition(int index)
        {
            return index * 2;
        }

        private string GetFormat(int digits)
        {
            return new string(Enumerable.Repeat('0', digits).ToArray());
        }

        private void EnableOrDisableYearConverterDependingOnFormat()
        {
            var converter = TryFindResource("YearLongToYearShortConverter") as YearLongToYearShortConverter;
            if (converter != null)
            {
                converter.IsEnabled = IsYearShortFormat;
                BindingOperations.GetBindingExpression(NumericTBYear, NumericTextBox.ValueProperty).UpdateTarget();
            }
        }
        #endregion
    }
}