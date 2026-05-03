namespace Orc.Controls;

using System;
using Catel.MVVM;

public class StepBarItemViewModel : ViewModelBase
{
    public StepBarItemViewModel(IStepBarItem stepBarItem, IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        ValidateUsingDataAnnotations = false;

        Item = stepBarItem;
    }

    public IStepBarItem Item { get; private set; }
}
