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
    using System.Linq;
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

        private static readonly HashSet<Key> AllowedKeys = new()
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
            Key.Up,
            Key.Delete
        };

        private readonly MouseButtonEventHandler _selectivelyIgnoreMouseButtonDelegate;
        private readonly RoutedEventHandler _selectAllTextDelegate;

        private bool _textChangingIsInProgress = true;
        private bool _suspendTextChanged = false;

        #region Constructors
        static NumericTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericTextBox), new FrameworkPropertyMetadata(typeof(NumericTextBox)));
        }

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
        public string NullString
        {
            get { return (string)GetValue(NullStringProperty); }
            set { SetValue(NullStringProperty, value); }
        }

        public static readonly DependencyProperty NullStringProperty = DependencyProperty.Register(
            nameof(NullString), typeof(string), typeof(NumericTextBox), new PropertyMetadata(default(string)));


        public CultureInfo CultureInfo
        {
            get { return (CultureInfo)GetValue(CultureInfoProperty); }
            set { SetValue(CultureInfoProperty, value); }
        }

        public static readonly DependencyProperty CultureInfoProperty = DependencyProperty.Register(
            nameof(CultureInfo), typeof(CultureInfo), typeof(NumericTextBox), new PropertyMetadata(default(CultureInfo)));


        public bool IsChangeValueByUpDownKeyEnabled
        {
            get { return (bool)GetValue(IsChangeValueByUpDownKeyEnabledProperty); }
            set { SetValue(IsChangeValueByUpDownKeyEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsChangeValueByUpDownKeyEnabledProperty = DependencyProperty.Register(
            nameof(IsChangeValueByUpDownKeyEnabled), typeof(bool), typeof(NumericTextBox), new PropertyMetadata(true));
        

        public bool IsNullValueAllowed
        {
            get { return (bool)GetValue(IsNullValueAllowedProperty); }
            set { SetValue(IsNullValueAllowedProperty, value); }
        }

        public static readonly DependencyProperty IsNullValueAllowedProperty = DependencyProperty.Register(nameof(IsNullValueAllowed), typeof(bool),
            typeof(NumericTextBox), new PropertyMetadata(true, (sender, e) => ((NumericTextBox)sender).OnIsNullValueAllowedChanged()));


        public bool IsNegativeAllowed
        {
            get { return (bool)GetValue(IsNegativeAllowedProperty); }
            set { SetValue(IsNegativeAllowedProperty, value); }
        }

        public static readonly DependencyProperty IsNegativeAllowedProperty = DependencyProperty.Register(nameof(IsNegativeAllowed), typeof(bool),
            typeof(NumericTextBox), new PropertyMetadata(false, (sender, e) => ((NumericTextBox)sender).OnIsNegativeAllowedChanged()));


        public bool IsDecimalAllowed
        {
            get { return (bool)GetValue(IsDecimalAllowedProperty); }
            set { SetValue(IsDecimalAllowedProperty, value); }
        }

        public static readonly DependencyProperty IsDecimalAllowedProperty = DependencyProperty.Register(nameof(IsDecimalAllowed), typeof(bool),
            typeof(NumericTextBox), new PropertyMetadata(false, (sender, e) => ((NumericTextBox)sender).OnIsDecimalAllowedChanged()));


        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof(MinValue), typeof(double),
            typeof(NumericTextBox), new UIPropertyMetadata(0d));


        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(double),
            typeof(NumericTextBox), new UIPropertyMetadata(double.MaxValue));


        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string),
            typeof(NumericTextBox), new UIPropertyMetadata("F0", (sender, e) => ((NumericTextBox)sender).OnFormatChanged()));


        public double? Value
        {
            get { return (double?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double?),
            typeof(NumericTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((NumericTextBox)sender).OnValueChanged()));
        #endregion

        #region Events
        public event EventHandler RightBoundReached;
        public event EventHandler LeftBoundReached;
        public event EventHandler ValueChanged;
        #endregion

        #region Properties
        private bool AllTextSelected => SelectedText == Text;

        private bool CaretAtStart => CaretIndex == 0;

        private bool CaretAtEnd => CaretIndex == Text.Length;
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

                if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out _))
                {
                    return;
                }

                Log.Warning("Pasted text '{0}' could not be parsed as double (wrong culture?), paste is not allowed", text);

                e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void OnIsNullValueAllowedChanged()
        {
            //EnforceRules();
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

           // EnforceRules();
        }

        private void OnIsDecimalAllowedChanged()
        {
          // EnforceRules();
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
                // User is typing -0 (would result in 0, which we don't want yet, maybe they are typing -0.5)
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

            using (new DisposableToken<NumericTextBox>(this, x => x.Instance._suspendTextChanged = true,
                x => x.Instance._suspendTextChanged = false))
            {
                UpdateText();
            }
        }
        
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Argument.IsNotNull(() => sender);

            if (_suspendTextChanged)
            {
                return;
            }

            if (_textChangingIsInProgress && IsKeyboardFocused)
            {
                return;
            }

            using (new DisposableToken<NumericTextBox>(this, x => x.Instance._textChangingIsInProgress = false,
                x => x.Instance._textChangingIsInProgress = true))
            {
                UpdateValue();
            }
        }

        internal void UpdateValue()
        {
            var text = Text;

            if (!IsNegativeAllowed && text.StartsWith("-"))
            {
                SetCurrentValue(TextProperty, text.Replace("-", string.Empty));

                return;
            }

            if (DoesStringValueRequireUpdate(text))
            {
                SetCurrentValue(ValueProperty, GetDoubleValue(text));
            }
        }

        private double? GetDoubleValue(string text)
        {
            double? doubleValue = null;

            try
            {
                if (!string.IsNullOrEmpty(text))
                {

                    var culture = CultureInfo ?? CultureInfo.CurrentCulture;

                    var factor = 1d;
                    if (text.Contains(culture.NumberFormat.PercentSymbol))
                    {
                        text = text.Replace(culture.NumberFormat.PercentSymbol, string.Empty);
                        factor = 1d / 100;
                    }

                    // TODO: Do we want to handle P2, etc (e.g. 50.00%)
                    doubleValue = Convert.ToDouble(text, CultureInfo ?? CultureInfo.CurrentCulture) * factor;
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

            if (!IsNegativeAllowed && text.StartsWith("-"))
            {
                SetCurrentValue(TextProperty, text.Replace("-", string.Empty));

                return;
            }

            if (!DoesStringValueRequireUpdate(text))
            {
                return;
            }

            if (!double.TryParse(text, out var value))
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

            if (IsChangeValueByUpDownKeyEnabled && e.Key == Key.Up && AllTextSelected && !IsReadOnly)
            {
                OnUpDown(1);
                e.Handled = true;
            }

            if (IsChangeValueByUpDownKeyEnabled && e.Key == Key.Down && AllTextSelected && !IsReadOnly)
            {
                OnUpDown(-1);
                e.Handled = true;
            }

            e.Handled = IsKeyNotAllowed(e);
        }

        private bool IsKeyNotAllowed(KeyEventArgs e)
        {
            var keyValue = GetKeyValue(e);

            var numberDecimalSeparator = GetDecimalSeparator();

            if (keyValue == numberDecimalSeparator && IsDecimalAllowed)
            {
                return Text.Contains(numberDecimalSeparator);
            }

            if (keyValue == MinusCharacter && IsNegativeAllowed)
            {
                return CaretIndex > 0;
            }

            if (AllowedKeys.Contains(e.Key) || IsDigit(e.Key))
            {
                return e.Key == Key.OemMinus && CaretIndex > 0 && IsNegativeAllowed;
            }

            return true;
        }

        private void OnUpDown(int increment)
        {
            var value = Value;
            var newValue = value is null ? MinValue : GetNewValue(value.Value, increment);

            SetCurrentValue(ValueProperty, newValue);

            PendingMethod.InvokeDispatcher(SelectAll, 1);
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

            return oldValue + Convert.ToDouble(increment, CultureInfo ?? CultureInfo.CurrentCulture);
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
            while (parent is not null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (!(parent is TextBox textBox))
            {
                return;
            }

            if (textBox.IsFocused || textBox.IsKeyboardFocusWithin)
            {
                return;
            }

            textBox.Focus();
            e.Handled = true;
        }

        private void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            textBox?.SelectAll();
        }

        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);

            if (_textChangingIsInProgress && IsKeyboardFocused)
            {
                return;
            }

            UpdateText();
        }

        private void OnFormatChanged()
        {
            UpdateText();
        }

        internal void UpdateText()
        {
            var textValue = Value is null ? NullString : Value.Value.ToString(Format, CultureInfo ?? CultureInfo.CurrentCulture);

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

            if (!Equals(value, Value))
            {
                SetCurrentValue(ValueProperty, value);
            }
        }

        private string GetDecimalSeparator()
        {
            var culture = CultureInfo ?? CultureInfo.CurrentCulture;

            var numberDecimalSeparator = culture.NumberFormat.NumberDecimalSeparator;
            return numberDecimalSeparator;
        }

        private bool IsDigitsOnly(string input)
        {
            return input.All(c => c >= '0' && c <= '9');
        }

        private static bool IsDigit(Key key)
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

            switch (e.Key)
            {
                case Key.Decimal:
                    keyValue = GetDecimalSeparator();
                    break;

                case Key.OemMinus:
                case Key.Subtract:
                    keyValue = MinusCharacter;
                    break;

                case Key.OemComma:
                    keyValue = CommaCharacter;
                    break;

                case Key.OemPeriod:
                    keyValue = PeriodCharacter;
                    break;
            }

            return keyValue;
        }
        #endregion
    }
}
