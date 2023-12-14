namespace Orc.Controls.Example.Views
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using ViewModels;

    public partial class LogViewer
    {
        #region Constructors
        public LogViewer()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override async void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (ViewModel is not null)
            {
                await UpdateLogFilterGroupsAsync();
            }
        }

        private void LogViewerControlOnLogRecordDoubleClick(object sender, LogEntryDoubleClickEventArgs e)
        {
            EventsTextBox.AppendText(e.LogEntry.Message + " clicked" + Environment.NewLine);
        }

        private void ClearLog_OnClick(object sender, RoutedEventArgs e)
        {
            LogViewerControl.Clear();
        }

        private async void OnLogFilterGroupListUpdated(object sender, EventArgs e)
        {
            await UpdateLogFilterGroupsAsync();
        }

        private async Task UpdateLogFilterGroupsAsync()
        {
            if (ViewModel is LogViewerViewModel vm)
            {
                var filterGroups = await vm.GetLogFilterGroupsAsync();

                ActiveFilterGroupComboBox.SetCurrentValue(System.Windows.Controls.ItemsControl.ItemsSourceProperty, filterGroups);
            }
        }
        #endregion
    }
}
