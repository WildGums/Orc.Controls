// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToggleButtonExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2021 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows.Controls.Primitives;

    internal static class ToggleButtonExtensions
    {
        #region Methods
        public static void Toggle(this ToggleButton toggleButton)
        {
            toggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, !toggleButton.IsChecked);
        }
        #endregion
    }
}
