namespace Orc.Controls
{
    using System;
    using Catel;
    using Catel.MVVM.Converters;

    public class StepToOpacityConverter : ValueConverterBase
    {
        public StepToOpacityConverter()
        {
        }

        protected override object Convert(object value, Type targetType, object parameter)
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

            // Current is 1
            if (Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsCurrent))
            {
                return 1d;
            }

            if (Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsVisited))
            {
                return 0.75d;
            }

            return 0.5d;
        }
    }
}
