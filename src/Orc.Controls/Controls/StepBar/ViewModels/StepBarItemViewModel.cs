namespace Orc.Controls
{
    using Catel.MVVM;

    public class StepBarItemViewModel : ViewModelBase
    {
        public StepBarItemViewModel(IStepBarItem stepBarItem)
        {
            Item = stepBarItem;
        }

        public IStepBarItem Item { get; private set; }
    }
}
