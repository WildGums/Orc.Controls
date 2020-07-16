namespace Orc.Controls.Example.Converters
{
    using System;
    using System.Windows;
    using Catel.MVVM.Converters;
    using Orc.Controls;

    public class ScrollModeEnumToBooleanConverter : ValueConverterBase<ScrollMode, bool>
    {
        protected override object Convert(ScrollMode value, Type targetType, object parameter)
        {
            string comparedEnumString = parameter as string;

            if (Enum.TryParse<ScrollMode>(comparedEnumString, out var scrollMode))
            {
                return value == scrollMode;
            }

            return DependencyProperty.UnsetValue;
        }

        protected override object ConvertBack(bool value, Type targetType, object parameter)
        {
            string comparedEnumString = parameter as string;

            if (!value || string.IsNullOrEmpty(comparedEnumString))
            {
                return DependencyProperty.UnsetValue;
            }

            Enum.TryParse<ScrollMode>(comparedEnumString, out var scrollMode);

            return scrollMode;
        }
    }
}
