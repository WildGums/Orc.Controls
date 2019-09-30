// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringAdvancedOptionsWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Windows;

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
