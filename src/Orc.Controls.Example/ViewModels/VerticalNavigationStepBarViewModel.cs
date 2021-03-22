namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.MVVM;

    public class VerticalNavigationStepBarViewModel : ViewModelBase
    {
        public IList<IStepBarItem> Items { get; }

        public VerticalNavigationStepBarViewModel()
        {
            var items = new List<IStepBarItem>()
            {
                new StepBarItemBase { Title = "Person", Number = 1 },
                new StepBarItemBase { Title = "Age", Number = 2 },
                new StepBarItemBase { Title = "Skills", Number = 3 },
                new StepBarItemBase { Title = "Gadgets", Number = 4 },
            };
            Items = items;

            MoveBackItem = new TaskCommand<StepBar>(MoveBackItemExecuteAsync, MoveBackItemCanExecute);
            MoveForwardItem = new TaskCommand<StepBar>(MoveForwardItemExecuteAsync, MoveForwardCanExecute);
        }

        public TaskCommand<StepBar> MoveBackItem { get; private set; }

        public bool MoveBackItemCanExecute(StepBar stepBar)
        {
            return true;
        }

        public async Task MoveBackItemExecuteAsync(StepBar stepBar)
        {
            if (stepBar.ViewModel is StepBarViewModel)
            {
                ((StepBarViewModel)stepBar.ViewModel).MoveBack();
            }
        }

        public TaskCommand<StepBar> MoveForwardItem { get; private set; }

        public bool MoveForwardCanExecute(StepBar stepBar)
        {
            return true;
        }

        public async Task MoveForwardItemExecuteAsync(StepBar stepBar)
        {
            if (stepBar.ViewModel is StepBarViewModel)
            {
                ((StepBarViewModel)stepBar.ViewModel).MoveForward();
            }
        }
    }
}
