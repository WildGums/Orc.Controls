// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerControl.xaml.cs" company="WildGums">
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
    using System.Windows.Media;
    using Catel.MVVM.Views;
    using Calendar = System.Windows.Controls.Calendar;

    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public partial class DatePickerControl
    {
        #region Fields
        private readonly List<NumericTextBox> _numericTextBoxes;
        private DateTimePart _activeDateTimePart;
        #endregion

        #region Constructors
        static DatePickerControl()
        {
            typeof (DatePickerControl).AutoDetectViewPropertiesToSubscribe();
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

            SubscribeNumericTextBoxes();

            UpdateSeparators();
        }

        private void SubscribeNumericTextBoxes()
        {
            _numericTextBoxes[0].RightBoundReached += NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[1].RightBoundReached += NumericTextBoxOnRightBoundReached;

            _numericTextBoxes[2].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[1].LeftBoundReached += NumericTextBoxOnLeftBoundReached;
        }

        private void UnsubscribeNumericTextBoxes()
        {
            _numericTextBoxes[0].RightBoundReached -= NumericTextBoxOnRightBoundReached;
            _numericTextBoxes[1].RightBoundReached -= NumericTextBoxOnRightBoundReached;

            _numericTextBoxes[2].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
            _numericTextBoxes[1].LeftBoundReached -= NumericTextBoxOnLeftBoundReached;
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime Value
        {
            get { return (DateTime) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime), typeof(DatePickerControl),
            new FrameworkPropertyMetadata(DateTime.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
            typeof(DatePickerControl), new FrameworkPropertyMetadata(string.Empty, (sender, e) => ((DatePickerControl)sender).OnFormatChanged()));

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
            _activeDateTimePart = (DateTimePart) ((ToggleButton) sender).Tag;

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

        private void UpdateDate(DateTime date)
        {
            Value = new DateTime(date.Year, date.Month, date.Day);
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
            UpdateDate(DateTime.Today.Date);
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
                accentColor.CreateAccentColorResourceDictionary("DatePicker");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
        }

        private void OnFormatChanged()
        {
            UpdateFormat();
            UpdatePosition();
            UpdateSeparators();
        }

        private void UpdateFormat()
        {
            var dayFormat = Format.Count(x => x == 'd');
            var monthFormat = Format.Count(x => x == 'M');
            var yearFormat = Format.Count(x => x == 'y');

            NumericTBDay.Format = GetFormat(dayFormat);
            NumericTBMonth.Format = GetFormat(monthFormat);
            NumericTBYear.Format = GetFormat(yearFormat);
        }

        private void UpdatePosition()
        {
            int? dayPosition = null;
            int? monthPosition = null;
            int? yearPosition = null;

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
            }

            if (dayPosition == null || monthPosition == null || yearPosition == null)
            {
                throw new FormatException("Format string is incorrect. Day, month and year fields are mandatory");
            }

            UnsubscribeNumericTextBoxes();
            
            Grid.SetColumn(NumericTBDay, GetPosition(dayPosition.Value));
            Grid.SetColumn(NumericTBMonth, GetPosition(monthPosition.Value));
            Grid.SetColumn(NumericTBYear, GetPosition(yearPosition.Value));

            Grid.SetColumn(ToggleButtonD, GetPosition(dayPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonMo, GetPosition(monthPosition.Value) + 1);
            Grid.SetColumn(ToggleButtonY, GetPosition(yearPosition.Value) + 1);

            _numericTextBoxes[dayPosition.Value] = NumericTBDay;
            _numericTextBoxes[monthPosition.Value] = NumericTBMonth;
            _numericTextBoxes[yearPosition.Value] = NumericTBYear;

            SubscribeNumericTextBoxes();
        }

        private int GetPosition(int index)
        {
            return index*2;
        }

        private string GetFormat(int digits)
        {
            return new string(Enumerable.Repeat('0', digits).ToArray());
        }

        private void UpdateSeparators()
        {
            var separator = CultureInfo.InvariantCulture.DateTimeFormat.DateSeparator;
            Separator1.Text = Separator2.Text = separator;
        }
        #endregion
    }
}