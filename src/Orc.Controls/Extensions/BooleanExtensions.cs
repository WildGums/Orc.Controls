// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Extensions
{
    using System.Windows;

    internal static class BooleanExtensions
    {
        #region Methods
        public static Visibility ToVisibility(this bool value, Visibility hiddenState = Visibility.Collapsed)
        {
            return value ? Visibility.Visible : hiddenState;
        }
        #endregion
    }
}
