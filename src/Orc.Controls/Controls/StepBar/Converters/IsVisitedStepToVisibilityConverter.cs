namespace Orc.Controls
{
    using System;
    using Catel;
    using Catel.MVVM.Converters;

    public class IsVisitedStepToVisibilityConverter : VisibilityConverterBase
    {
        public IsVisitedStepToVisibilityConverter()
            : base(System.Windows.Visibility.Hidden)
        {
        }

        protected override bool IsVisible(object value, Type targetType, object parameter)
        {
            var state = StepBarItemStates.None;

            if (value is StepBarItemStates)
            {
                state = (StepBarItemStates)value;
            }
            else
            {
                var stepBarItem = value as IStepBarItem;
                if (stepBarItem is not null)
                {
                    state = stepBarItem.State;
                }
            }

            // Skip current
            if (Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsCurrent))
            {
                return false;
            }

            return Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsVisited);
        }
    }
}
