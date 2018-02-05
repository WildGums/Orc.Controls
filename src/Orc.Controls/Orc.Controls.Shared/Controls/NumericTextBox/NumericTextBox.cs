// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class NumericTextBox : TextBox
    {
        private bool _textChangingIsInProgress = false;

        #region Constructors
        public NumericTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);

            TextChanged += OnTextChanged;
            LostFocus += OnLostFocus;
        }
        #endregion

        #region Properties
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double),
            typeof(NumericTextBox), new UIPropertyMetadata(0.0));


        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double),
            typeof(NumericTextBox), new UIPropertyMetadata(double.MaxValue));


        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string),
            typeof(NumericTextBox), new UIPropertyMetadata("F0", (sender, e) => ((NumericTextBox)sender).OnFormatChanged()));


        public double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double?),
            typeof(NumericTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((NumericTextBox)sender).OnValueChanged()));
        #endregion

        #region Events
        public event EventHandler RightBoundReached;
        public event EventHandler LeftBoundReached;
        #endregion

        #region Properties
        private bool AllTextSelected
        {
            get { return SelectedText == Text; }
        }

        private bool CaretAtStart
        {
            get { return CaretIndex == 0; }
        }

        private bool CaretAtEnd
        {
            get { return CaretIndex == Text.Length; }
        }
        #endregion

        #region Methods
        private bool IsValidValue(double inputValue)
        {
            return inputValue <= MaxValue && inputValue >= MinValue;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(ValueProperty, GetDoubleValue(Text));
            UpdateText();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _textChangingIsInProgress = true;
            SetCurrentValue(ValueProperty, GetDoubleValue(Text));
            _textChangingIsInProgress = false;
        }

        private double GetDoubleValue(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return MinValue;
            }

            return Convert.ToDouble(text);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            var text = GetText(e.Text);
            if (text == string.Empty)
            {
                return;
            }

            double value;
            if (!double.TryParse(text, out value))
            {
                e.Handled = true;
                return;
            }

            if (!IsValidValue(value))
            {
                e.Handled = true;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Right && (CaretAtEnd || CaretAtStart && AllTextSelected))
            {
                RaiseRightBoundReachedEvent();
                e.Handled = true;
            }

            if (e.Key == Key.Left && CaretAtStart)
            {
                RaiseLeftBoundReachedEvent();
                e.Handled = true;
            }

            if (e.Key == Key.Up && AllTextSelected && !IsReadOnly)
            {
                OnUpDown(1);
                e.Handled = true;
            }

            if (e.Key == Key.Down && AllTextSelected && !IsReadOnly)
            {
                OnUpDown(-1);
                e.Handled = true;
            }
        }

        private void OnUpDown(int increment)
        {
            var value = Value;
            var newValue = value;

            if (value == null)
            {
                newValue = MinValue;
            }
            else
            {
                newValue = GetNewValue(Value.Value, increment);
            }

            SetCurrentValue(ValueProperty, newValue);

            SelectAll();
        }

        private double GetNewValue(double oldValue, int increment)
        {
            if (oldValue.Equals(MaxValue) && increment == 1)
            {
                return MinValue;
            }
            if (oldValue.Equals(MinValue) && increment == -1)
            {
                return MaxValue;
            }

            return oldValue + Convert.ToDouble(increment);
        }

        private void RaiseRightBoundReachedEvent()
        {
            RightBoundReached?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseLeftBoundReachedEvent()
        {
            LeftBoundReached?.Invoke(this, EventArgs.Empty);
        }

        private string GetText(string inputText)
        {
            var text = new StringBuilder(base.Text);
            if (!string.IsNullOrEmpty(SelectedText))
            {
                text.Remove(CaretIndex, SelectedText.Length);
            }
            text.Insert(CaretIndex, inputText);
            return (text.ToString());
        }

        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            var textBox = parent as TextBox;
            if (textBox != null)
            {
                if (!textBox.IsKeyboardFocusWithin)
                {
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        private void OnValueChanged()
        {
            if (_textChangingIsInProgress)
            {
                return;
            }
            UpdateText();
        }

        private void OnFormatChanged()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            SetCurrentValue(TextProperty, Value == null ? string.Empty : Value.Value.ToString(Format));
        }
        #endregion
    }
}