// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanControl.xaml.cs" company="Wild Gums">
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
    using Helpers;

    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public partial class TimeSpanControl : UserControl
    {
        #region Constants
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (TimeSpan), typeof (TimeSpanControl), new UIPropertyMetadata(TimeSpan.Zero, OnValueChanged));
        #endregion

        #region Fields
        private readonly List<NumericTextBox> _numericTextBoxes;
        private readonly TimeSpanControlViewModel _timeSpanControlViewModel;
        private TimeSpanPart _activeTextBoxPart;
        private bool _isInEditMode;
        #endregion

        #region Constructors
        public TimeSpanControl()
        {
            InitializeComponent();
            _timeSpanControlViewModel = new TimeSpanControlViewModel();
            MainContainer.DataContext = _timeSpanControlViewModel;
            _timeSpanControlViewModel.PropertyChanged += TimeSpanControlViewModelOnPropertyChanged;

            _numericTextBoxes = new List<NumericTextBox>()
            {
                NumericTBDays,
                NumericTBHours,
                NumericTBMinutes,
                NumericTBSeconds,
            };
            NumericTBDays.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBHours.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBMinutes.RightBoundReached += NumericTextBoxOnRightBoundReached;

            NumericTBHours.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBMinutes.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBSeconds.LeftBoundReached += NumericTextBoxOnLeftBoundReached;

            TextBlockD.MouseDown += TextBlock_MouseDown;
            TextBlockH.MouseDown += TextBlock_MouseDown;
            TextBlockM.MouseDown += TextBlock_MouseDown;
            TextBlockS.MouseDown += TextBlock_MouseDown;
        }
        #endregion

        #region Properties
        public TimeSpan Value
        {
            get { return (TimeSpan) GetValue(ValueProperty); }
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
            var control = obj as TimeSpanControl;
            control._timeSpanControlViewModel.Value = control.Value;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                _activeTextBoxPart = (TimeSpanPart) ((sender as TextBlock).Tag);
                NumericTBEditorContainer.Visibility = Visibility.Visible;
                _isInEditMode = true;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape && _isInEditMode)
            {
                NumericTBEditorContainer.Visibility = Visibility.Collapsed;
                _isInEditMode = false;
                e.Handled = true;
            }

            if (e.Key == Key.Enter && _isInEditMode)
            {
                NumericTBEditorContainer.Visibility = Visibility.Collapsed;
                _isInEditMode = false;
                Value = RoundTimeSpan(TimeSpanPartHelper.CreateTimeSpan(_activeTextBoxPart, NumericTBEditor.Value));
                e.Handled = true;
            }
        }

        private TimeSpan RoundTimeSpan(TimeSpan timeSpan)
        {
            var totalSeconds = Math.Round(timeSpan.TotalSeconds);
            return TimeSpan.FromSeconds(totalSeconds);
        }

        private void NumericTBEditor_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NumericTBEditorUnit.Text = TimeSpanPartHelper.GetTimeSpanPartName(_activeTextBoxPart);
            NumericTBEditor.Focus();
        }

        private void NumericTBEditor_OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsKeyboardFocusWithin)
            {
                NumericTBEditor.Value = TimeSpanPartHelper.GetTimeSpanPartValue(Value, _activeTextBoxPart);
                return;
            }
            NumericTBEditorContainer.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}