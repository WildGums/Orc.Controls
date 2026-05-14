namespace Orc.Controls;

public partial class IsSkippedStepToVisibilityConverter : StepBarVisibilityConverterBase
{
    protected override bool IsVisible(StepBarItemStates state)
    {
        return state.IsSkipped();
    }
}
