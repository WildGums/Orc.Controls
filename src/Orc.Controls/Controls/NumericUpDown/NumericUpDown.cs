namespace Orc.Controls
{
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using Catel;

    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public class NumericUpDown : Control
    {
        #region Fields
#pragma warning disable WPF0120 // Register containing member name as name for routed command.
        private static readonly RoutedUICommand CancelChangesCommand = new ("CancelChanges", "CancelChanges", typeof(NumericUpDown));
        private static readonly RoutedUICommand MajorDecreaseValueCommand = new ("MajorDecreaseValue", "MajorDecreaseValue", typeof(NumericUpDown));
        private static readonly RoutedUICommand MajorIncreaseValueCommand = new ("MajorIncreaseValue", "MajorIncreaseValue", typeof(NumericUpDown));
        private static readonly RoutedUICommand MinorDecreaseValueCommand = new ("MinorDecreaseValue", "MinorDecreaseValue", typeof(NumericUpDown));
        private static readonly RoutedUICommand MinorIncreaseValueCommand = new ("MinorIncreaseValue", "MinorIncreaseValue", typeof(NumericUpDown));
        private static readonly RoutedUICommand UpdateValueStringCommand = new ("UpdateValueString", "UpdateValueString", typeof(NumericUpDown));
#pragma warning restore WPF0120 // Register containing member name as name for routed command.

        private readonly CultureInfo _culture;

        private RepeatButton _decreaseButton;
        private RepeatButton _increaseButton;
        private TextBox _textBox;

        private bool _isValueCoercing;
        #endregion

        #region Constructors
        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public NumericUpDown()
        {
            _culture = (CultureInfo) CultureInfo.CurrentCulture.Clone();

            _culture.NumberFormat.NumberDecimalDigits = DecimalPlaces;

            Loaded += OnLoaded;
        }
        #endregion

        #region Dependency properties
        public decimal Value
        {
            get { return (decimal) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(decimal), typeof(NumericUpDown),
            new PropertyMetadata(0m, OnValueChanged, CoerceValue));

        public decimal MaxValue
        {
            get { return (decimal) GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(decimal), typeof(NumericUpDown),
            new PropertyMetadata(100000000m, OnMaxValueChanged, CoerceMaxValue));

        public decimal MinValue
        {
            get { return (decimal) GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof(MinValue), typeof(decimal), typeof(NumericUpDown),
            new PropertyMetadata(0m, OnMinValueChanged, CoerceMinValue));

        public int DecimalPlaces
        {
            get { return (int) GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(nameof(DecimalPlaces), typeof(int), typeof(NumericUpDown),
            new PropertyMetadata(0, OnDecimalPlacesChanged, CoerceDecimalPlaces));

        public int MaxDecimalPlaces
        {
            get { return (int) GetValue(MaxDecimalPlacesProperty); }
            set { SetValue(MaxDecimalPlacesProperty, value); }
        }

        public static readonly DependencyProperty MaxDecimalPlacesProperty = DependencyProperty.Register(nameof(MaxDecimalPlaces), typeof(int), typeof(NumericUpDown),
            new PropertyMetadata(28, OnMaxDecimalPlacesChanged, CoerceMaxDecimalPlaces));

        public int MinDecimalPlaces
        {
            get { return (int) GetValue(MinDecimalPlacesProperty); }
            set { SetValue(MinDecimalPlacesProperty, value); }
        }

        public static readonly DependencyProperty MinDecimalPlacesProperty = DependencyProperty.Register(nameof(MinDecimalPlaces), typeof(int), typeof(NumericUpDown),
            new PropertyMetadata(0, OnMinDecimalPlacesChanged, CoerceMinDecimalPlaces));

        public bool IsDecimalPointDynamic
        {
            get { return (bool) GetValue(IsDecimalPointDynamicProperty); }
            set { SetValue(IsDecimalPointDynamicProperty, value); }
        }   

        public static readonly DependencyProperty IsDecimalPointDynamicProperty = DependencyProperty.Register(nameof(IsDecimalPointDynamic), typeof(bool), typeof(NumericUpDown),
            new PropertyMetadata(false));

        public decimal MinorDelta
        {
            get { return (decimal) GetValue(MinorDeltaProperty); }
            set { SetValue(MinorDeltaProperty, value); }
        }

        public static readonly DependencyProperty MinorDeltaProperty = DependencyProperty.Register(nameof(MinorDelta), typeof(decimal), typeof(NumericUpDown),
            new PropertyMetadata(1m, OnMinorDeltaChanged, CoerceMinorDelta));

        public decimal MajorDelta
        {
            get { return (decimal) GetValue(MajorDeltaProperty); }
            set { SetValue(MajorDeltaProperty, value); }
        }

        public static readonly DependencyProperty MajorDeltaProperty = DependencyProperty.Register(nameof(MajorDelta), typeof(decimal), typeof(NumericUpDown),
            new PropertyMetadata(10m, OnMajorDeltaChanged, CoerceMajorDelta));

        public bool IsThousandSeparatorVisible
        {
            get { return (bool) GetValue(IsThousandSeparatorVisibleProperty); }
            set { SetValue(IsThousandSeparatorVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsThousandSeparatorVisibleProperty = DependencyProperty.Register(nameof(IsThousandSeparatorVisible), typeof(bool), typeof(NumericUpDown),
            new PropertyMetadata(false, OnIsThousandSeparatorVisibleChanged));

        public bool IsAutoSelectionActive
        {
            get { return (bool) GetValue(IsAutoSelectionActiveProperty); }
            set { SetValue(IsAutoSelectionActiveProperty, value); }
        }

        public static readonly DependencyProperty IsAutoSelectionActiveProperty = DependencyProperty.Register(nameof(IsAutoSelectionActive), typeof(bool), typeof(NumericUpDown),
            new PropertyMetadata(false));

        public bool IsValueWrapAllowed
        {
            get { return (bool) GetValue(IsValueWrapAllowedProperty); }
            set { SetValue(IsValueWrapAllowedProperty, value); }
        }

        public static readonly DependencyProperty IsValueWrapAllowedProperty = DependencyProperty.Register(nameof(IsValueWrapAllowed), typeof(bool), typeof(NumericUpDown),
            new PropertyMetadata(false));
        #endregion

        #region Methods
        private void CoerceValueToBounds(ref decimal value)
        {
            if (value < MinValue)
            {
                value = MinValue;
            }
            else if (value > MaxValue)
            {
                value = MaxValue;
            }
        }

        private void UpdateValue()
        {
            if (_isValueCoercing)
            {
                return;
            }

            SetCurrentValue(ValueProperty, ParseStringToDecimal(_textBox.Text));
        }

        private void CancelChanges()
        {
            _textBox.Undo();
        }

        private void RemoveFocus()
        {
            SetCurrentValue(FocusableProperty, true);
            Focus();
            SetCurrentValue(FocusableProperty, false);
        }

        private void IncreaseValue(bool minor)
        {
            var value = Value;//ParseStringToDecimal(_textBox.Text);

            CoerceValueToBounds(ref value);

            if (value >= MinValue)
            {
                if (minor)
                {
                    if (IsValueWrapAllowed && value + MinorDelta > MaxValue)
                    {
                        value = MinValue;
                    }
                    else
                    {
                        value += MinorDelta;
                    }
                }
                else
                {
                    if (IsValueWrapAllowed && value + MajorDelta > MaxValue)
                    {
                        value = MinValue;
                    }
                    else
                    {
                        value += MajorDelta;
                    }
                }
            }

            SetCurrentValue(ValueProperty, value);
        }

        private void DecreaseValue(bool minor)
        {
            var value = Value;

            CoerceValueToBounds(ref value);

            if (value <= MaxValue)
            {
                if (minor)
                {
                    if (IsValueWrapAllowed && value - MinorDelta < MinValue)
                    {
                        value = MaxValue;
                    }
                    else
                    {
                        value -= MinorDelta;
                    }
                }
                else
                {
                    if (IsValueWrapAllowed && value - MajorDelta < MinValue)
                    {
                        value = MaxValue;
                    }
                    else
                    {
                        value -= MajorDelta;
                    }
                }
            }

            SetCurrentValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject element,
            DependencyPropertyChangedEventArgs e)
        {
        }

        private static object CoerceValue(DependencyObject element, object baseValue)
        {
            var control = (NumericUpDown) element;
            var value = (decimal) baseValue;

            using (new DisposableToken<NumericUpDown>(control, x => x.Instance._isValueCoercing = true, x => x.Instance._isValueCoercing = false))
            {
                control.CoerceValueToBounds(ref value);

                var valueString = value.ToString(control._culture);
                var decimalPlaces = control.GetDecimalPlacesCount(valueString);
                if (decimalPlaces > control.DecimalPlaces)
                {
                    if (control.IsDecimalPointDynamic)
                    {
                        control.SetCurrentValue(DecimalPlacesProperty, decimalPlaces);

                        if (decimalPlaces > control.DecimalPlaces)
                        {
                            value = control.TruncateValue(valueString, control.DecimalPlaces);
                        }
                    }
                    else
                    {
                        value = control.TruncateValue(valueString, decimalPlaces);
                    }
                }
                else if (control.IsDecimalPointDynamic)
                {
                    control.SetCurrentValue(DecimalPlacesProperty, decimalPlaces);
                }

                control._textBox?.SetCurrentValue(TextBox.TextProperty, control.IsThousandSeparatorVisible
                    ? value.ToString("N", control._culture)
                    : value.ToString("F", control._culture));
            }

            return baseValue;
        }

        private static void OnMaxValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown) element;
            var maxValue = (decimal) e.NewValue;

            if (maxValue < control.MinValue)
            {
                control.SetCurrentValue(MinValueProperty, maxValue);
            }

            if (maxValue <= control.Value)
            {
                control.SetCurrentValue(ValueProperty, maxValue);
            }
        }

        private static object CoerceMaxValue(DependencyObject element, object baseValue)
        {
            var maxValue = (decimal) baseValue;
            return maxValue == decimal.MaxValue ? DependencyProperty.UnsetValue : maxValue;
        }

        private static void OnMinValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown) element;
            var minValue = (decimal) e.NewValue;

            if (minValue > control.MaxValue)
            {
                control.SetCurrentValue(MaxValueProperty, minValue);
            }

            if (minValue >= control.Value)
            {
                control.SetCurrentValue(ValueProperty, minValue);
            }
        }

        private static object CoerceMinValue(DependencyObject element, object baseValue)
        {
            var minValue = (decimal) baseValue;
            return minValue == decimal.MinValue ? DependencyProperty.UnsetValue : minValue;
        }

        private static void OnDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown) element;
            var decimalPlaces = (int) e.NewValue;

            control._culture.NumberFormat.NumberDecimalDigits = decimalPlaces;

            if (control.IsDecimalPointDynamic)
            {
                control.SetCurrentValue(IsDecimalPointDynamicProperty, false);
                control.InvalidateProperty(ValueProperty);
                control.SetCurrentValue(IsDecimalPointDynamicProperty, true);
            }
            else
            {
                control.InvalidateProperty(ValueProperty);
            }
        }

        private static object CoerceDecimalPlaces(DependencyObject element, object baseValue)
        {
            var decimalPlaces = (int) baseValue;
            var control = (NumericUpDown) element;

            if (decimalPlaces < control.MinDecimalPlaces)
            {
                decimalPlaces = control.MinDecimalPlaces;
            }
            else if (decimalPlaces > control.MaxDecimalPlaces)
            {
                decimalPlaces = control.MaxDecimalPlaces;
            }

            return decimalPlaces;
        }

        private static void OnMaxDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown) element;

            control.InvalidateProperty(DecimalPlacesProperty);
        }

        private static object CoerceMaxDecimalPlaces(DependencyObject element, object baseValue)
        {
            var maxDecimalPlaces = (int) baseValue;
            var control = (NumericUpDown) element;

            if (maxDecimalPlaces > 28)
            {
                maxDecimalPlaces = 28;
            }
            else if (maxDecimalPlaces < 0)
            {
                maxDecimalPlaces = 0;
            }
            else if (maxDecimalPlaces < control.MinDecimalPlaces)
            {
                control.SetCurrentValue(MinDecimalPlacesProperty, maxDecimalPlaces);
            }

            return maxDecimalPlaces;
        }

        private static void OnMinDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown) element;

            control.InvalidateProperty(DecimalPlacesProperty);
        }

        private static object CoerceMinDecimalPlaces(DependencyObject element, object baseValue)
        {
            var minDecimalPlaces = (int) baseValue;
            var control = (NumericUpDown) element;

            if (minDecimalPlaces < 0)
            {
                minDecimalPlaces = 0;
            }
            else if (minDecimalPlaces > 28)
            {
                minDecimalPlaces = 28;
            }
            else if (minDecimalPlaces > control.MaxDecimalPlaces)
            {
                control.SetCurrentValue(MaxDecimalPlacesProperty, minDecimalPlaces);
            }

            return minDecimalPlaces;
        }

        private static void OnMinorDeltaChanged(DependencyObject element,
            DependencyPropertyChangedEventArgs e)
        {
            var minorDelta = (decimal) e.NewValue;
            var control = (NumericUpDown) element;

            if (minorDelta > control.MajorDelta)
            {
                control.SetCurrentValue(MajorDeltaProperty, minorDelta);
            }
        }

        private static object CoerceMinorDelta(DependencyObject element, object baseValue)
        {
            var minorDelta = (decimal) baseValue;

            return minorDelta;
        }

        private static void OnMajorDeltaChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var majorDelta = (decimal) e.NewValue;
            var control = (NumericUpDown) element;

            if (majorDelta < control.MinorDelta)
            {
                control.SetCurrentValue(MinorDeltaProperty, majorDelta);
            }
        }

        private static object CoerceMajorDelta(DependencyObject element, object baseValue)
        {
            return (decimal) baseValue;
        }

        private static void OnIsThousandSeparatorVisibleChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (NumericUpDown) element;

            control.InvalidateProperty(ValueProperty);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AttachToVisualTree();
            AttachCommands();
        }

        private void TextBoxOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateValue();
        }

        private void TextBoxOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (IsAutoSelectionActive)
            {
                _textBox.SelectAll();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            InvalidateProperty(ValueProperty);
        }

        private void ButtonOnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            SetCurrentValue(ValueProperty, (decimal) 0);
        }

        private void AttachToVisualTree()
        {
            AttachTextBox();
            AttachIncreaseButton();
            AttachDecreaseButton();
        }

        private void AttachTextBox()
        {
            var textBox = GetTemplateChild("PART_TextBox") as TextBox;
            if (textBox is null)
            {
                return;
            }

            _textBox = textBox;
            _textBox.LostFocus += TextBoxOnLostFocus;
            _textBox.TextChanged += OnTextChanged;
            _textBox.PreviewMouseLeftButtonUp += TextBoxOnPreviewMouseLeftButtonUp;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //UpdateValue();
        }

        private void AttachIncreaseButton()
        {
            var increaseButton = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
            if (increaseButton is null)
            {
                return;
            }

            _increaseButton = increaseButton;
            _increaseButton.SetCurrentValue(FocusableProperty, false);
            _increaseButton.SetCurrentValue(ButtonBase.CommandProperty, MinorIncreaseValueCommand);
            _increaseButton.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            _increaseButton.PreviewMouseRightButtonDown += ButtonOnPreviewMouseRightButtonDown;
        }

        private void AttachDecreaseButton()
        {
            var decreaseButton = GetTemplateChild("PART_DecreaseButton") as RepeatButton;
            if (decreaseButton is null)
            {
                return;
            }

            _decreaseButton = decreaseButton;
            _decreaseButton.SetCurrentValue(FocusableProperty, false);
            _decreaseButton.SetCurrentValue(ButtonBase.CommandProperty, MinorDecreaseValueCommand);
            _decreaseButton.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            _decreaseButton.PreviewMouseRightButtonDown += ButtonOnPreviewMouseRightButtonDown;
        }

        private void AttachCommands()
        {
            CommandBindings.Add(new CommandBinding(MinorIncreaseValueCommand, (a, b) => IncreaseValue(true)));
            CommandBindings.Add(new CommandBinding(MinorDecreaseValueCommand, (a, b) => DecreaseValue(true)));
            CommandBindings.Add(new CommandBinding(MajorIncreaseValueCommand, (a, b) => IncreaseValue(false)));
            CommandBindings.Add(new CommandBinding(MajorDecreaseValueCommand, (a, b) => DecreaseValue(false)));
            CommandBindings.Add(new CommandBinding(UpdateValueStringCommand, (a, b) => UpdateValue()));
            CommandBindings.Add(new CommandBinding(CancelChangesCommand, (a, b) => CancelChanges()));

            CommandManager.RegisterClassInputBinding(typeof(TextBox), new KeyBinding(MinorIncreaseValueCommand, new KeyGesture(Key.Up)));
            CommandManager.RegisterClassInputBinding(typeof(TextBox), new KeyBinding(MinorDecreaseValueCommand, new KeyGesture(Key.Down)));
            CommandManager.RegisterClassInputBinding(typeof(TextBox), new KeyBinding(MajorIncreaseValueCommand, new KeyGesture(Key.PageUp)));
            CommandManager.RegisterClassInputBinding(typeof(TextBox), new KeyBinding(MajorDecreaseValueCommand, new KeyGesture(Key.PageDown)));
            CommandManager.RegisterClassInputBinding(typeof(TextBox), new KeyBinding(UpdateValueStringCommand, new KeyGesture(Key.Enter)));
            CommandManager.RegisterClassInputBinding(typeof(TextBox), new KeyBinding(CancelChangesCommand, new KeyGesture(Key.Escape)));
        }

        private static decimal ParseStringToDecimal(string source)
        {
            decimal.TryParse(source, out var value);
            return value;
        }

        private int GetDecimalPlacesCount(string valueString)
        {
            return valueString.SkipWhile(c => c.ToString(_culture) != _culture.NumberFormat.NumberDecimalSeparator)
                .Skip(1)
                .Count();
        }

        private decimal TruncateValue(string valueString, int decimalPlaces)
        {
            var endPoint = valueString.Length - (decimalPlaces - DecimalPlaces);
            endPoint++;

            var tempValueString = valueString.Substring(0, endPoint);

            return decimal.Parse(tempValueString, _culture);
        }
        #endregion
    }
}
