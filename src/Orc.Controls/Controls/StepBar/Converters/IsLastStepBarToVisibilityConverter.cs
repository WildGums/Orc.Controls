namespace Orc.Controls
{
    using System;
    using Catel;
    using Catel.MVVM.Converters;

    public class IsLastStepBarToVisibilityConverter : VisibilityConverterBase
    {
        public IsLastStepBarToVisibilityConverter()
            : base(System.Windows.Visibility.Hidden)
        {
        }

        protected override bool IsVisible(object value, Type targetType, object parameter)
        {
            var stepBarItem = value as IStepBarItem;
            if (stepBarItem is null)
            {
                return false;
            }

            return !Enum<StepBarItemStates>.Flags.IsFlagSet(stepBarItem.State, StepBarItemStates.IsLast);
        }
    }
}
