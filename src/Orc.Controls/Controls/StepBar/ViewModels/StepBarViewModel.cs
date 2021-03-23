namespace Orc.Controls
{
    using System.Collections.Generic;
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
        }
        #endregion Constructors

        #region Properties
        public IList<IStepBarItem> Items { get; set; }

        public IStepBarItem SelectedItem { get; set; }
        #endregion Properties

        #region Methods
        public void MoveForward()
        {
            SetSelectedItem(_currentIndex + 1);
        }

        public void MoveBack()
        {
            SetSelectedItem(_currentIndex - 1);
        }

        public void SetSelectedItem(int newIndex)
        {
            if (Items != null && newIndex >= 0 && newIndex < Items.Count)
            {
                Items[_currentIndex].State |= StepBarItemStates.IsVisited;
                _currentIndex = newIndex;
                SelectedItem = Items[_currentIndex];
                SelectedItem.State |= StepBarItemStates.IsVisited;
            }
        }

        protected override Task InitializeAsync()
        {
            if (Items != null && Items.Count > 0)
            {
                Items[Items.Count - 1].State |= StepBarItemStates.IsLast;
                SetSelectedItem(0);
            }
            return Task.CompletedTask;
        }
        #endregion Methods
    }
}
