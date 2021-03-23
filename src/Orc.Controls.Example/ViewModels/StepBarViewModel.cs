namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Catel;
    using Catel.MVVM;

    public class StepBarViewModel : ViewModelBase
    {
        public StepBarViewModel()
        {
            AvailableOrientations = Enum<Orientation>.GetValues().ToList();

            SelectNewItem = new TaskCommand<IStepBarItem>(OnSelectNewItemExecuteAsync);
        }

        public TaskCommand<IStepBarItem> SelectNewItem { get; private set; }

        private async Task OnSelectNewItemExecuteAsync(IStepBarItem item)
        {
            SelectedItem = item;
        }

        public List<Orientation> AvailableOrientations { get; private set; }

        public Orientation SelectedOrientation { get; set; }

        public List<IStepBarItem> Items { get; private set; }

        public IStepBarItem SelectedItem { get; set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var items = new List<IStepBarItem>();

            for (var i = 0; i < 10; i++)
            {
                items.Add(new StepBarItemModel
                {
                    Title = $"Item {i + 1}",
                    Number = i + 1,
                    Command = SelectNewItem
                });
            }

            Items = items;
        }
    }

    public class StepBarItemModel : StepBarItemBase
    {

    }
}
