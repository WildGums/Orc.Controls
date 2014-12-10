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
        private int _activeTextBoxIndex;
        private bool _isInEditMode;

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
        }

        private void NumericTextBoxOnLeftBoundReached(object sender, EventArgs e)
        {
            var activeTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var prevTextBox = _numericTextBoxes[activeTextBoxIndex - 1];
            prevTextBox.CaretIndex = prevTextBox.Text.Length;
            prevTextBox.Focus();
        }

        private void NumericTextBoxOnRightBoundReached(object sender, EventArgs eventArgs)
        {
            var activeTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var nextTextBox = _numericTextBoxes[activeTextBoxIndex + 1];
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

        private void NumericTBDays_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _activeTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            NumericTBEditor.Visibility = Visibility.Visible;
            _isInEditMode = true;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape && _isInEditMode)
            {
                NumericTBEditor.Visibility = Visibility.Collapsed;
                _isInEditMode = false;
                e.Handled = true;
            }

            if (e.Key == Key.Enter && _isInEditMode)
            {
                NumericTBEditor.Visibility = Visibility.Collapsed;
                _isInEditMode = false;
                RefreshValues();
                e.Handled = true;
            }
        }

        private void RefreshValues()
        {
            var newValue = NumericTBEditor.Text;
            switch (_activeTextBoxIndex)
            {
                case 0:
                    Value = TimeSpan.FromDays(Convert.ToInt32(newValue));
                    break;
                case 1:
                    Value = TimeSpan.FromHours(Convert.ToInt32(newValue));
                    break;
                case 2:
                    Value = TimeSpan.FromMinutes(Convert.ToInt32(newValue));
                    break;
                case 3:
                    Value = TimeSpan.FromSeconds(Convert.ToInt32(newValue));
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        private void NumericTBEditor_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NumericTBEditor.Focus();
        }
        private void NumericTBEditor_OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsKeyboardFocusWithin)
            {
                NumericTBEditor.Visibility = Visibility.Collapsed;
            }

        }
    }
}
