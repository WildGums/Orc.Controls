namespace Orc.Controls;

public class IsCurrentStepToVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        return state.IsFlagSet(StepBarItemStates.IsCurrent);
    }
}
