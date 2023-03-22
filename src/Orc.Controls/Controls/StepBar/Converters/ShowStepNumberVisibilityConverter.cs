namespace Orc.Controls;

public class ShowStepNumberVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        // Always show for current
        return state.IsFlagSet(StepBarItemStates.IsCurrent)
               || !state.IsFlagSet(StepBarItemStates.IsBeforeCurrent)
               && !state.IsFlagSet(StepBarItemStates.IsVisited);
    }
}   
