namespace Orc.Controls;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Catel;
using Catel.Logging;
using Catel.MVVM;
using Orc.Automation;

[TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
[TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
[TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
[TemplatePart(Name = "PART_SpinButton", Type = typeof(SpinButton))]
public class NumericUpDown : Control
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private const int MaxPossibleDecimalPlaces = 28;

    private readonly CultureInfo _culture;

    private SpinButton? _spinButton;
    private TextBox? _textBox;

    private Command? _decreaseCommand;
    private Command? _increaseCommand;

    private bool _isValueChanging;
    private bool _isValueCoercing;
    private bool _isValueInput;

    static NumericUpDown()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
    }

    public NumericUpDown()
    {
        _culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        _culture.NumberFormat.NumberDecimalDigits = DecimalPlaces;

        Loaded += OnLoaded;
    }

    #region Dependency properties
    public Number Value
    {
        get { return (Number)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Number), typeof(NumericUpDown),
        new PropertyMetadata((Number)0d, (sender, _) => ((NumericUpDown)sender).OnValueChanged(), (o, value) => ((NumericUpDown)o).CoerceValue(value)));

    public double MaxValue
    {
        get { return (double)GetValue(MaxValueProperty); }
        set { SetValue(MaxValueProperty, value); }
    }

    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(NumericUpDown),
        new PropertyMetadata(double.MaxValue, (sender, args) => ((NumericUpDown)sender).OnMaxValueChanged(args), (o, value) => ((NumericUpDown)o).CoerceMaxValue(value)));

    public double MinValue
    {
        get { return (double)GetValue(MinValueProperty); }
        set { SetValue(MinValueProperty, value); }
    }

    public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(NumericUpDown),
        new PropertyMetadata(double.MinValue, (sender, args) => ((NumericUpDown)sender).OnMinValueChanged(args), (o, value) => ((NumericUpDown)o).CoerceMinValue(value)));

    public int DecimalPlaces
    {
        get { return (int)GetValue(DecimalPlacesProperty); }
        set { SetValue(DecimalPlacesProperty, value); }
    }

    public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(nameof(DecimalPlaces), typeof(int), typeof(NumericUpDown),
        new PropertyMetadata(0, OnDecimalPlacesChanged, CoerceDecimalPlaces));

    public int MaxDecimalPlaces
    {
        get { return (int)GetValue(MaxDecimalPlacesProperty); }
        set { SetValue(MaxDecimalPlacesProperty, value); }
    }

    public static readonly DependencyProperty MaxDecimalPlacesProperty = DependencyProperty.Register(nameof(MaxDecimalPlaces), typeof(int), typeof(NumericUpDown),
        new PropertyMetadata(MaxPossibleDecimalPlaces, OnMaxDecimalPlacesChanged, CoerceMaxDecimalPlaces));

    public int MinDecimalPlaces
    {
        get { return (int)GetValue(MinDecimalPlacesProperty); }
        set { SetValue(MinDecimalPlacesProperty, value); }
    }

    public static readonly DependencyProperty MinDecimalPlacesProperty = DependencyProperty.Register(nameof(MinDecimalPlaces), typeof(int), typeof(NumericUpDown),
        new PropertyMetadata(0, OnMinDecimalPlacesChanged, CoerceMinDecimalPlaces));

    public bool IsDecimalPointDynamic
    {
        get { return (bool)GetValue(IsDecimalPointDynamicProperty); }
        set { SetValue(IsDecimalPointDynamicProperty, value); }
    }

    public static readonly DependencyProperty IsDecimalPointDynamicProperty = DependencyProperty.Register(nameof(IsDecimalPointDynamic), typeof(bool), typeof(NumericUpDown),
        new PropertyMetadata(false));

    public double MinorDelta
    {
        get { return (double)GetValue(MinorDeltaProperty); }
        set { SetValue(MinorDeltaProperty, value); }
    }

    public static readonly DependencyProperty MinorDeltaProperty = DependencyProperty.Register(nameof(MinorDelta), typeof(double), typeof(NumericUpDown),
        new PropertyMetadata(1d, OnMinorDeltaChanged, CoerceMinorDelta));

    public double MajorDelta
    {
        get { return (double)GetValue(MajorDeltaProperty); }
        set { SetValue(MajorDeltaProperty, value); }
    }

    public static readonly DependencyProperty MajorDeltaProperty = DependencyProperty.Register(nameof(MajorDelta), typeof(double), typeof(NumericUpDown),
        new PropertyMetadata(10d, OnMajorDeltaChanged, CoerceMajorDelta));

    public bool IsThousandSeparatorVisible
    {
        get { return (bool)GetValue(IsThousandSeparatorVisibleProperty); }
        set { SetValue(IsThousandSeparatorVisibleProperty, value); }
    }

    public static readonly DependencyProperty IsThousandSeparatorVisibleProperty = DependencyProperty.Register(nameof(IsThousandSeparatorVisible), typeof(bool), typeof(NumericUpDown),
        new PropertyMetadata(false, OnIsThousandSeparatorVisibleChanged));

    public bool IsAutoSelectionActive
    {
        get { return (bool)GetValue(IsAutoSelectionActiveProperty); }
        set { SetValue(IsAutoSelectionActiveProperty, value); }
    }

    public static readonly DependencyProperty IsAutoSelectionActiveProperty = DependencyProperty.Register(nameof(IsAutoSelectionActive), typeof(bool), typeof(NumericUpDown),
        new PropertyMetadata(true));
    #endregion

    private double ActualMinValue => Math.Max(MinValue, Value.MinValue);
    private double ActualMaxValue => Math.Min(MaxValue, Value.MaxValue);

    [MemberNotNullWhen(true, nameof(_textBox),
        nameof(_spinButton), nameof(_increaseCommand), 
        nameof(_decreaseCommand))]
    private bool IsPartsInitialized { get; set; }

    public event EventHandler<EventArgs>? TextChanged;

    public override void OnApplyTemplate()
    {
        _textBox = GetTemplateChild("PART_TextBox") as TextBox;
        if (_textBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_TextBox'");
        }

        _textBox.LostFocus += TextBoxOnLostFocus;
        _textBox.PreviewMouseLeftButtonUp += TextBoxOnPreviewMouseLeftButtonUp;
        _textBox.TextChanged += OnTextChanged;

        _spinButton = GetTemplateChild("PART_SpinButton") as SpinButton;
        if (_spinButton is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SpinButton'");
        }

        _increaseCommand = new Command(() => ChangeValue(MinorDelta), () => Value.DValue + MinorDelta <= ActualMaxValue);
        _decreaseCommand = new Command(() => ChangeValue((-1) * MinorDelta), () => Value.DValue - MinorDelta >= ActualMinValue);

        _spinButton.Canceled += (_, _) => Cancel();
        _spinButton.SetCurrentValue(SpinButton.IncreaseProperty, _increaseCommand);
        _spinButton.SetCurrentValue(SpinButton.DecreaseProperty, _decreaseCommand);
        _spinButton.PreviewMouseLeftButtonDown += (_, _) => RemoveFocus();

        IsPartsInitialized = true;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!_isValueChanging)
        {
            using (new DisposableToken<NumericUpDown>(this, x => x.Instance._isValueInput = true, x => x.Instance._isValueInput = false))
            {
                UpdateValue();
            }
        }

        TextChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateValue()
    {
        if (!IsPartsInitialized || _isValueCoercing)
        {
            return;
        }
            
        var text = _textBox.Text;
        if (!double.TryParse(text, out var value))
        {
            Cancel();

            return;
        }

        var number = (Number) CoerceValue(new Number(value, Value.Type));

        SetCurrentValue(ValueProperty, number);

        if (value < ActualMinValue || value > ActualMaxValue)
        {
            UpdateTextByValue();
        }
    }

    private void UpdateTextByValue()
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        var previousCaretIndex = _textBox.CaretIndex;

        var coercedNumberText = IsThousandSeparatorVisible
            ? Value.DValue.ToString("N", _culture)
            : Value.DValue.ToString("F", _culture);

        _textBox.SetCurrentValue(TextBox.TextProperty, coercedNumberText);

        _textBox.CaretIndex = previousCaretIndex;
    }
        
    private void ChangeValue(double delta)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        using (new DisposableToken<NumericUpDown>(this, x => x.Instance._isValueChanging = true, x => x.Instance._isValueChanging = false))
        {
            var routedEvent = delta > 0 ? SpinButton.IncreasedEvent : SpinButton.DecreasedEvent;

            _spinButton.RaiseEvent(new RoutedEventArgs(routedEvent));

            SetCurrentValue(ValueProperty, Value + delta);
        }
    }

    private void RemoveFocus()
    {
        SetCurrentValue(FocusableProperty, true);
        Focus();
        SetCurrentValue(FocusableProperty, false);
    }

    private void OnValueChanged()
    {
        _increaseCommand?.RaiseCanExecuteChanged();
        _decreaseCommand?.RaiseCanExecuteChanged();
    }

    private object CoerceValue(object baseValue)
    {
        if (baseValue is not Number number)
        {
            return baseValue;
        }

        var value = number.DValue;

        using (new DisposableToken<NumericUpDown>(this, x => x.Instance._isValueCoercing = true, x => x.Instance._isValueCoercing = false))
        {
            if (value < MinValue)
            {
                value = MinValue;
            }

            if (value > MaxValue)
            {
                value = MaxValue;
            }

            var valueString = value.ToString(_culture);
            if (number.Type?.IsFloatingPointType() != true)
            {
                _culture.NumberFormat.NumberDecimalDigits = 0;
            }
            else
            {
                var decimalPlaces = GetDecimalPlacesCount(valueString);
                if (decimalPlaces > DecimalPlaces)
                {
                    if (IsDecimalPointDynamic)
                    {
                        SetCurrentValue(DecimalPlacesProperty, decimalPlaces);

                        if (decimalPlaces > DecimalPlaces)
                        {
                            value = TruncateValue(valueString, DecimalPlaces);
                        }
                    }
                    else
                    {
                        value = TruncateValue(valueString, decimalPlaces);
                    }
                }
                else if (IsDecimalPointDynamic)
                {
                    SetCurrentValue(DecimalPlacesProperty, decimalPlaces);
                }
            }

            if (!_isValueInput)
            {
                _textBox?.SetCurrentValue(TextBox.TextProperty, IsThousandSeparatorVisible
                    ? value.ToString("N", _culture)
                    : value.ToString("F", _culture));
            }
        }

        return new Number(value, number.Type);
    }

    private void OnMaxValueChanged(DependencyPropertyChangedEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        var maxValue = (double)e.NewValue;

        if (maxValue < MinValue)
        {
            SetCurrentValue(MinValueProperty, maxValue);
        }

        var value = Value;
        if (maxValue <= value)
        {
            SetCurrentValue(ValueProperty, new Number(maxValue, value.Type));
        }

        _increaseCommand.RaiseCanExecuteChanged();
        _decreaseCommand.RaiseCanExecuteChanged();
    }

    private object CoerceMaxValue(object baseValue)
    {
        var value = Value;
        var maxValue = value.MaxValue;
        var baseNumber = (double)baseValue;

        return baseNumber >= maxValue ? maxValue : baseNumber;
    }

    private object CoerceMinValue(object baseValue)
    {
        var value = Value;
        var minValue = value.MinValue;
        var baseNumber = (double)baseValue;

        return baseNumber <= minValue ? minValue : baseNumber;
    }

    private void OnMinValueChanged(DependencyPropertyChangedEventArgs e)
    {
        var minValue = (double)e.NewValue;

        if (minValue > MaxValue)
        {
            SetCurrentValue(MaxValueProperty, minValue);
        }

        var value = Value;
        if (minValue >= value)
        {
            SetCurrentValue(ValueProperty, new Number(minValue, value.Type));
        }

        _increaseCommand?.RaiseCanExecuteChanged();
        _decreaseCommand?.RaiseCanExecuteChanged();
    }

    private static void OnDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        var control = (NumericUpDown)element;
        var decimalPlaces = (int)e.NewValue;

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

    private static object? CoerceDecimalPlaces(DependencyObject element, object? baseValue)
    {
        if (baseValue is null)
        {
            return 0;
        }

        var decimalPlaces = (int)baseValue;
        var control = (NumericUpDown)element;

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
        var control = (NumericUpDown)element;

        control.InvalidateProperty(DecimalPlacesProperty);
    }

    private static object CoerceMaxDecimalPlaces(DependencyObject element, object? baseValue)
    {
        if (baseValue is null)
        {
            return 0;
        }

        var maxDecimalPlaces = (int)baseValue;
        var control = (NumericUpDown)element;

        switch (maxDecimalPlaces)
        {
            case > MaxPossibleDecimalPlaces:
                maxDecimalPlaces = MaxPossibleDecimalPlaces;
                break;

            case < 0:
                maxDecimalPlaces = 0;
                break;

            default:
                {
                    if (maxDecimalPlaces < control.MinDecimalPlaces)
                    {
                        control.SetCurrentValue(MinDecimalPlacesProperty, maxDecimalPlaces);
                    }

                    break;
                }
        }

        return maxDecimalPlaces;
    }

    private static void OnMinDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        var control = (NumericUpDown)element;

        control.InvalidateProperty(DecimalPlacesProperty);
    }

    private static object CoerceMinDecimalPlaces(DependencyObject element, object? baseValue)
    {
        if (baseValue is null)
        {
            return 0;
        }

        var minDecimalPlaces = (int)baseValue;
        var control = (NumericUpDown)element;

        switch (minDecimalPlaces)
        {
            case < 0:
                minDecimalPlaces = 0;
                break;

            case > MaxPossibleDecimalPlaces:
                minDecimalPlaces = MaxPossibleDecimalPlaces;
                break;

            default:
                {
                    if (minDecimalPlaces > control.MaxDecimalPlaces)
                    {
                        control.SetCurrentValue(MaxDecimalPlacesProperty, minDecimalPlaces);
                    }

                    break;
                }
        }

        return minDecimalPlaces;
    }

    private static void OnMinorDeltaChanged(DependencyObject element,
        DependencyPropertyChangedEventArgs e)
    {
        var minorDelta = (double)e.NewValue;
        var control = (NumericUpDown)element;

        if (minorDelta > control.MajorDelta)
        {
            control.SetCurrentValue(MajorDeltaProperty, minorDelta);
        }
    }

    private static object CoerceMinorDelta(DependencyObject element, object? baseValue)
    {
        if (baseValue is null)
        {
            return 0;
        }

        var minorDelta = (double)baseValue;

        return minorDelta;
    }

    private static void OnMajorDeltaChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        var majorDelta = (double)e.NewValue;
        var control = (NumericUpDown)element;

        if (majorDelta < control.MinorDelta)
        {
            control.SetCurrentValue(MinorDeltaProperty, majorDelta);
        }
    }

    private static object CoerceMajorDelta(DependencyObject element, object? baseValue)
    {
        if (baseValue is null)
        {
            return 0;
        }

        return (double)baseValue;
    }

    private static void OnIsThousandSeparatorVisibleChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        var control = (NumericUpDown)element;

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
            _textBox?.SelectAll();
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
        InvalidateProperty(ValueProperty);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                Cancel();

                e.Handled = true;

                return;

            case Key.Enter:
                UpdateValue();

                e.Handled = true;
                break;
        }
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Up:
                _increaseCommand.Execute(null);
                break;

            case Key.Down:
                _decreaseCommand.Execute(null);
                break;

            case Key.PageDown:
                //   delta = MajorDelta;
                break;

            case Key.PageUp:
                //    delta = (-1) * MajorDelta;
                break;

            default:
                return;
        }

        //ChangeValue(delta);
        e.Handled = true;
    }

    private void Cancel()
    {
        SetCurrentValue(ValueProperty, new Number(0d, Value.Type));
    }

    private int GetDecimalPlacesCount(string valueString)
    {
        return valueString.SkipWhile(c => c.ToString(_culture) != _culture.NumberFormat.NumberDecimalSeparator)
            .Skip(1)
            .Count();
    }

    private double TruncateValue(string valueString, int decimalPlaces)
    {
        var endPoint = valueString.Length - (decimalPlaces - DecimalPlaces);
        endPoint++;

        var tempValueString = valueString[..endPoint];

        return double.Parse(tempValueString, _culture);
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new NumericUpDownAutomationPeer(this);
    }
}
