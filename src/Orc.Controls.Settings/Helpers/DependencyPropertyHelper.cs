namespace Orc.Controls.Settings;

using System.Windows;

internal static class DependencyPropertyHelper
{
    public static bool IsPropertySet(DependencyObject obj, DependencyProperty property)
    {
        return obj.ReadLocalValue(property) != DependencyProperty.UnsetValue;
    }
}
