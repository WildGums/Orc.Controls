// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogUserControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.MVVM.Views;

    public partial class LogUserControl
    {
        static LogUserControl()
        {
            typeof (LogUserControl).AutoDetectViewPropertiesToSubscribe();
        }

        public LogUserControl()
        {
            InitializeComponent();
        }
    }
}