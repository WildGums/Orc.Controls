namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    internal class AmPmLongToAmPmShortConverter : ValueConverterBase
    {
        public AmPmLongToAmPmShortConverter()
        {
        }

        public bool IsEnabled { get; set; }

        protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            if (!IsEnabled || value is null)
            {
                return value;
            }

            switch (value)
            {
                case Meridiems.LongAM:
                    return Meridiems.ShortAM;

                case Meridiems.LongPM:
                    return Meridiems.ShortPM;

                default:
                    return value;
            }
        }

        protected override object? ConvertBack(object? value, Type targetType, object? parameter)
        {
            if (!IsEnabled || value is null)
            {
                return value;
            }

            switch (value)
            {
                case Meridiems.ShortAM:
                    return Meridiems.LongAM;

                case Meridiems.ShortPM:
                    return Meridiems.LongPM;

                default:
                    return value;
            }
        }
    }
}
