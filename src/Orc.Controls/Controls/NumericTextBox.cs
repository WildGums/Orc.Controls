// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericTextBox.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class NumericTextBox : TextBox
    {
        #region Constants
        public static readonly DependencyProperty AllowDecimalProperty =
            DependencyProperty.Register("AllowDecimal", typeof (bool), typeof (NumericTextBox), new UIPropertyMetadata(false, AllowDecimalChanged));

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof (double), typeof (NumericTextBox), new UIPropertyMetadata(0.0, OnMinValueChanged));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof (double), typeof (NumericTextBox), new UIPropertyMetadata(double.MaxValue, OnMaxValueChanged));
        #endregion

        #region Constructors
        public NumericTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
        }
        #endregion

        #region Properties
        public bool AllowDecimal
        {
            get { return (bool) GetValue(AllowDecimalProperty); }
            set { SetValue(AllowDecimalProperty, value); }
        }

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

        public new string Text
        {
            get { return base.Text; }
            set { base.Text = LeaveOnlyNumbers(value); }
        }

        public double Value
        {
            get { return Convert.ToDouble(base.Text); }
            set
            {
                var format = "F0";
                if (AllowDecimal)
                {
                    format = "0.#####";
                }
                base.Text = value.ToString(format);
            }
        }
        #endregion

        #region Functions
        private string LeaveOnlyNumbers(string inputText)
        {
            var validText = inputText;
            foreach (var c in inputText)
            {
                if (!IsValidSymbol(c))
                {
                    validText = validText.Replace(c.ToString(), string.Empty);
                }
            }
            return validText;
        }

        private bool IsValidSymbol(char c)
        {
            if (IsDigit(c))
            {
                return true;
            }
            var symbol = c.ToString();
            if (AllowDecimal && symbol == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
            {
                return true;
            }
            return false;
        }

        private bool IsValidValue(double inputValue)
        {
            return inputValue <= MaxValue && inputValue >= MinValue;
        }

        public bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
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
            var text = new StringBuilder(Text);
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

        private static void AllowDecimalChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnMinValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion

        public event EventHandler RightBoundReached;
        public event EventHandler LeftBoundReached;
    }
}