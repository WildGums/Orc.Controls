// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringEditWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    public sealed partial class ConnectionStringEditWindow
    {
        #region Constructors
        public ConnectionStringEditWindow()
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
