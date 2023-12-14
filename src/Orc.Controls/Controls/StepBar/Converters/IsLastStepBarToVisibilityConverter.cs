namespace Orc.Controls;

public class IsLastStepBarToVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        return state.IsFlagSet(StepBarItemStates.IsLast);
    }
}
