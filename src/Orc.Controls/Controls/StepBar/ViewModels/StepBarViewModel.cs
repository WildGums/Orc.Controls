namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;

    public class StepBarViewModel : ViewModelBase
    {
        #region Fields
        private int _currentIndex = 0;
        #endregion Fields

        #region Constructors
        public StepBarViewModel()
        {
            QuickNavigateToItem = new TaskCommand<IStepBarItem>(QuickNavigateToItemExecuteAsync, QuickNavigateToItemCanExecute);
        }
        #endregion Constructors

        #region Properties
        public IList<IStepBarItem> Items { get; set; }

        public IStepBarItem SelectedItem { get; set; }

        public bool AllowQuickNavigation { get; set; }
        #endregion Properties

        #region Methods
        public void MoveForwardAsync()
        {
            if (_currentIndex < Items.Count - 1)
            {
                Items[_currentIndex].IsVisited = true;
                _currentIndex++;
                SelectedItem = Items[_currentIndex];
                Items[_currentIndex].IsVisited = true;
            }
        }

        public void MoveBackAsync()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                SelectedItem = Items[_currentIndex];
            }
        }

        public void SetSelectedItem(int newIndex)
        {
            if (_currentIndex >= 0 && _currentIndex < Items.Count)
            {
                _currentIndex = newIndex;
                SelectedItem = Items[_currentIndex];
            }
        }

        public bool IsLastItem(IStepBarItem item)
        {
            return item.Equals(Items.LastOrDefault());
        }
        #endregion Methods

        #region Commands
        public TaskCommand<IStepBarItem> QuickNavigateToItem { get; private set; }

        public bool QuickNavigateToItemCanExecute(IStepBarItem parameter)
        {
            if (!AllowQuickNavigation)
            {
                return false;
            }

            if (!parameter.IsVisited)
            {
                return false;
            }

            if (SelectedItem == parameter)
            {
                return false;
            }

            return true;
        }

        public async Task QuickNavigateToItemExecuteAsync(IStepBarItem parameter)
        {
            var item = parameter;
            if (item != null && item.IsVisited && Items is List<IStepBarItem>)
            {
                var index = Items.IndexOf(item);

                SetSelectedItem(index);
            }
        }
        #endregion Commands
    }
}
