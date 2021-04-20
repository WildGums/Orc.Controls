// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SByteNumericTextBoxAdapterBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using Catel;
    using Catel.Windows.Interactivity;

    public class NumericTextBoxAdapterBehavior : BehaviorBase<NumericTextBox>
    {
        private class NumericTextBoxTypeDescription
        {
            public object MaxValue { get; set; }
            public object MinValue { get; set; }
            public bool IsNullable { get; set; }
            public bool IsNegativeAllowed { get; set; }
            public bool IsDecimalAllowed { get; set; }
            public Func<double?, CultureInfo, object> ConvertFunc { get; set; }
        }

        #region Fields
        private static readonly Dictionary<Type, NumericTextBoxTypeDescription> TypeDescriptions = new Dictionary<Type, NumericTextBoxTypeDescription>()
        {
            {
                typeof(sbyte),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = sbyte.MaxValue,
                    MinValue = sbyte.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = false,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToSByte(x, c),
                }
            },

            {
                typeof(sbyte?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = sbyte.MaxValue,
                    MinValue = sbyte.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = true,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToSByte(x, c),
                }
            },

            {
                typeof(byte),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = byte.MaxValue,
                    MinValue = byte.MinValue,
                    IsNegativeAllowed = false,
                    IsNullable = false,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToByte(x, c),
                }
            },

            {
                typeof(byte?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = byte.MaxValue,
                    MinValue = byte.MinValue,
                    IsNegativeAllowed = false,
                    IsNullable = true,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToByte(x, c),
                }
            },

            {
                typeof(short),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = short.MaxValue,
                    MinValue = short.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = false,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToInt16(x, c),
                }
            },

            {
                typeof(short?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = short.MaxValue,
                    MinValue = short.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = true,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToInt16(x, c),
                }
            },

            {
                typeof(ushort),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = ushort.MaxValue,
                    MinValue = ushort.MinValue,
                    IsNegativeAllowed = false,
                    IsNullable = false,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToUInt16(x, c),
                }
            },

            {
                typeof(ushort?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = ushort.MaxValue,
                    MinValue = ushort.MinValue,
                    IsNegativeAllowed = false,
                    IsNullable = true,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToUInt16(x, c),
                }
            },

            {
                typeof(int),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = int.MaxValue,
                    MinValue = int.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = false,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToInt32(x, c),
                }
            },

            {
                typeof(int?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = int.MaxValue,
                    MinValue = int.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = true,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToInt32(x, c),
                }
            },

            {
                typeof(uint),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = uint.MaxValue,
                    MinValue = uint.MinValue,
                    IsNegativeAllowed = false,
                    IsNullable = false,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToUInt32(x, c),
                }
            },

            {
                typeof(uint?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = uint.MaxValue,
                    MinValue = uint.MinValue,
                    IsNegativeAllowed = false,
                    IsNullable = true,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToUInt32(x, c),
                }
            },

            {
                typeof(long),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = long.MaxValue,
                    MinValue = long.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = false,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToInt64(x, c),
                }
            },

            {
                typeof(long?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = long.MaxValue,
                    MinValue = long.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = true,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToInt64(x, c),
                }
            },

            {
                typeof(ulong),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = ulong.MaxValue,
                    MinValue = ulong.MinValue,
                    IsNegativeAllowed = false,
                    IsNullable = false,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToUInt64(x, c),
                }
            },

            {
                typeof(ulong?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = ulong.MaxValue,
                    MinValue = ulong.MinValue,
                    IsNegativeAllowed = false,
                    IsNullable = true,
                    IsDecimalAllowed = false,
                    ConvertFunc = (x, c) => System.Convert.ToUInt64(x, c),
                }
            },

            {
                typeof(double),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = double.MaxValue,
                    MinValue = double.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = false,
                    IsDecimalAllowed = true,
                    ConvertFunc = (x, c) => System.Convert.ToDouble(x, c),
                }
            },

            {
                typeof(double?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = double.MaxValue,
                    MinValue = double.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = true,
                    IsDecimalAllowed = true,
                    ConvertFunc = (x, c) => System.Convert.ToDouble(x, c),
                }
            },

            {
                typeof(float),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = float.MaxValue,
                    MinValue = float.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = false,
                    IsDecimalAllowed = true,
                    ConvertFunc = (x, c) => System.Convert.ToSingle(x, c),
                }
            },

            {
                typeof(float?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = float.MaxValue,
                    MinValue = float.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = true,
                    IsDecimalAllowed = true,
                    ConvertFunc = (x, c) => System.Convert.ToSingle(x, c),
                }
            },

            {
                typeof(decimal),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = decimal.MaxValue,
                    MinValue = decimal.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = false,
                    IsDecimalAllowed = true,
                    ConvertFunc = (x, c) => System.Convert.ToDecimal(x, c),
                }
            },

            {
                typeof(decimal?),
                new NumericTextBoxTypeDescription
                {
                    MaxValue = decimal.MaxValue,
                    MinValue = decimal.MinValue,
                    IsNegativeAllowed = true,
                    IsNullable = true,
                    IsDecimalAllowed = true,
                    ConvertFunc = (x, c) => System.Convert.ToDecimal(x, c),
                }
            },
        };

        private bool _isControlValueChanging = false;
        private bool _isValueChanging = false;
        private NumericTextBoxTypeDescription _typeDescription;
        #endregion

        #region Dependency properties
        public Type ValueType
        {
            get { return (Type)GetValue(ValueTypeProperty); }
            set { SetValue(ValueTypeProperty, value); }
        }

        public static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register(
            nameof(ValueType), typeof(Type), typeof(NumericTextBoxAdapterBehavior), new PropertyMetadata(default(Type)));


        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(object), typeof(NumericTextBoxAdapterBehavior), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, args) => ((NumericTextBoxAdapterBehavior)sender).OnValueChanged(args)));
        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            var numericTextBox = AssociatedObject;

            if (TypeDescriptions.TryGetValue(ValueType, out _typeDescription))
            {
                numericTextBox.SetCurrentValue(NumericTextBox.IsNullValueAllowedProperty, _typeDescription.IsNullable);
                numericTextBox.SetCurrentValue(NumericTextBox.IsNegativeAllowedProperty, _typeDescription.IsNegativeAllowed);
                numericTextBox.SetCurrentValue(NumericTextBox.IsDecimalAllowedProperty, _typeDescription.IsDecimalAllowed);

                numericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)ConvertBack(_typeDescription.MaxValue));
                numericTextBox.SetCurrentValue(NumericTextBox.MinValueProperty, (double)ConvertBack(_typeDescription.MinValue));
            }

            numericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, ConvertBack(Value));

            numericTextBox.ValueChanged += OnNumericTextBoxValueChanged;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            var numericTextBox = AssociatedObject;

            numericTextBox.ValueChanged -= OnNumericTextBoxValueChanged;
        }

        private void OnNumericTextBoxValueChanged(object sender, EventArgs args)
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

        protected virtual double? ConvertBack(object value)
        {
            return value is not null ? System.Convert.ToDouble(value, GetCulture()) : (double?)null;
        }

        protected virtual object Convert(double? value)
        {
            if (value is null)
            {
                return default;
            }

            return _typeDescription.ConvertFunc.Invoke(value, GetCulture());
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
        #endregion
    }
}
