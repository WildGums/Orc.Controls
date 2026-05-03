namespace Orc.Controls;

using System;
using Catel.MVVM.Converters;

public class StepToOpacityConverter : ValueConverterBase
{
    protected override object Convert(object? value, Type targetType, object? parameter)
    {
        var state = value switch
        {
            StepBarItemStates states => states,
            IStepBarItem stepBarItem => stepBarItem.State,
            _ => StepBarItemStates.None
        };

        // Current is 1
        if (state.IsFlagSet(StepBarItemStates.IsCurrent))
        {
            return 1d;
        }

        return state.IsFlagSet(StepBarItemStates.IsVisited) 
            ? 0.75d 
            : 0.5d;
    }
}
