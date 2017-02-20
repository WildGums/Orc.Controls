// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel;

    internal static class WindowExtensions
    {
        public static Size GetSize(this Window window)
        {
            Argument.IsNotNull(() => window);

            return new Size(window.ActualWidth, window.ActualHeight);
        }
    }
}