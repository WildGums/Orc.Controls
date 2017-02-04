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
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for DatePickerControl.xaml
    /// </summary>
    public partial class DatePickerControl
    {
        #region Fields
        private readonly List<TextBox> _textBoxes;
        private DateTimePart _activeDateTimePart;
        private DateTime _todayValue;
        private DateTimeFormatInfo _formatInfo;
        #endregion

        #region Constructors
        static DatePickerControl()
        {
            typeof(DatePickerControl).AutoDetectViewPropertiesToSubscribe();
        }

        public DatePickerControl()
        {
            InitializeComponent();

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DatePickerControl), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));

            _textBoxes = new List<TextBox>()
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
            // Enable support for switching between textboxes,
            // 0-1 because we can't switch to right on last textbox.
            _textBoxes[0].SubscribeToOnRightBoundReachedEvent(TextBoxOnRightBoundReached);
            _textBoxes[1].SubscribeToOnRightBoundReachedEvent(TextBoxOnRightBoundReached);

            // Enable support for switching between textboxes,
            // 2-1 because we can't switch to left on first textbox.
            _textBoxes[2].SubscribeToOnLeftBoundReachedEvent(TextBoxOnLeftBoundReached);
            _textBoxes[1].SubscribeToOnLeftBoundReachedEvent(TextBoxOnLeftBoundReached);
        }

        private void UnsubscribeNumericTextBoxes()
        {
            // Disable support for switching between textboxes,
            // 0-1 because we can't switch to right on last textbox.
            _textBoxes[0].UnsubscribeFromOnRightBoundReachedEvent(TextBoxOnRightBoundReached);
            _textBoxes[1].UnsubscribeFromOnRightBoundReachedEvent(TextBoxOnRightBoundReached);

            // Disable support for switching between textboxes,
            // 2-1 because we can't switch to left on first textbox.
            _textBoxes[2].UnsubscribeFromOnLeftBoundReachedEvent(TextBoxOnLeftBoundReached);
            _textBoxes[1].UnsubscribeFromOnLeftBoundReachedEvent(TextBoxOnLeftBoundReached);
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

        public bool AllowCopyPaste
        {
            get { return (bool)GetValue(AllowCopyPasteProperty); }
            set { SetValue(AllowCopyPasteProperty, value); }
        }

        public static readonly DependencyProperty AllowCopyPasteProperty = DependencyProperty.Register("AllowCopyPaste", typeof(bool),
            typeof(DatePickerControl), new PropertyMetadata(true));

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
            private set { SetValue(IsYearShortFormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsYearShortFormatPropertyKey = DependencyProperty.RegisterReadOnly("IsYearShortFormat", typeof(bool),
            typeof(DatePickerControl), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Count(x => x == 'y') < 3 ? true : false));

        public static readonly DependencyProperty IsYearShortFormatProperty = IsYearShortFormatPropertyKey.DependencyProperty;
        #endregion

        #region Methods
        private void TextBoxOnLeftBoundReached(object sender, EventArgs e)
        {
            var currentTextBoxIndex = _textBoxes.IndexOf(sender as TextBox);
            var prevTextBox = _textBoxes[currentTextBoxIndex - 1];

            prevTextBox.CaretIndex = prevTextBox.Text.Length;
            prevTextBox.Focus();
        }

        private void TextBoxOnRightBoundReached(object sender, EventArgs eventArgs)
        {
            var currentTextBoxIndex = _textBoxes.IndexOf(sender as TextBox);
            var nextTextBox = _textBoxes[currentTextBoxIndex + 1];

            nextTextBox.CaretIndex = 0;
            nextTextBox.Focus();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            _activeDateTimePart = (DateTimePart)((ToggleButton)sender).Tag;

            var activeTextBox = (TextBox)FindName(_activeDateTimePart.GetDateTimePartName());
            var activeToggleButton = (ToggleButton)FindName(_activeDateTimePart.GetDateTimePartToggleButtonName());

            var dateTime = Value == null ? _todayValue : Value.Value;
            var dateTimePartHelper = new DateTimePartHelper(dateTime, _activeDateTimePart, _formatInfo, activeTextBox, activeToggleButton);
            dateTimePartHelper.CreatePopup();
        }

        private void NumericTBMonth_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var dateTime = Value == null ? _todayValue : Value.Value;
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            NumericTBDay.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)daysInMonth);
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

            ((Popup)calendar.Parent).SetCurrentValue(Popup.IsOpenProperty, false);
        }

        private void UpdateDate(DateTime? date)
        {
            DateTime? value = null;

            if (date != null)
            {
                value = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
            }

            SetCurrentValue(ValueProperty, value);
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
            UpdateDate(DateTime.Today.Date);
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
            UpdateDate(null);
        }

        private void OnCopyButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            if (Value != null)
            {
                Clipboard.SetText(Value.Value.ToString(Format), TextDataFormat.Text);
            }
        }

        private void OnPasteButtonClick(object sender, RoutedEventArgs e)
        {
            DatePickerIcon.SetCurrentValue(ToggleButton.IsCheckedProperty, false);

            if (Clipboard.ContainsData(DataFormats.Text))
            {
                var text = Clipboard.GetText(TextDataFormat.Text);
                var value = DateTime.MinValue;
                if (!string.IsNullOrEmpty(text)
                    && (DateTime.TryParseExact(text, Format, null, DateTimeStyles.None, out value)
                        || DateTime.TryParseExact(text, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, null, DateTimeStyles.None, out value)
                        || DateTime.TryParseExact(text, CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, null, DateTimeStyles.None, out value)))
                {
                    SetCurrentValue(ValueProperty, new DateTime(value.Year, value.Month, value.Day));
                }
            }
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
                Dispatcher.Invoke(() => SetCurrentValue(ValueProperty, nv));
            }
        }

        private void ApplyFormat()
        {
            _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(Format, true);

            IsYearShortFormat = _formatInfo.IsYearShortFormat;
            NumericTBYear.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(_formatInfo.IsYearShortFormat ? 0 : 1));
            NumericTBYear.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(_formatInfo.IsYearShortFormat ? 99 : 3000));

            EnableOrDisableYearConverterDependingOnFormat();

            NumericTBDay.SetCurrentValue(NumericTextBox.FormatProperty, GetFormat(_formatInfo.DayFormat.Length));
            NumericTBMonth.SetCurrentValue(NumericTextBox.FormatProperty, GetFormat(_formatInfo.MonthFormat.Length));
            NumericTBYear.SetCurrentValue(NumericTextBox.FormatProperty, GetFormat(_formatInfo.YearFormat.Length));

            UnsubscribeNumericTextBoxes();

            Grid.SetColumn(NumericTBDay, GetPosition(_formatInfo.DayPosition));
            Grid.SetColumn(NumericTBMonth, GetPosition(_formatInfo.MonthPosition));
            Grid.SetColumn(NumericTBYear, GetPosition(_formatInfo.YearPosition));

            Grid.SetColumn(ToggleButtonD, GetPosition(_formatInfo.DayPosition) + 1);
            Grid.SetColumn(ToggleButtonMo, GetPosition(_formatInfo.MonthPosition) + 1);
            Grid.SetColumn(ToggleButtonY, GetPosition(_formatInfo.YearPosition) + 1);

            _textBoxes[_formatInfo.DayPosition] = NumericTBDay;
            _textBoxes[_formatInfo.MonthPosition] = NumericTBMonth;
            _textBoxes[_formatInfo.YearPosition] = NumericTBYear;

            // Fix tab order inside control.
            NumericTBDay.SetCurrentValue(TabIndexProperty, _formatInfo.DayPosition);
            NumericTBMonth.SetCurrentValue(TabIndexProperty, _formatInfo.MonthPosition);
            NumericTBYear.SetCurrentValue(TabIndexProperty, _formatInfo.YearPosition);
            DatePickerIcon.SetCurrentValue(TabIndexProperty, Int32.MaxValue);

            SubscribeNumericTextBoxes();

            Separator1.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator1);
            Separator2.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator2);
            Separator3.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator3);
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
                BindingOperations.GetBindingExpression(NumericTBYear, NumericTextBox.ValueProperty)?.UpdateTarget();
            }
        }
        #endregion
    }
}