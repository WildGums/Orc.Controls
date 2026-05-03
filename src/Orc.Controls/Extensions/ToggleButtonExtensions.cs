namespace Orc.Controls;

using System;
using System.Windows.Controls.Primitives;

internal static class ToggleButtonExtensions
{
    public static void Toggle(this ToggleButton toggleButton)
    {
        ArgumentNullException.ThrowIfNull(toggleButton);

        toggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, !toggleButton.IsChecked);
    }
}