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
    public partial class TimeSpanControl : UserControl
    {
        private readonly TimeSpanControlViewModel _timeSpanControlViewModel;
        private readonly List<NumericTextBox> _numericTextBoxes;
        private TimeSpanPart _activeTextBoxPart;
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

        public TimeSpan Value
        {
            get { return (TimeSpan) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof (TimeSpan), typeof (TimeSpanControl), new UIPropertyMetadata(TimeSpan.Zero, OnValueChanged));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = obj as TimeSpanControl;
            control._timeSpanControlViewModel.Value = control.Value;
        }

        private void NumericTBDays_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _activeTextBoxPart = (TimeSpanPart)(_numericTextBoxes.IndexOf(sender as NumericTextBox));
            NumericTBEditorContainer.Visibility = Visibility.Visible;
            _isInEditMode = true;
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
                Value = CreateTimeSpan(_activeTextBoxPart, NumericTBEditor.Value);
                e.Handled = true;
            }
        }

        private TimeSpan CreateTimeSpan(TimeSpanPart timeSpanPart, double value)
        {
            switch (timeSpanPart)
            {
                case TimeSpanPart.Days:
                    return TimeSpan.FromDays(value);
                case TimeSpanPart.Hours:
                    return TimeSpan.FromHours(value);
                case TimeSpanPart.Minutes:
                    return TimeSpan.FromMinutes(value);
                case TimeSpanPart.Seconds:
                    return TimeSpan.FromSeconds(value);
                default:
                    throw new InvalidOperationException();
            }
        }

        private void NumericTBEditor_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NumericTBEditorUnit.Text = GetUnit();
            NumericTBEditor.Focus();
        }

        private string GetUnit()
        {
            switch (_activeTextBoxPart)
            {
                case TimeSpanPart.Days:
                    return "days";
                case TimeSpanPart.Hours:
                    return "hours";
                case TimeSpanPart.Minutes:
                    return "minutes";
                case TimeSpanPart.Seconds:
                    return "seconds";
                default:
                    throw new InvalidOperationException();
            }
        }

        private void NumericTBEditor_OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsKeyboardFocusWithin)
            {
                NumericTBEditor.Value = GetTotalValue();
                return;
            }
            NumericTBEditorContainer.Visibility = Visibility.Collapsed;
        }

        private double GetTotalValue()
        {
            switch (_activeTextBoxPart)
            {
                case TimeSpanPart.Days:
                    return Value.TotalDays;
                case TimeSpanPart.Hours:
                    return Value.TotalHours;
                case TimeSpanPart.Minutes:
                    return Value.TotalMinutes;
                case TimeSpanPart.Seconds:
                    return Value.TotalSeconds;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
