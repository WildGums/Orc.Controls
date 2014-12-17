// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public partial class DateTimePickerControl : UserControl
    {
        #region Constants
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(DateTime), typeof(DateTimePickerControl), new UIPropertyMetadata(DateTime.Now, OnValueChanged));
        #endregion

        #region Fields
        private readonly List<NumericTextBox> _numericTextBoxes;
        private readonly DateTimePickerControlViewModel _timeSpanControlViewModel;
        #endregion

        #region Constructors
        public DateTimePickerControl()
        {
            InitializeComponent();
            _timeSpanControlViewModel = new DateTimePickerControlViewModel();
            MainContainer.DataContext = _timeSpanControlViewModel;
            _timeSpanControlViewModel.PropertyChanged += TimeSpanControlViewModelOnPropertyChanged;

            _numericTextBoxes = new List<NumericTextBox>()
            {
                NumericTBDay,
                NumericTBMonth,
                NumericTBYear,
                NumericTBHour,
                NumericTBMinute,
                NumericTBSecond,
            };
            NumericTBDay.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBMonth.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBYear.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBHour.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBMinute.RightBoundReached += NumericTextBoxOnRightBoundReached;

            NumericTBYear.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBMonth.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBHour.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBMinute.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBSecond.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
        }
        #endregion

        #region Properties
        public DateTime Value
        {
            get { return (DateTime) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
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

        private void TimeSpanControlViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Value")
            {
                Value = _timeSpanControlViewModel.Value;
            }
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as DateTimePickerControl;
            control._timeSpanControlViewModel.Value = control.Value;
        }
        #endregion
    }
}