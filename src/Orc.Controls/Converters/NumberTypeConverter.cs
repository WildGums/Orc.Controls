namespace Orc.Controls.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class NumberTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string)
                   || sourceType.TryGetNumberRange() is not null;
        }

        public override object? ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object? value)
        {
            if (value is string str)
            {
                return new Number(Convert.ToDouble(str));
            }

            return new Number(value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string)
                   || destinationType.TryGetNumberRange() is not null;
        }

        public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object? value, Type destinationType)
        {
            if (value is not Number number)
            {
                return null;
            }

            if (destinationType == typeof(string))
            {
                return number.DValue.ToString();
            }

            return number.ConvertTo(destinationType);

        }
    }
}
