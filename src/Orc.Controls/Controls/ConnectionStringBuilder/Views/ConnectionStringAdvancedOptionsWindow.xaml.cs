// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringAdvancedOptionsWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
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
    }
}
