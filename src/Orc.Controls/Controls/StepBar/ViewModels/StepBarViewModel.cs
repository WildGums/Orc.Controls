namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.MVVM;

    public class StepBarViewModel : ViewModelBase
    {
        public StepBarViewModel()
        {
        }

        public IList<IStepBarItem> Items { get; set; }

        public IStepBarItem SelectedItem { get; set; }

        protected override async Task InitializeAsync()
        {
            UpdateSelection();
        }

        private void OnItemsChanged()
        {
            UpdateSelection();
        }

        private void OnSelectedItemChanged()
        {
            var item = SelectedItem;
            if (item is not null)
            {
                item.State |= StepBarItemStates.IsLast;
            }
        }

        private void UpdateSelection()
        {
            IStepBarItem selectedItem = null;

            var items = Items;
            if (items is not null)
            {
                if (items.Count > 0)
                {
                    selectedItem = Items[Items.Count - 1];
                }
            }

            SelectedItem = selectedItem;
        }
    }
}
