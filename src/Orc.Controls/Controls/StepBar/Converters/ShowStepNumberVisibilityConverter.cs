namespace Orc.Controls;

public partial class ShowStepNumberVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        // Always show for current
        return state.IsFlagSet(StepBarItemStates.IsCurrent)
               || !state.IsFlagSet(StepBarItemStates.IsBeforeCurrent)
               && !state.IsFlagSet(StepBarItemStates.IsVisited);
    }
}   
