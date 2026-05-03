namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Catel;
using Catel.Windows.Interactivity;

public partial class NumericTextBoxAdapterBehavior : BehaviorBase<NumericTextBox>
{
    private static readonly Dictionary<Type, NumericTextBoxTypeDescription> TypeDescriptions = new()
    {
        {
            typeof(sbyte), new NumericTextBoxTypeDescription(sbyte.MinValue, sbyte.MaxValue, (x, c) => System.Convert.ToSByte(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = false,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(sbyte?), new NumericTextBoxTypeDescription(sbyte.MinValue, sbyte.MaxValue, (x, c) => System.Convert.ToSByte(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = true,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(byte), new NumericTextBoxTypeDescription(byte.MinValue, byte.MaxValue, (x, c) => System.Convert.ToByte(x, c))
            {
                IsNegativeAllowed = false,
                IsNullable = false,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(byte?), new NumericTextBoxTypeDescription(byte.MinValue, byte.MaxValue, (x, c) => System.Convert.ToByte(x, c))
            {
                IsNegativeAllowed = false,
                IsNullable = true,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(short), new NumericTextBoxTypeDescription(short.MinValue, short.MaxValue, (x, c) => System.Convert.ToInt16(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = false,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(short?), new NumericTextBoxTypeDescription(short.MinValue, short.MaxValue, (x, c) => System.Convert.ToInt16(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = true,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(ushort), new NumericTextBoxTypeDescription(ushort.MinValue, ushort.MaxValue, (x, c) => System.Convert.ToUInt16(x, c))
            {
                IsNegativeAllowed = false,
                IsNullable = false,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(ushort?), new NumericTextBoxTypeDescription(ushort.MinValue, ushort.MaxValue, (x, c) => System.Convert.ToUInt16(x, c))
            {
                IsNegativeAllowed = false,
                IsNullable = true,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(int), new NumericTextBoxTypeDescription(int.MinValue, int.MaxValue, (x, c) => System.Convert.ToInt32(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = false,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(int?), new NumericTextBoxTypeDescription(int.MinValue, int.MaxValue, (x, c) => System.Convert.ToInt32(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = true,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(uint), new NumericTextBoxTypeDescription(uint.MinValue, uint.MaxValue, (x, c) => System.Convert.ToUInt32(x, c))
            {
                IsNegativeAllowed = false,
                IsNullable = false,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(uint?), new NumericTextBoxTypeDescription(uint.MinValue, uint.MaxValue,(x, c) => System.Convert.ToUInt32(x, c))
            {
                IsNegativeAllowed = false, 
                IsNullable = true, 
                IsDecimalAllowed = false
            }
        },
        {
            typeof(long), new NumericTextBoxTypeDescription(long.MinValue, long.MaxValue,(x, c) => System.Convert.ToInt64(x, c))
            {
                IsNegativeAllowed = true, 
                IsNullable = false, 
                IsDecimalAllowed = false
            }
        },
        {
            typeof(long?), new NumericTextBoxTypeDescription(long.MinValue, long.MaxValue,(x, c) => System.Convert.ToInt64(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = true,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(ulong), new NumericTextBoxTypeDescription(ulong.MinValue, ulong.MaxValue,(x, c) => System.Convert.ToUInt64(x, c))
            {
                IsNegativeAllowed = false, 
                IsNullable = false,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(ulong?), new NumericTextBoxTypeDescription(ulong.MinValue, ulong.MaxValue,(x, c) => System.Convert.ToUInt64(x, c))
            {
                IsNegativeAllowed = false,
                IsNullable = true,
                IsDecimalAllowed = false
            }
        },
        {
            typeof(double), new NumericTextBoxTypeDescription(double.MinValue, double.MaxValue, (x, c) => System.Convert.ToDouble(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = false,
                IsDecimalAllowed = true
            }
        },
        {
            typeof(double?), new NumericTextBoxTypeDescription(double.MinValue, double.MaxValue,(x, c) => System.Convert.ToDouble(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = true,
                IsDecimalAllowed = true
            }
        },
        {
            typeof(float), new NumericTextBoxTypeDescription(float.MinValue, float.MaxValue,(x, c) => System.Convert.ToSingle(x, c))
            {
                IsNegativeAllowed = true, 
                IsNullable = false, 
                IsDecimalAllowed = true
            }
        },
        {
            typeof(float?), new NumericTextBoxTypeDescription(float.MinValue, float.MaxValue,(x, c) => System.Convert.ToSingle(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = true,
                IsDecimalAllowed = true
            }
        },
        {
            typeof(decimal), new NumericTextBoxTypeDescription(decimal.MinValue, decimal.MaxValue,(x, c) => System.Convert.ToDecimal(x, c))
            {
                IsNegativeAllowed = true, 
                IsNullable = false,
                IsDecimalAllowed = true
            }
        },
        {
            typeof(decimal?), new NumericTextBoxTypeDescription(decimal.MinValue, decimal.MaxValue,(x, c) => System.Convert.ToDecimal(x, c))
            {
                IsNegativeAllowed = true,
                IsNullable = true,
                IsDecimalAllowed = true
            }
        },
    };

    private NumericTextBoxTypeDescription? _typeDescription;

    private bool _isControlValueChanging;
    private bool _isValueChanging;

    #region Dependency properties
    public Type ValueType
    {
        get { return (Type)GetValue(ValueTypeProperty); }
        set { SetValue(ValueTypeProperty, value); }
    }

    public static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register(
        nameof(ValueType), typeof(Type), typeof(NumericTextBoxAdapterBehavior), new PropertyMetadata(typeof(double?)));

    public object? Value
    {
        get { return (object?)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(object), typeof(NumericTextBoxAdapterBehavior), new FrameworkPropertyMetadata(default(double?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (sender, args) => ((NumericTextBoxAdapterBehavior)sender).OnValueChanged(args)));
    #endregion

    protected override void OnAssociatedObjectLoaded()
    {
        var numericTextBox = AssociatedObject;

        if (TypeDescriptions.TryGetValue(ValueType, out _typeDescription))
        {
            numericTextBox.SetCurrentValue(NumericTextBox.IsNullValueAllowedProperty, _typeDescription.IsNullable);
            numericTextBox.SetCurrentValue(NumericTextBox.IsNegativeAllowedProperty, _typeDescription.IsNegativeAllowed);
            numericTextBox.SetCurrentValue(NumericTextBox.IsDecimalAllowedProperty, _typeDescription.IsDecimalAllowed);

            numericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, ConvertBack(_typeDescription.MaxValue) ?? double.MaxValue);
            numericTextBox.SetCurrentValue(NumericTextBox.MinValueProperty, ConvertBack(_typeDescription.MinValue) ?? double.MinValue);
        }

        numericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, ConvertBack(Value));

        numericTextBox.ValueChanged += OnNumericTextBoxValueChanged;
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        var numericTextBox = AssociatedObject;

        numericTextBox.ValueChanged -= OnNumericTextBoxValueChanged;
    }

    private void OnNumericTextBoxValueChanged(object? sender, EventArgs args)
    {
        if (_isValueChanging || _isControlValueChanging)
        {
            return;
        }

        using (new DisposableToken<NumericTextBoxAdapterBehavior>(this, x => x.Instance._isValueChanging = true,
                   x => x.Instance._isValueChanging = false))
        {
            SetCurrentValue(ValueProperty, Convert(AssociatedObject.Value));
        }
    }

    protected virtual double? ConvertBack(object? value)
    {
        return value is not null ? System.Convert.ToDouble(value, GetCulture()) : null;
    }

    protected virtual object? Convert(double? value)
    {
        return value is null 
            ? default 
            : _typeDescription?.ConvertFunc.Invoke(value, GetCulture());
    }

    private void OnValueChanged(DependencyPropertyChangedEventArgs args)
    {
        var numericTextBox = AssociatedObject;
        if (numericTextBox is null)
        {
            return;
        }

        if (_isValueChanging || _isControlValueChanging)
        {
            return;
        }

        using (new DisposableToken<NumericTextBoxAdapterBehavior>(this, x => x.Instance._isControlValueChanging = true, x => x.Instance._isControlValueChanging = false))
        {
            numericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, ConvertBack(args.NewValue));
        }
    }

    private CultureInfo GetCulture()
    {
        return AssociatedObject.CultureInfo ?? CultureInfo.CurrentCulture;
    }

    #region Nested type: NumericTextBoxTypeDescription
    private class NumericTextBoxTypeDescription
    {
        public NumericTextBoxTypeDescription(object minValue, object maxValue, Func<double?, CultureInfo, object> convertFunc)
        {
            ArgumentNullException.ThrowIfNull(minValue);
            ArgumentNullException.ThrowIfNull(maxValue);
            ArgumentNullException.ThrowIfNull(convertFunc);

            MaxValue = maxValue;
            MinValue = minValue;
            ConvertFunc = convertFunc;
        }

        public object MaxValue { get; }
        public object MinValue { get; }
        public Func<double?, CultureInfo, object> ConvertFunc { get; }
        public bool IsNullable { get; init; }
        public bool IsNegativeAllowed { get; init; }
        public bool IsDecimalAllowed { get; init; }
    }
    #endregion
}
