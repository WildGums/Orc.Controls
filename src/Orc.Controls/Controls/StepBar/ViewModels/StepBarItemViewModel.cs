namespace Orc.Controls;

using Catel.MVVM;

public class StepBarItemViewModel : ViewModelBase
{
    public StepBarItemViewModel(IStepBarItem stepBarItem)
    {
        ValidateUsingDataAnnotations = false;

        Item = stepBarItem;
    }

    public IStepBarItem Item { get; private set; }
}
