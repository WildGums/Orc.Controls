// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public partial class DateTimePickerControl
    {
        #region Constants
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof (DateTime), typeof (DateTimePickerControl),
            new UIPropertyMetadata(DateTime.Now));
        #endregion

        #region Fields
        private readonly List<NumericTextBox> _numericTextBoxes;
        private NumericTextBox _activeNumericTextBox;
        private bool _isInEditMode;
        private bool _isInEditPopup;
        #endregion

        #region Constructors
        static DateTimePickerControl()
        {
            typeof (DateTimePickerControl).AutoDetectViewPropertiesToSubscribe();
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
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
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

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var activeTextBoxPart = (DateTimePart) ((ToggleButton) sender).Tag;

            _activeNumericTextBox = (NumericTextBox) FindName(activeTextBoxPart.GetDateTimePartName());

            var dateTimePartHelper = new DateTimePartHelper(Value, activeTextBoxPart, _activeNumericTextBox);
            dateTimePartHelper.CreatePopup();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape && _isInEditMode)
            {
                _isInEditMode = false;
                e.Handled = true;
            }

            if (e.Key == Key.Enter && _isInEditMode)
            {
                _isInEditMode = false;
                e.Handled = true;
            }
        }
        #endregion
    }
}