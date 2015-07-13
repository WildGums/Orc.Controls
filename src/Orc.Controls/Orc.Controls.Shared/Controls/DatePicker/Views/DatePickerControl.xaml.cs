// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel.MVVM.Views;

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

            NumericTBDay.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBMonth.RightBoundReached += NumericTextBoxOnRightBoundReached;

            NumericTBYear.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBMonth.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
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
        #endregion
    }
}