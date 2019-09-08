// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlToolManagerFactory.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Tools
{
    using System.Windows;

    public interface IControlToolManagerFactory
    {
        #region Methods
        IControlToolManager GetOrCreateManager(FrameworkElement frameworkElement);
        #endregion
    }
}
