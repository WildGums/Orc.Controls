// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;

namespace Orc.Controls.Example.Views
{
    public partial class MainWindow
    {
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            CanCloseUsingEscape = false;
        }


        #endregion

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((CalloutView.PopupTimer is not null))
            {
                CalloutView.PopupTimer.Stop();
                if (CalloutTab.IsSelected)
                {
                    CalloutView.PopupTimer.Interval = TimeSpan.FromSeconds(5);
                    CalloutView.PopupTimer.Start();
                }
            }
        }
    }
}
