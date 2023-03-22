namespace Orc.Controls;

public class IsBeforeCurrentStepToVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        return state.IsFlagSet(StepBarItemStates.IsBeforeCurrent);
    }
}
