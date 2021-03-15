namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Models;
    using Orc.Controls.Controls.StepBar.ViewModels;
    using Orc.Controls.Controls.StepBar.Views;

    public class SideNavigationStepBarViewModel : ViewModelBase
    {
        public IList<IStepBarItem> Items { get; }

        public SideNavigationStepBarViewModel()
        {
            var items = new List<IStepBarItem>()
            {
                new AgeExampleItem(),
                new AgeExampleItem(),
                new AgeExampleItem(),
                new AgeExampleItem(),
            };
            var num = 0;
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
                await ((StepBarViewModel)stepBar.ViewModel).MoveBackAsync();
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
                await ((StepBarViewModel)stepBar.ViewModel).MoveForwardAsync();
            }
        }
    }
}
