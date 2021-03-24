namespace Orc.Controls
{
    using System;
    using Catel;
    using Catel.MVVM.Converters;

    public class ShowStepNumberVisibilityConverter : VisibilityConverterBase
    {
        public ShowStepNumberVisibilityConverter()
            : base(System.Windows.Visibility.Collapsed)
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

            // Always show for current
            if (Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsCurrent))
            {
                return true;
            }

            if (Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsBeforeCurrent))
            {
                return false;
            }

            if (Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsVisited))
            {
                return false;
            }

            return true;
        }
    }
}
