using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Orc.Controls
{
    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    public partial class TimeSpanControl : UserControl
    {
        private TimeSpanControlViewModel _timeSpanControlViewModel;
        private List<NumericTextBox> _numericTextBoxes;
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
            NumericTBDays.RightBoundReached += NumericTbDaysOnRightBoundReached;
            NumericTBHours.RightBoundReached += NumericTbDaysOnRightBoundReached;
            NumericTBMinutes.RightBoundReached += NumericTbDaysOnRightBoundReached;

            NumericTBHours.LeftBoundReached += NumericTbDaysOnLeftBoundReached;
            NumericTBMinutes.LeftBoundReached += NumericTbDaysOnLeftBoundReached;
            NumericTBSeconds.LeftBoundReached += NumericTbDaysOnLeftBoundReached;
        }

        private void NumericTbDaysOnLeftBoundReached(object sender, EventArgs e)
        {
            var activeTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var prevTextBox = _numericTextBoxes[activeTextBoxIndex - 1];
            prevTextBox.Focus();
            prevTextBox.CaretIndex = prevTextBox.Text.Length;
        }

        private void NumericTbDaysOnRightBoundReached(object sender, EventArgs eventArgs)
        {
            var activeTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var nextTextBox = _numericTextBoxes[activeTextBoxIndex + 1];
            nextTextBox.Focus();
            nextTextBox.CaretIndex = 0;
        }

        private void TimeSpanControlViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Value")
            {
                Value = _timeSpanControlViewModel.Value;
            }
        }

        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimeSpanControl), new UIPropertyMetadata(TimeSpan.Zero, OnValueChanged));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as TimeSpanControl;
            control._timeSpanControlViewModel.Value = control.Value;
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);

            if ((bool)e.NewValue)
            {
                var focusedControl = Keyboard.FocusedElement;
                if (focusedControl.Equals(DaysDockPanel))
                {
                    Keyboard.Focus(NumericTBDays);
                }
            }
        }
        
    }
}
