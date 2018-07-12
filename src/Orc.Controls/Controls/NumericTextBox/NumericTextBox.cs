// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel;
    using Catel.Logging;
    using Catel.Windows.Input;

    public class NumericTextBox : TextBox
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string MinusCharacter = "-";
        private const string PeriodCharacter = ".";
        private const string CommaCharacter = ",";

        private static readonly HashSet<Key> AllowedKeys = new HashSet<Key>
        {
            Key.Back,
            Key.CapsLock,
            Key.LeftCtrl,
            Key.RightCtrl,
            Key.Down,
            Key.End,
            Key.Enter,
            Key.Escape,
            Key.Home,
            Key.Insert,
            Key.Left,
            Key.PageDown,
            Key.PageUp,
            Key.Right,
            Key.LeftShift,
            Key.RightShift,
            Key.Tab,
            Key.Up
        };

        private readonly MouseButtonEventHandler _selectivelyIgnoreMouseButtonDelegate;
        private readonly RoutedEventHandler _selectAllTextDelegate;

        private bool _textChangingIsInProgress = false;

        #region Constructors
        public NumericTextBox()
        {
            _selectivelyIgnoreMouseButtonDelegate = SelectivelyIgnoreMouseButton;
            _selectAllTextDelegate = SelectAllText;

            VerticalContentAlignment = VerticalAlignment.Center;

            TextChanged += OnTextChanged;
            LostFocus += OnLostFocus;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
        #endregion

        #region Properties
        public bool IsNullValueAllowed
        {
            get { return (bool)GetValue(IsNullValueAllowedProperty); }
            set { SetValue(IsNullValueAllowedProperty, value); }
        }

        public static readonly DependencyProperty IsNullValueAllowedProperty = DependencyProperty.Register("IsNullValueAllowed", typeof(bool),
            typeof(NumericTextBox), new PropertyMetadata(true, (sender, e) => ((NumericTextBox)sender).OnIsNullValueAllowedChanged()));


        public bool IsNegativeAllowed
        {
            get { return (bool)GetValue(IsNegativeAllowedProperty); }
            set { SetValue(IsNegativeAllowedProperty, value); }
        }

        public static readonly DependencyProperty IsNegativeAllowedProperty = DependencyProperty.Register("IsNegativeAllowed", typeof(bool),
            typeof(NumericTextBox), new PropertyMetadata(false, (sender, e) => ((NumericTextBox)sender).OnIsNegativeAllowedChanged()));


        public bool IsDecimalAllowed
        {
            get { return (bool)GetValue(IsDecimalAllowedProperty); }
            set { SetValue(IsDecimalAllowedProperty, value); }
        }

        public static readonly DependencyProperty IsDecimalAllowedProperty = DependencyProperty.Register("IsDecimalAllowed", typeof(bool),
            typeof(NumericTextBox), new PropertyMetadata(false, (sender, e) => ((NumericTextBox)sender).OnIsDecimalAllowedChanged()));


        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double),
            typeof(NumericTextBox), new UIPropertyMetadata(0d));


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

        public event EventHandler ValueChanged;
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
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, _selectivelyIgnoreMouseButtonDelegate, true);
            AddHandler(GotKeyboardFocusEvent, _selectAllTextDelegate, true);
            AddHandler(MouseDoubleClickEvent, _selectAllTextDelegate, true);

            DataObject.AddPastingHandler(this, OnPaste);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            RemoveHandler(PreviewMouseLeftButtonDownEvent, _selectivelyIgnoreMouseButtonDelegate);
            RemoveHandler(GotKeyboardFocusEvent, _selectAllTextDelegate);
            RemoveHandler(MouseDoubleClickEvent, _selectAllTextDelegate);

            DataObject.RemovePastingHandler(this, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsDecimalAllowed && !IsDigitsOnly(text))
                {
                    Log.Warning("Pasted text '{0}' contains decimal separator which is not allowed, paste is not allowed", text);

                    e.CancelCommand();
                }
                else if (!IsNegativeAllowed && text.Contains(MinusCharacter))
                {
                    Log.Warning("Pasted text '{0}' contains negative value which is not allowed, paste is not allowed", text);

                    e.CancelCommand();
                }

                var tempDouble = 0d;
                if (!double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out tempDouble))
                {
                    Log.Warning("Pasted text '{0}' could not be parsed as double (wrong culture?), paste is not allowed", text);

                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void OnIsNullValueAllowedChanged()
        {
            EnforceRules();
        }

        private void OnIsNegativeAllowedChanged()
        {
            if (IsNegativeAllowed)
            {
                AllowedKeys.Add(Key.OemMinus);
            }
            else
            {
                if (AllowedKeys.Contains(Key.OemMinus))
                {
                    AllowedKeys.Remove(Key.OemMinus);
                }
            }

            EnforceRules();
        }

        private void OnIsDecimalAllowedChanged()
        {
            EnforceRules();
        }

        private static bool DoesStringValueRequireUpdate(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            var update = true;

            // CTL-1000 NumericTextBox behavior doesn't allow some values (e.g. 2.05)
            var separator = Math.Max(text.IndexOf(CommaCharacter), text.IndexOf(PeriodCharacter));
            if (separator >= 0)
            {
                var resetUpdate = true;

                for (var i = separator + 1; i < text.Length; i++)
                {
                    if (text[i] == '0')
                    {
                        continue;
                    }

                    resetUpdate = false;
                    break;
                }

                if (resetUpdate)
                {
                    update = false;
                }
            }

            // CTL-761
            if (string.Equals(text, "-") || string.Equals(text, "-0"))
            {
                // User is typing -0 (whould would result in 0, which we don't want yet, maybe they are typing -0.5)
                update = false;
            }

            if (text.StartsWith(CommaCharacter) || text.EndsWith(CommaCharacter) ||
                text.StartsWith(PeriodCharacter) || text.EndsWith(PeriodCharacter))
            {
                // User is typing a . or , don't update
                update = false;
            }

            if (text.StartsWith(CommaCharacter) || text.EndsWith(CommaCharacter) ||
                text.StartsWith(PeriodCharacter) || text.EndsWith(PeriodCharacter))
            {
                // User is typing a . or , don't update
                update = false;
            }

            return update;
        }

        private bool IsValidDoubleValue(double inputValue)
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

            var text = Text;

            if (DoesStringValueRequireUpdate(text))
            {
                SetCurrentValue(ValueProperty, GetDoubleValue(text));
            }

            _textChangingIsInProgress = false;
        }

        private double? GetDoubleValue(string text)
        {
            double? doubleValue = null;

            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    // TODO: Do we want to handle P2, etc (e.g. 50.00%)
                    doubleValue = Convert.ToDouble(text, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception)
            {
                // Ignore
            }

            if (!IsNullValueAllowed && !doubleValue.HasValue)
            {
                doubleValue = default(double);
            }

            return doubleValue;
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            var text = GetText(e.Text);
            if (text == string.Empty)
            {
                return;
            }

            if (!DoesStringValueRequireUpdate(text))
            {
                return;
            }

            double value;
            if (!double.TryParse(text, out value))
            {
                e.Handled = true;
                return;
            }

            if (!IsValidDoubleValue(value))
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

            var notAllowed = true;
            var keyValue = GetKeyValue(e);

            var numberDecimalSeparator = GetDecimalSeparator();

            if (keyValue == numberDecimalSeparator && IsDecimalAllowed)
            {
                notAllowed = Text.Contains(numberDecimalSeparator);
            }
            else if (keyValue == MinusCharacter && IsNegativeAllowed)
            {
                notAllowed = CaretIndex > 0;
            }
            else if (AllowedKeys.Contains(e.Key) || IsDigit(e.Key))
            {
                notAllowed = (e.Key == Key.OemMinus && CaretIndex > 0 && IsNegativeAllowed);
            }

            e.Handled = notAllowed;
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
                newValue = GetNewValue(value.Value, increment);
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

            var selectedText = SelectedText;
            if (!string.IsNullOrEmpty(selectedText))
            {
                text.Remove(CaretIndex, selectedText.Length);
            }

            text.Insert(CaretIndex, inputText);

            return text.ToString();
        }

        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
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

        private void SelectAllText(object sender, RoutedEventArgs e)
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

            ValueChanged.SafeInvoke(this);

            UpdateText();
        }

        private void OnFormatChanged()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            var textValue = Value == null ? string.Empty : Value.Value.ToString(Format);

            SetCurrentValue(TextProperty, textValue);
        }

        private void EnforceRules()
        {
            var value = Value;
            if (value.HasValue)
            {
                if (!IsNegativeAllowed && value.Value < 0)
                {
                    value = 0d;
                }

                if (!IsDecimalAllowed)
                {
                    value = Math.Round(value.Value, 0);
                }
            }
            else
            {
                value = MinValue;
            }

            if (value != Value)
            {
                SetCurrentValue(ValueProperty, value);
            }
        }

        private string GetDecimalSeparator()
        {
            var numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            return numberDecimalSeparator;
        }

        private bool IsDigitsOnly(string input)
        {
            foreach (char c in input)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsDigit(Key key)
        {
            bool isDigit;

            var isShiftKey = KeyboardHelper.AreKeyboardModifiersPressed(ModifierKeys.Shift);

            if (key >= Key.D0 && key <= Key.D9 && !isShiftKey)
            {
                isDigit = true;
            }
            else
            {
                isDigit = key >= Key.NumPad0 && key <= Key.NumPad9;
            }

            return isDigit;
        }

        private string GetKeyValue(KeyEventArgs e)
        {
            var keyValue = string.Empty;

            if (e.Key == Key.Decimal)
            {
                keyValue = GetDecimalSeparator();
            }
            else if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                keyValue = MinusCharacter;
            }
            else if (e.Key == Key.OemComma)
            {
                keyValue = CommaCharacter;
            }
            else if (e.Key == Key.OemPeriod)
            {
                keyValue = PeriodCharacter;
            }

            return keyValue;
        }
        #endregion
    }
}
