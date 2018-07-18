// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewer.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.Views
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using Orc.Controls.Example.ViewModels;

    public partial class LogViewer
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        protected override async void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (ViewModel != null)
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
            var vm = ViewModel as LogViewerViewModel;
            if (vm != null)
            {
                var filterGroups = await vm.GetLogFilterGroupsAsync();

                ActiveFilterGroupComboBox.SetCurrentValue(System.Windows.Controls.ItemsControl.ItemsSourceProperty, filterGroups);
            }
        }
    }
}
