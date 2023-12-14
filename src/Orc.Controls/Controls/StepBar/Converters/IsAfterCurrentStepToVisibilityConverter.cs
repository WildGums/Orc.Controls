namespace Orc.Controls;

public class IsAfterCurrentStepToVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        return !state.IsFlagSet(StepBarItemStates.IsBeforeCurrent)
               && !state.IsFlagSet(StepBarItemStates.IsCurrent);
    }
}
