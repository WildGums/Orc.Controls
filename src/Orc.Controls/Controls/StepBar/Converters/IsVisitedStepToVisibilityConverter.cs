namespace Orc.Controls;

public class IsVisitedStepToVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        return !state.IsFlagSet(StepBarItemStates.IsCurrent)
               && state.IsFlagSet(StepBarItemStates.IsVisited);
    }
}
