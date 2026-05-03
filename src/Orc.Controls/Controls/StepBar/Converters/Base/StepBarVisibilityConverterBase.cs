namespace Orc.Controls;

using System;
using Catel.MVVM.Converters;

public abstract class StepBarVisibilityConverterBase : VisibilityConverterBase
{
    protected StepBarVisibilityConverterBase()
        : base(System.Windows.Visibility.Collapsed)
    {
    }

    protected override bool IsVisible(object? value, Type targetType, object? parameter)
    {
        var state = value switch
        {
            StepBarItemStates states => states,
            IStepBarItem stepBarItem => stepBarItem.State,
            _ => StepBarItemStates.None
        };

        return IsVisible(state);
    }

    protected abstract bool IsVisible(StepBarItemStates state);
}
