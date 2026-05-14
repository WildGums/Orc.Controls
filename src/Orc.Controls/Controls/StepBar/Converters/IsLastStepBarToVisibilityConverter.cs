namespace Orc.Controls;

public partial class IsLastStepBarToVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        return state.IsFlagSet(StepBarItemStates.IsLast);
    }
}
