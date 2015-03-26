// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowView.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.Views
{
    using System;
    using System.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView
    {
        #region Constructors
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        #endregion

        private void LogViewerControlOnLogRecordDoubleClick(object sender, LogEntryDoubleClickEventArgs e)
        {
            EventsTextBox.AppendText(e.LogEntry.Message + " clicked" + Environment.NewLine);
        }
    }
}