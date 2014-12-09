using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Orc.Controls
{
    using System.Windows.Media;

    public class NumericTextBox:TextBox
    {
        public event EventHandler RightBoundReached;
        public event EventHandler LeftBoundReached;
        public NumericTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
        }

        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                var textBox = (TextBox)parent;
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
                textBox.SelectAll();
        }
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(NumericTextBox), new UIPropertyMetadata(0, OnMinValueChanged));

        private static void OnMinValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
           
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericTextBox), new UIPropertyMetadata(int.MaxValue, OnMaxValueChanged));

        private static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #region Properties
        new public string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = LeaveOnlyNumbers(value);
            }
        }
 
        #endregion
 
        #region Functions
        private string LeaveOnlyNumbers(string inputText)
        {
            var validText = inputText;
            foreach (var c in inputText)
            {
                if (!IsDigit(c))
                {
                    validText = validText.Replace(c.ToString(), string.Empty);
                }
            }
            return validText;
        }

        private bool IsValidValue(int inputValue)
        {
            return inputValue <= MaxValue && inputValue >= MinValue;
        }

        public bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }
        #endregion
 
        #region Event Functions

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            var text = GetText(e.Text);
            if (text == string.Empty)
            {
                return;
            }
            
            int value;
            if (!int.TryParse(text, out value))
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

            if (e.Key == Key.Right  && CaretIndex == Text.Length)
            {
                RaiseRightBoundReachedEvent();
                e.Handled = true;
            }

            if (e.Key == Key.Left && CaretIndex == 0)
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
    }
}
