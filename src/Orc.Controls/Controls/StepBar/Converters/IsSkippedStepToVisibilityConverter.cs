namespace Orc.Controls;

public class IsSkippedStepToVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        return state.IsSkipped();
    }
}
