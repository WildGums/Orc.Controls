// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewer.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.Views
{
    using System;
    using System.Windows;

    public partial class LogViewer
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        private void LogViewerControlOnLogRecordDoubleClick(object sender, LogEntryDoubleClickEventArgs e)
        {
            EventsTextBox.AppendText(e.LogEntry.Message + " clicked" + Environment.NewLine);
        }

        private void ClearLog_OnClick(object sender, RoutedEventArgs e)
        {
            LogViewerControl.Clear();
        }
    }
}