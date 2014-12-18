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
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Helpers;

    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public partial class DateTimePickerControl : UserControl
    {
        #region Constants
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (DateTime), typeof (DateTimePickerControl), new UIPropertyMetadata(DateTime.Now, OnValueChanged));
        #endregion

        #region Fields
        private readonly DateTimePickerControlViewModel _dateTimePickerControlViewModel;
        private readonly List<NumericTextBox> _numericTextBoxes;
        private DateTimePart _activeTextBoxPart;
        private UIElement _currentCombobox;
        private bool _isInEditMode;
        #endregion

        #region Constructors
        public DateTimePickerControl()
        {
            InitializeComponent();
            _dateTimePickerControlViewModel = new DateTimePickerControlViewModel();
            MainContainer.DataContext = _dateTimePickerControlViewModel;
            _dateTimePickerControlViewModel.PropertyChanged += DateTimePickerControlViewModelOnPropertyChanged;

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

        private void DateTimePickerControlViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Value")
            {
                Value = _dateTimePickerControlViewModel.Value;
            }
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as DateTimePickerControl;
            control._dateTimePickerControlViewModel.Value = control.Value;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RemoveCombobox();

            if (e.ClickCount == 2)
            {
                RemoveCombobox();

                _activeTextBoxPart = (DateTimePart) ((sender as TextBlock).Tag);
                DateTimePartHelper.CreateCombobox(MainGrid, _activeTextBoxPart);

                _isInEditMode = true;
            }
        }

        private void RemoveCombobox()
        {
            var comboBox = MainGrid.Children
                .Cast<FrameworkElement>()
                .Where(x => x.Name == "comboBox")
                .FirstOrDefault();
            if (comboBox != null)
            {
                MainGrid.Children.Remove(comboBox);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape && _isInEditMode)
            {
                RemoveCombobox();
                _isInEditMode = false;
                e.Handled = true;
            }

            if (e.Key == Key.Enter && _isInEditMode)
            {
                RemoveCombobox();
                _isInEditMode = false;
                e.Handled = true;
            }
        }
        #endregion
    }
}