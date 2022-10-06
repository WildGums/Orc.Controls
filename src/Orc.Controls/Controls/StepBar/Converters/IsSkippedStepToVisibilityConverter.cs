namespace Orc.Controls
{
    using System;
    using Catel;
    using Catel.MVVM.Converters;

    public class IsSkippedStepToVisibilityConverter : VisibilityConverterBase
    {
        public IsSkippedStepToVisibilityConverter()
            : base(System.Windows.Visibility.Collapsed)
        {
        }

        protected override bool IsVisible(object? value, Type targetType, object? parameter)
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

            return state.IsSkipped();
        }
    }
}
