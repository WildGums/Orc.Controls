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
    using System.Data;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public partial class DateTimePickerControl
    {
        #region Fields
        private readonly List<NumericTextBox> _numericTextBoxes;
        private bool _isInEditMode;
        private NumericTextBox _activeNumericTextBox;
        private bool _isInEditPopup;
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

            TextBlockD.MouseDown += TextBlock_MouseDown;
            TextBlockMo.MouseDown += TextBlock_MouseDown;
            TextBlockY.MouseDown += TextBlock_MouseDown;
            TextBlockH.MouseDown += TextBlock_MouseDown;
            TextBlockM.MouseDown += TextBlock_MouseDown;
            TextBlockS.MouseDown += TextBlock_MouseDown;
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(DateTime), typeof(DateTimePickerControl),
            new UIPropertyMetadata(DateTime.Now));
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

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_activeNumericTextBox != null)
            {
                _activeNumericTextBox.PreviewLostKeyboardFocus -= NumericTextBoxOnLostFocus;

                RemovePopup();
            }
            

            if (e.ClickCount == 2)
            {
                var activeTextBoxPart = (DateTimePart)((TextBlock)sender).Tag;
                var numericTextBoxName = activeTextBoxPart.GetDateTimePartName();
                _activeNumericTextBox = (NumericTextBox)FindName(numericTextBoxName);

                var popup = DateTimePartHelper.CreatePopup(Value, MainGrid, activeTextBoxPart, _activeNumericTextBox);
                popup.PreviewMouseDown += PopupOnPreviewMouseDown;

                _activeNumericTextBox.PreviewLostKeyboardFocus += NumericTextBoxOnLostFocus;
                _activeNumericTextBox.Focus();

                _isInEditMode = true;
            }
        }

        private void PopupOnPreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _isInEditPopup = true;
        }

        private void NumericTextBoxOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!_isInEditPopup)
            {
                RemovePopup();
            }
        }

        private void RemovePopup()
        {
            var popup = MainGrid.Children.Cast<FrameworkElement>().FirstOrDefault(x => string.Equals(x.Name, "comboBox"));
            if (popup != null)
            {
                MainGrid.Children.Remove(popup);
                _isInEditPopup = false;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape && _isInEditMode)
            {
                RemovePopup();
                _isInEditMode = false;
                e.Handled = true;
            }

            if (e.Key == Key.Enter && _isInEditMode)
            {
                RemovePopup();
                _isInEditMode = false;
                e.Handled = true;
            }
        }
        #endregion
    }
}