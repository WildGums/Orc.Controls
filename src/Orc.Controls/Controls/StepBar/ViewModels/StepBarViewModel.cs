namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
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
            var items = Items;
            if (items is not null)
            {
                items.ForEach(x =>
                {
                    x.State = Enum<StepBarItemStates>.Flags.ClearFlag(x.State, StepBarItemStates.IsCurrent);
                    x.State = Enum<StepBarItemStates>.Flags.ClearFlag(x.State, StepBarItemStates.IsBeforeCurrent);
                });
            }

            var selectedItem = SelectedItem;
            if (selectedItem is not null)
            {
                selectedItem.State = Enum<StepBarItemStates>.Flags.SetFlag(selectedItem.State, StepBarItemStates.IsVisited);
                selectedItem.State = Enum<StepBarItemStates>.Flags.SetFlag(selectedItem.State, StepBarItemStates.IsCurrent);

                if (items is not null)
                {
                    foreach (var item in items)
                    {
                        if (ReferenceEquals(selectedItem, item))
                        {
                            break;
                        }

                        item.State = Enum<StepBarItemStates>.Flags.SetFlag(item.State, StepBarItemStates.IsBeforeCurrent);
                    }
                }
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
