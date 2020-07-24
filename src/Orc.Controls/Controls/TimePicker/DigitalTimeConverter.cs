namespace Orc.Controls
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    public class DigitalTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DigitalTime)
            {
                var time = (DigitalTime)value;
                return time.ToTimeSpan().Ticks;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long)
            {
                return new DigitalTime(TimeSpan.FromTicks((long)value));
            }

            if (value is int)
            {
                return new DigitalTime(TimeSpan.FromTicks((int)value));
            }

            if (value is double)
            {
                return new DigitalTime(TimeSpan.FromTicks((long)((double)value)));
            }

            throw new NotSupportedException();
        }
    }
}

