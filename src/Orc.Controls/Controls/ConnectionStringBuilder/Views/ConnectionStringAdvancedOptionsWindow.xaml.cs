// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringAdvancedOptionsWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Windows;

    [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use ConnectionStringAdvancedOptionsWindow from Orc.DataAccess.Xaml library instead")]
    public sealed partial class ConnectionStringAdvancedOptionsWindow
    {
        #region Constructors
        public ConnectionStringAdvancedOptionsWindow()
            : base(DataWindowMode.Close)
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override void OnLoaded(EventArgs e)
        {
            base.OnLoaded(e);

            this.LoadWindowSize(true);
        }

        protected override void OnUnloaded(EventArgs e)
        {
            this.SaveWindowSize();

            base.OnUnloaded(e);
        }
        #endregion
    }
}
