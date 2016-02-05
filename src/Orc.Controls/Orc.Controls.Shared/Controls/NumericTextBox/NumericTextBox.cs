// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
        #region Constructors
        public NumericTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
            this.TextChanged += NumericTextBox_TextChanged;
            this.LostFocus += NumericTextBox_LostFocus;
        }

        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Value = GetDoubleValue(Text);
            UpdateText();
        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _textChangingIsInProgress = true;
            Value = GetDoubleValue(Text);
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
        #endregion

        #region Constants
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof (double),
            typeof (NumericTextBox), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof (double),
            typeof (NumericTextBox), new UIPropertyMetadata(double.MaxValue));

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof (string),
            typeof (NumericTextBox), new UIPropertyMetadata("F0"));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof (double),
            typeof (NumericTextBox), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((NumericTextBox) sender).OnValueChanged()));

        private bool _textChangingIsInProgress = false;
        #endregion

        #region Properties
        public double MinValue
        {
            get { return (double) GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public double MaxValue
        {
            get { return (double) GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public string Format
        {
            get { return (string) GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region Events
        public event EventHandler RightBoundReached;
        public event EventHandler LeftBoundReached;
        #endregion

        #region Functions
        private bool IsValidValue(double inputValue)
        {
            return inputValue <= MaxValue && inputValue >= MinValue;
        }
        #endregion

        #region Event Functions

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

            if (e.Key == Key.Up && AllTextSelected)
            {
                OnUpDown(1);
                e.Handled = true;
            }

            if (e.Key == Key.Down)
            {
                OnUpDown(-1);
                e.Handled = true;
            }
        }

        private void OnUpDown(int increment)
        {
            Value = GetNewValue(Value, increment);
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
            var eventCall = RightBoundReached;
            if (eventCall != null)
            {
                eventCall(this, EventArgs.Empty);
            }
        }

        private void RaiseLeftBoundReachedEvent()
        {
            var eventCall = LeftBoundReached;
            if (eventCall != null)
            {
                eventCall(this, EventArgs.Empty);
            }
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
        #endregion

        #endregion

        #region Methods
        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                var textBox = (TextBox) parent;
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

        private void UpdateText()
        {
            base.Text = Value.ToString(Format);
        }
        #endregion
    }
}