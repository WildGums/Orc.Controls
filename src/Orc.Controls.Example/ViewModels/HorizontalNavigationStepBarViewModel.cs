namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orc.Controls.Example.Models.StepBar;

    public class HorizontalNavigationStepBarViewModel : ViewModelBase
    {
        public IList<IStepBarItem> Items { get; } 

        public HorizontalNavigationStepBarViewModel()
        {
            var items = new List<IStepBarItem>()
            {
                new AgeExampleItem(),
                new AgeExampleItem(),
                new AgeExampleItem(),
                new AgeExampleItem(),
            };
            var num = 1;
            foreach (var page in items)
                page.Number = num++;
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
                ((StepBarViewModel)stepBar.ViewModel).MoveBackAsync();
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
                ((StepBarViewModel)stepBar.ViewModel).MoveForwardAsync();
            }
        }
    }
}
