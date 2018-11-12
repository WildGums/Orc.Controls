// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel;

    internal static class WindowExtensions
    {
        #region Methods
        public static Size GetSize(this Window window)
        {
            Argument.IsNotNull(() => window);

            return new Size(window.ActualWidth, window.ActualHeight);
        }
        #endregion
    }
}
