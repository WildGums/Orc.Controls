namespace Orc.Controls.Converters
{
    using System;
    using Catel.MVVM.Converters;

    internal class TimeSpanToTotalStringConverter : ValueConverterBase
    {
        protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            var timeSpan = ((TimeSpan?)value) is null ? TimeSpan.Zero : (TimeSpan)value;
            const string format = "F";

            if (parameter.Equals("Days"))
            {
                return timeSpan.TotalDays.ToString(format);
            }

            if (parameter.Equals("Hours"))
            {
                return timeSpan.TotalHours.ToString(format);
            }

            if (parameter.Equals("Minutes"))
            {
                return timeSpan.TotalMinutes.ToString(format);
            }

            return string.Empty;
        }
    }
}
