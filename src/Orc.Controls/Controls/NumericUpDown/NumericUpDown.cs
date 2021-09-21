namespace Orc.Controls
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;

    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_SpinButton", Type = typeof(SpinButton))]
    public class NumericUpDown : Control
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const int MaxPossibleDecimalPlaces = 28;

        private readonly CultureInfo _culture;

        private TextBox _textBox;
        private SpinButton _spinButton;

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
            new PropertyMetadata(MaxPossibleDecimalPlaces, OnMaxDecimalPlacesChanged, CoerceMaxDecimalPlaces));

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
            new PropertyMetadata(true));
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            if (_textBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_TextBox'");
            }
            _textBox.LostFocus += TextBoxOnLostFocus;
            _textBox.PreviewMouseLeftButtonUp += TextBoxOnPreviewMouseLeftButtonUp;
            
            _spinButton = GetTemplateChild("PART_SpinButton") as SpinButton;
            if (_spinButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SpinButton'");
            }
            _spinButton.Canceled += (_, _) => Cancel();
            _spinButton.SetCurrentValue(SpinButton.IncreaseProperty, new Command(() => ChangeValue(MinorDelta), () => true));
            _spinButton.SetCurrentValue(SpinButton.DecreaseProperty, new Command(() => ChangeValue((-1) * MinorDelta), () => true));
            _spinButton.PreviewMouseLeftButtonDown += (_, _) => RemoveFocus();
        }

        private void UpdateValue()
        {
            if (_isValueCoercing)
            {
                return;
            }

            SetCurrentValue(ValueProperty, ParseStringToDecimal(_textBox.Text));
        }

        private void ChangeValue(decimal delta)
        {
            var routedEvent = delta > 0 ? SpinButton.IncreasedEvent : SpinButton.DecreasedEvent;

            _spinButton.RaiseEvent(new RoutedEventArgs(routedEvent));

            SetCurrentValue(ValueProperty, Value + delta);
        }

        private void CoerceValueToBounds(ref decimal value)
        {
            if (value < MinValue)
            {
                value = MinValue;
            }

            if (value > MaxValue)
            {
                value = MaxValue;
            }
        }

        private void RemoveFocus()
        {
            SetCurrentValue(FocusableProperty, true);
            Focus();
            SetCurrentValue(FocusableProperty, false);
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

            return value;
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

            if (maxDecimalPlaces > MaxPossibleDecimalPlaces)
            {
                maxDecimalPlaces = MaxPossibleDecimalPlaces;
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
            else if (minDecimalPlaces > MaxPossibleDecimalPlaces)
            {
                minDecimalPlaces = MaxPossibleDecimalPlaces;
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Cancel();

                e.Handled = true;

                return;
            }

            if (e.Key == Key.Enter)
            {
                UpdateValue();

                e.Handled = true;

                return;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            decimal delta;
            switch (e.Key)
            {
                case Key.Up:
                    delta = MinorDelta;
                    break;

                case Key.Down:
                    delta = (-1) * MinorDelta;
                    break;

                case Key.PageDown:
                    delta = MajorDelta;
                    break;

                case Key.PageUp:
                    delta = (-1) * MajorDelta;
                    break;

                default:
                    return;
            }

            ChangeValue(delta);
            e.Handled = true;
        }

        private void Cancel()
        {
            SetCurrentValue(ValueProperty, (decimal)0);
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
